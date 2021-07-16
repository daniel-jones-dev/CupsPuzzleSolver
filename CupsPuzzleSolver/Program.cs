using System;

namespace CupsPuzzleSolver
{
    public class Solver
    {
        private readonly Cups state;
        private int moveCount = 0;

        public Solver(Cups start)
        {
            state = start;
            state.PrintState();
            state.CheckValid();
        }

        public bool Step()
        {
            if (state.Solved())
            {
                Console.WriteLine("Solved with "+moveCount + " moves.");
                return false;
            }
            
            var moves = state.GetPossibleMoves();
            if (moves.Count > 0)
            {
                state.PrintMoves();
                var from = moves[0].Item1;
                var to = moves[0].Item2;
                Console.WriteLine("Move " + moveCount + ": Pouring cup " + from + " into cup " + to);
                state.Move(from, to);
                moveCount++;
                state.PrintState();
                state.CheckValid();
                return true;
            }

            Console.WriteLine("No possible moves left, took "+moveCount + " moves.");
            return false;
        }
    }

    internal class Program
    {
        private static void Main(string[] args)
        {
            string[] cupContents = {"", "POPO", "OPOP"};
            var cups = new Cups(cupContents);

            var solver = new Solver(cups);
            while (solver.Step())
            {
            }
        }
    }
}