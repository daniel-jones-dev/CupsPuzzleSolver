using System;
using System.Collections.Generic;
using System.Linq;

namespace CupsPuzzleSolver
{
    public class Cups : ICloneable
    {
        private readonly Cup[] cups;

        public Cups(string[] cupContents)
        {
            cups = new Cup[cupContents.Length];
            for (var i = 0; i < cupContents.Length; ++i)
            {
                var cupContent = cupContents[i];
                cups[i] = new Cup(cupContent);
            }
        }

        public Cups(string content)
            : this(content.Split(','))
        {
        }

        #region ICloneable Members

        public object Clone()
        {
            string[] content = new string[cups.Length];
            for (var i = 0; i < cups.Length; i++) content[i] = cups[i].Content;

            return new Cups(content);
        }

        #endregion

        public string GetStateString()
        {
            var result = "";

            for (var cupIndex = 0; cupIndex < cups.Length; ++cupIndex)
            {
                result += " " + cupIndex + " ";
                result += cupIndex + 1 < cups.Length ? " " : "\n";
            }

            for (var withinCupIndex = Cup.MaxSize - 1; withinCupIndex >= 0; --withinCupIndex)
            for (var cupIndex = 0; cupIndex < cups.Length; ++cupIndex)
            {
                result += "|";
                if (cups[cupIndex].Volume > withinCupIndex)
                    result += cups[cupIndex].Color(withinCupIndex);
                else
                    result += " ";
                result += "|";

                result += cupIndex + 1 < cups.Length ? " " : "\n";
            }

            for (var cupIndex = 0; cupIndex < cups.Length; ++cupIndex)
            {
                result += " ‾ ";
                result += cupIndex + 1 < cups.Length ? " " : "\n";
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
            return string.Join(',', from cup in cups orderby cup.Content select cup.Content);
        }

        public List<(int, int)> GetPossibleMoves()
        {
            var result = new List<(int, int)>();
            for (var i = 0; i < cups.Length; ++i)
            for (var j = 0; j < cups.Length; ++j)
                if (i != j && cups[i].CanPourInto(cups[j]))
                    result.Add((i, j));
            return result;
        }

        public void Move(int from, int to)
        {
            cups[from].PourInto(cups[to]);
        }

        public void CheckValid()
        {
            var colorCount = new Dictionary<char, int>();
            for (var i = 0; i < cups.Length; i++)
            {
                var cup = cups[i];
                if (cup.Volume > Cup.MaxSize)
                    throw new Exception("Cup " + i + " exceeds valid size (" + Cup.MaxSize + "), has size of " +
                                        cup.Volume + ".");
                for (var j = 0; j < cup.Volume; ++j)
                {
                    var color = cup.Color(j);
                    if (!colorCount.ContainsKey(color)) colorCount.Add(color, 0);
                    colorCount[color]++;
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
            foreach (var cup in cups)
            foreach (var color in cup.DistinctColors())
                if (!colorAppearances.ContainsKey(color)) colorAppearances.Add(color, 1);
                else colorAppearances[color]++;

            var cost = 0.0F;
            foreach (var (_, count) in colorAppearances) cost += count - 1;
            return cost;
        }

        public bool Solved()
        {
            foreach (var cup in cups)
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
            foreach (var (from, to) in moves) Console.WriteLine("Can pour cup " + from + " into cup " + to);
        }
    }
}