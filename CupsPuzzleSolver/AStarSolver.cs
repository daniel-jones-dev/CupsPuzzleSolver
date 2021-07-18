using System;
using System.Collections.Generic;
using C5;

namespace CupsPuzzleSolver
{
    internal class StateCostComparer : Comparer<(float f, Cups state)>
    {
        public override int Compare((float f, Cups state) x, (float f, Cups state) y)
        {
            return x.f.CompareTo(y.f);
        }
    }

    public class AStarSolver
    {
        // Store collection, foreach state: g (moves to reach), f (g + heuristic), and the previous state
        private readonly Dictionary<Cups, (int g, float f, Cups? prev)> _explored;

        // Queue of states to be visited
        private readonly IntervalHeap<(float f, Cups state)> _openSet;

        private (int, Cups?) _bestSolution;

        public AStarSolver(Cups start)
        {
            start.CheckValid();
            var h = start.EstimatedMoves();
            _explored = new Dictionary<Cups, (int, float, Cups?)> {{start, (0, h, null)}};
            _openSet = new IntervalHeap<(float, Cups)>(new StateCostComparer()) {(h, start)};
        }

        public bool Step()
        {
            // Take the next state to be explored
            if (_openSet.Count == 0)
            {
                Console.WriteLine("Fully explored tree");
                return false;
            }

            var (f, currState) = _openSet.FindMin();
            _openSet.DeleteMin();
            var (currMoves, _, _) = _explored[currState];

            // Console.WriteLine("==============================================");
            // Console.WriteLine("Checking state: (f: " + f + ")");
            // currState.PrintState();

            if (currState.Solved())
            {
                var (bestMoveCount, bestSolution) = _bestSolution;
                if (bestSolution == null || currMoves < bestMoveCount)
                {
                    Console.WriteLine("Found a solution with " + currMoves + " moves.");
                    _bestSolution = (currMoves, currState);
                    Console.WriteLine("Explored " + (_explored.Count + 1) + " states.");
                    return false;
                }
            }
            else
            {
                // Enumerate possible moves from this state, and add them all to the to-explore list
                var moveList = currState.GetPossibleMoves();
                // Console.WriteLine(moveList.Count + " possible moves");
                foreach (var (from, to) in moveList)
                {
                    // Console.WriteLine("Pouring cup " + from + " into cup " + to + ", resulting in:");
                    var nextState = (Cups) currState.Clone();
                    nextState.Move(from, to);
                    nextState.CheckValid();
                    // nextState.PrintState();
                    var g = currMoves + 1;

                    if (_explored.TryGetValue(nextState, out (int, float, Cups?) existingEntry))
                    {
                        var (existingEntryMoves, _, existingEntryPrev) = existingEntry;
                        if (g >= existingEntryMoves) continue;
                        // Console.WriteLine("Improving cost of an explored state (was: " + existingEntryMoves +
                        //                   ", now: " + g + "), found from:");
                        // existingEntryPrev?.PrintState();

                        _explored.Remove(nextState);
                    }

                    var nextH = nextState.EstimatedMoves();
                    var nextF = g + nextH;
                    // Console.WriteLine("g: " + g + ", h: " + nextH + ", f: " + nextF);
                    _explored.Add(nextState, (g, nextF, currState));
                    _openSet.Add((nextF, nextState));
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
                var (moves, f, prevState) = _explored[currState];

                if (prevState == null) break;
                currState = prevState;
            }

            Console.WriteLine(solution.Count - 1 + " moves in solution");
            foreach (var state in solution) state.PrintState();
        }
    }
}