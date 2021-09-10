namespace CupsPuzzleSolver
{
    public struct Move
    {
        public int FromCupIndex;
        public int FromRowIndex;
        public bool Tap;
        public int ToCupIndex;
        public int ToRowIndex;

        public override string ToString()
        {
            if (Tap)
            {
                return $"Tap cup {FromCupIndex} in row {FromRowIndex}";
            }
            else
            {
                return $"Pour cup {FromCupIndex} in row {FromRowIndex} into cup {ToCupIndex} in row {ToRowIndex}";
            }
        }
    }
}