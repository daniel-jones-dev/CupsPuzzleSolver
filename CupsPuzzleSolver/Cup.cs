using System;

namespace CupsPuzzleSolver
{
    public class Cup
    {
        public const int MaxSize = 4;

        private string _contents = "";

        public Cup()
        {
        }

        public Cup(string content)
        {
            if (content == "-") content = "";
            _contents = content;
        }

        public int Volume => _contents.Length;
        public bool Empty => Volume == 0;
        public bool Full => Volume == MaxSize;

        public char Color(int index)
        {
            return _contents[index];
        }

        public char TopColor()
        {
            if (Volume == 0) throw new Exception("Cup is empty");

            return Color(Volume - 1);
        }

        public int NumTopColors()
        {
            if (Empty) return 0;
            for (var i = 1; i < MaxSize; ++i)
                if (Volume <= i || Color(Volume - i - 1) != TopColor())
                    return i;

            return MaxSize;
        }

        public bool CanPourInto(Cup other)
        {
            if (other.Full) return false;
            if (Empty) return false;
            if (other.Empty) return true;
            return other.TopColor() == TopColor();
        }

        public void PourInto(Cup other)
        {
            if (!CanPourInto(other)) throw new Exception("Cannot pour this cup into other");

            var colorCount = NumTopColors();
            if (other.Volume + colorCount > Cup.MaxSize) colorCount = Cup.MaxSize - other.Volume;
            other._contents += new string(TopColor(), colorCount);
            _contents = _contents.TrimEnd(TopColor());
        }
    }
}