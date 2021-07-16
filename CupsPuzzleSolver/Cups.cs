using System;
using System.Collections.Generic;

namespace CupsPuzzleSolver
{
    public class Cups
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