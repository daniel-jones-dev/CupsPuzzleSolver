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
            for (var i = 0; i < cupContents.Length; ++i) cups[i] = new Cup(cupContents[i]);
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
            List<(int, int)> result = new List<(int, int)>();
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
        
        public void PrintState()
        {
            Console.WriteLine(GetStateString());
        }

        public void PrintMoves()
        {
            var moves = GetPossibleMoves();
            foreach ((var from, var to) in moves)
            {
                Console.WriteLine("Can pour cup " + from + " into cup " + to);
            }
        }
    }

    internal class Program
    {
        private static void Main(string[] args)
        {
            string[] cupContents = {"", "POPO", "OPOP"};
            
            
            
            var cups = new Cups(cupContents);
            cups.PrintState();
            cups.PrintMoves();

            Console.WriteLine("Pouring cup 1 into cup 0");
            cups.Move(1,0);
            cups.PrintState();
            cups.PrintMoves();
            
            
        }
    }
}