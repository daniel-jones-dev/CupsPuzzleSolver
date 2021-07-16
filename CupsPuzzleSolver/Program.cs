﻿namespace CupsPuzzleSolver
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var puzzle1 = "RRR,R";
            var puzzle2 = "-,POPO,OPOP";
            var puzzle3 = "YYYB,DDBB,DDBY,-,-"; // Note: second empty cup is not needed
            var puzzle4 = "YYBO,BBYO,OYBO,-,-"; // Note: second empty cup is not needed
            var puzzle5 = "BYYB,DOOD,DYOB,OYDB,-,-"; // Note: second empty cup is not needed
            var puzzle6 = "YYBP,YpBP,pPpB,PpBY,-,-";
            var puzzle7 = "GuPP,PODu,DGPG,ODuG,OuOD,-,-";
            var puzzle8 = "YOPG,OGYu,GYPP,GuOu,YOuP,-,-";
            var puzzle9 = "OBYG,BODB,OGOY,DGBD,YDGY,-,-";

            var cups = new Cups(puzzle9);

            var solver = new NaiveSolver(cups);
            while (solver.Step())
            {
            }

            solver.PrintBestSolution();
        }
    }
}