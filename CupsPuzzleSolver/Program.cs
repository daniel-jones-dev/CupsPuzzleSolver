namespace CupsPuzzleSolver
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            string puzzle1 = "-,POPO,OPOP";
            string puzzle2 = "YYYB,DDBB,DDBY,-,-";
            
            var cups = new Cups(puzzle2);

            var solver = new Solver(cups);
            while (solver.Step())
            {
            }
        }
    }
}