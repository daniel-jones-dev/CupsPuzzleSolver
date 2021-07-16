namespace CupsPuzzleSolver
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var puzzle1 = "-,POPO,OPOP";
            var puzzle2 = "YYYB,DDBB,DDBY,-,-";

            var cups = new Cups(puzzle2);

            var solver = new NaiveSolver(cups);
            while (solver.Step())
            {
            }
        }
    }
}