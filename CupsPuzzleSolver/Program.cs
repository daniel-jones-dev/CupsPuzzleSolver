namespace CupsPuzzleSolver
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            // Colors:
            // (R)ed, (P)ink, (O)range, (Y)ellow, (p)urple,
            // B(r)own, (V)iolet (darker purple),
            // (G)reen, Light (g)reen, (C)yan
            // (B)lue, Light (b)lue
            var puzzle1 = "RRR,R";
            var puzzle2 = "-,POPO,OPOP";
            var puzzle3 = "YYYb,BBbb,BBbY,-,-"; // Note: second empty cup is not needed
            var puzzle4 = "YYbO,bbYO,OYbO,-,-"; // Note: second empty cup is not needed
            var puzzle5 = "bYYb,BOOB,BYOb,OYBb,-,-"; // Note: second empty cup is not needed
            var puzzle6 = "YYrP,YVrP,VPVr,PVrY,-,-";
            var puzzle7 = "GVPP,PODV,DGPG,ODVG,OVOD,-,-";
            var puzzle8 = "YOPG,OGYu,GYPP,GuOu,YOuP,-,-";
            var puzzle9 = "OBYG,BODB,OGOY,DGBD,YDGY,-,-";
            var puzzle10 = "uBBu,OOBr,PBur,OVPP,VrPr,uOVV,-,-";
            var puzzle11 = "gYPV,gPBP,YGGB,gYVB,BVGG,YVgP,-,-";
            var puzzle12 = "YYRV,GOOB,VGRV,YGOO,GYBR,BRBV,-,-";
            var puzzle13 = "ggBr,VrOG,gGRR,GRgO,BVRV,GBBV,rrOO,-,-";
            var puzzle14 = "BBPV,GGBB,OPVC,VCgO,VCgC,PGOP,OGgg,-,-";
            var puzzle15 = "CCVB,VrRG,RBRR,YCGY,YVrr,GrVB,BCGY,-,-";

            var cups = new Cups(puzzle15);

            var solver = new AStarSolver(cups);
            while (solver.Step())
            {
            }

            solver.PrintBestSolution();
        }
    }
}