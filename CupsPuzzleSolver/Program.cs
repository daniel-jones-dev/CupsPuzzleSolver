namespace CupsPuzzleSolver
{
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