using System;
using System.Collections.Generic;
using System.Linq;

namespace CupsPuzzleSolver
{
    public class Cups : ICloneable // TODO rename to CupPuzzle
    {
        private readonly List<Cup[]> _cupRows;

        public Cups(string content)
        {
            _cupRows = new List<Cup[]>();
            var rowsContent = content.Split('|');
            var numRows = rowsContent.Length;
            foreach (var rowContent in rowsContent)
            {
                var cupContents = rowContent.Split(',');
                var cupRow = new Cup[cupContents.Length];
                for (var i = 0; i < cupContents.Length; ++i)
                {
                    var cupContent = cupContents[i];
                    cupRow[i] = new Cup(cupContent);
                }

                _cupRows.Add(cupRow);
            }
            // TODO check taps are valid, and do not clash with lids
        }

        #region ICloneable Members

        public object Clone()
        {
            return new Cups(ToString());
        }

        #endregion

        public override string ToString()
        {
            return string.Join("|",
                from cupRow in _cupRows select (string.Join(",", from cup in cupRow select cup.ToString())));
        }

        public string GetStateString()
        {
            var result = "";

            for (var rowIdx = 0; rowIdx < _cupRows.Count; rowIdx++)
            {
                var cupRow = _cupRows[rowIdx];
                if (rowIdx > 0) result += "\n";

                // Index number
                for (var cupIndex = 0; cupIndex < cupRow.Length; ++cupIndex)
                {
                    result += " " + cupIndex + " ";
                    result += (cupIndex + 1 < cupRow.Length) ? " " : "\n";
                }

                // Lids
                for (var cupIndex = 0; cupIndex < cupRow.Length; ++cupIndex)
                {
                    var cup = cupRow[cupIndex];
                    result += " " + (cup.HasLid || (cup.NumTopColors() == Cup.MaxSize) ? "_" : " ") + " ";
                    result += (cupIndex + 1 < cupRow.Length) ? " " : "\n";
                }

                // Cup contents
                for (var withinCupIndex = Cup.MaxSize - 1; withinCupIndex >= 0; --withinCupIndex)
                for (var cupIndex = 0; cupIndex < cupRow.Length; ++cupIndex)
                {
                    result += "|";
                    if (cupRow[cupIndex].Volume > withinCupIndex)
                        result += cupRow[cupIndex].Color(withinCupIndex);
                    else
                        result += " ";
                    result += "|";

                    result += cupIndex + 1 < cupRow.Length ? " " : "\n";
                }

                // Base or tap
                for (var cupIndex = 0; cupIndex < cupRow.Length; ++cupIndex)
                {
                    var cup = cupRow[cupIndex];
                    result += " " + (cup.HasTap ? "v" : "‾") + " ";
                    result += cupIndex + 1 < cupRow.Length ? " " : "";
                }
            }

            return result;
        }

        public override int GetHashCode()
        {
            return GetShortStateString().GetHashCode();
        }

        public override bool Equals(object? obj)
        {
            if (obj == null) return false;
            return GetShortStateString() == ((Cups) obj).GetShortStateString();
        }

        public string GetShortStateString()
        {
            // TODO needs to be updated to reflect that cups with taps or lids are not interchangeable
            return ToString();
            //return string.Join(',', from cup in cupRows select cup.Content);
        }

        public List<Move> GetPossibleMoves()
        {
            var possibleMoves = new List<Move>();
            for (var fromRow = 0; fromRow < _cupRows.Count; ++fromRow)
            for (var fromCupIndex = 0; fromCupIndex < _cupRows[fromRow].Length; ++fromCupIndex)
            {
                var fromCup = _cupRows[fromRow][fromCupIndex];
                if (fromCup.HasTap)
                {
                    var toCup = _cupRows[fromRow + 1][fromCupIndex];
                    if (fromCup.CanPourInto(toCup, tap: true))
                        possibleMoves.Add(new Move {FromCupIndex = fromCupIndex, FromRowIndex = fromRow, Tap = true});
                }

                for (var toRow = 0; toRow < _cupRows.Count; ++toRow)
                for (var toCupIndex = 0; toCupIndex < _cupRows[toRow].Length; ++toCupIndex)
                {
                    var toCup = _cupRows[toRow][toCupIndex];
                    if ((fromRow != toRow || fromCupIndex != toCupIndex) && fromCup.CanPourInto(toCup))
                    {
                        possibleMoves.Add(new Move
                        {
                            FromCupIndex = fromCupIndex, FromRowIndex = fromRow, Tap = false, ToCupIndex = toCupIndex,
                            ToRowIndex = toRow
                        });
                    }
                }
            }

            return possibleMoves;
        }

        public void Move(Move move)
        {
            var fromCup = _cupRows[move.FromRowIndex][move.FromCupIndex];
            if (move.Tap)
            {
                // TODO                
            }
            else
            {
                var toCup = _cupRows[move.ToRowIndex][move.ToCupIndex];
                fromCup.PourInto(toCup);
            }
        }

        public void CheckValid()
        {
            var colorCount = new Dictionary<char, int>();
            for (var rowIdx = 0; rowIdx < _cupRows.Count; rowIdx++)
            {
                var cupRow = _cupRows[rowIdx];
                for (var cupIdx = 0; cupIdx < cupRow.Length; ++cupIdx)
                {
                    var cup = cupRow[cupIdx];
                    if (cup.Volume > Cup.MaxSize)
                        throw new Exception(
                            $"Cup (row {rowIdx}, cup{cupIdx}) exceeds valid size ({Cup.MaxSize}), has size of {cup.Volume}.");
                    for (var colorIdx = 0; colorIdx < cup.Volume; ++colorIdx)
                    {
                        var color = cup.Color(colorIdx);
                        if (!colorCount.ContainsKey(color)) colorCount.Add(color, 0);
                        colorCount[color]++;
                    }
                }
            }

            foreach (var (color, count) in colorCount)
                if (count != Cup.MaxSize)
                    throw new Exception("Color " + color + " has an invalid count of " + count + " (expected " +
                                        Cup.MaxSize + ")");
        }

        public float EstimatedMoves()
        {
            // Estimate the number of moves to complete this state.
            // The heuristic must be admissible (i.e. never overestimate) to guarantee the best solution will be found.

            Dictionary<char, int> colorAppearances = new();
            foreach (var cupRow in _cupRows)
            foreach (var cup in cupRow)
            foreach (var color in cup.DistinctColors())
                if (!colorAppearances.ContainsKey(color)) colorAppearances.Add(color, 1);
                else colorAppearances[color]++;

            var cost = 0.0F;
            foreach (var (_, count) in colorAppearances) cost += count - 1;
            return cost;
        }

        public bool Solved()
        {
            foreach (var cupRow in _cupRows)
            foreach (var cup in cupRow)
                switch (cup.NumTopColors())
                {
                    case 0:
                    case Cup.MaxSize:
                        continue;
                    default:
                        return false;
                }

            return true;
        }

        public void PrintState()
        {
            Console.WriteLine(GetStateString());
        }

        public void PrintMoves()
        {
            var moves = GetPossibleMoves();
            foreach (var move in moves)
            {
                if (move.Tap)
                {
                    Console.WriteLine($"Can tap cup {move.FromCupIndex} in row {move.FromRowIndex}");
                }
                else
                {
                    Console.WriteLine(
                        $"Can pour cup {move.FromCupIndex} in row {move.FromRowIndex} into " +
                        $"cup {move.ToCupIndex} in row {move.ToRowIndex}");
                }
            }
        }
    }
}