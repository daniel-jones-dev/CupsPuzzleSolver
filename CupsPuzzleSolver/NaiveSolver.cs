using System;
using System.Collections.Generic;

namespace CupsPuzzleSolver
{
    public class NaiveSolver
    {
        // Store collection, foreach state: moves to reach, and the previous state
        private readonly Dictionary<Cups, (int moves, Cups? prev)> _explored;

        // List of states yet to be explored, the steps to get there, and the state we reached them from
        private readonly List<(Cups state, int moves, Cups? prev)> _toExplore;

        private (int, Cups?) _bestSolution;

        public NaiveSolver(Cups start)
        {
            start.CheckValid();
            _toExplore = new List<(Cups, int, Cups?)> {(start, 0, null)};
            _explored = new Dictionary<Cups, (int, Cups?)>();
        }

        private bool Explore(Cups to, Cups? from)
        {
            if (from == null)
            {
                _explored.Add(to, (0, null));
                return true;
            }

            var (fromCost, _) = _explored[from];

            if (_explored.ContainsKey(to))
            {
                var (currCost, _) = _explored[to];
                if (fromCost + 1 < currCost)
                {
                    _explored[to] = (fromCost + 1, from);
                    return true;
                }
            }
            else
            {
                _explored.Add(to, (fromCost + 1, from));
                return true;
            }

            return false;
        }

        public bool Step()
        {
            // Take the next state to be explored
            if (_toExplore.Count == 0)
            {
                Console.WriteLine("Fully explored tree");
                return false;
            }

            var (currState, currMoves, prevState) = _toExplore[0];
            _toExplore.RemoveAt(0);

            // Console.WriteLine("==============================================");
            // Console.WriteLine("Checking state:");
            // currState.PrintState();

            if (_explored.TryGetValue(currState, out (int, Cups?) foundEntry))
            {
                var (foundMoves, _) = foundEntry;
                if (currMoves >= foundMoves)
                    // Console.WriteLine("State already encountered");
                    return true;

                _explored[currState] = (currMoves, prevState);
            }
            else
            {
                _explored.Add(currState, (currMoves, prevState));
                Console.WriteLine(_explored.Count + " states have been explored");
            }

            if (currState.Solved())
            {
                var (bestMoveCount, bestSolution) = _bestSolution;
                if (bestSolution == null || currMoves < bestMoveCount)
                {
                    Console.WriteLine("Found a solution with " + currMoves + " moves.");
                    _bestSolution = (currMoves, currState);
                }
            }
            else
            {
                // Enumerate possible moves from this state, and add them all to the to-explore list
                var moveList = currState.GetPossibleMoves();
                // Console.WriteLine(moveList.Count + " possible moves");
                foreach (var move in moveList)
                {
                    // Console.WriteLine("Pouring cup " + from + " into cup " + to + ", resulting in:");
                    var nextState = (Cups) currState.Clone();
                    nextState.Move(move);
                    nextState.CheckValid();
                    // nextState.PrintState();
                    _toExplore.Add((nextState, currMoves + 1, currState));
                }
            }

            return true;
        }

        public void PrintBestSolution()
        {
            var (_, solvedState) = _bestSolution;
            if (solvedState == null)
            {
                Console.WriteLine("No solution found.");
                return;
            }
            var currState = solvedState;
            List<Cups> solution = new();

            while (true)
            {
                solution.Insert(0, currState);
                var (moves, prevState) = _explored[currState];

                if (prevState == null) break;
                currState = prevState;
            }

            Console.WriteLine(solution.Count - 1 + " moves in solution");
            foreach (var state in solution) state.PrintState();
        }
    }
}