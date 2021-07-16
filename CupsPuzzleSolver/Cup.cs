using System;

namespace CupsPuzzleSolver
{
    public class Cup
    {
        public const int MaxSize = 4;

        public Cup()
        {
        }

        public Cup(string content)
        {
            if (content == "-") content = "";
            Content = content;
        }

        public int Volume => Content.Length;
        public bool Empty => Volume == 0;
        public bool Full => Volume == MaxSize;

        public string Content { get; private set; } = "";

        public char Color(int index)
        {
            return Content[index];
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
            if (other.Full || Empty || NumTopColors() == 4) return false;
            if (other.Empty) return true;
            return other.TopColor() == TopColor();
        }

        public void PourInto(Cup other)
        {
            if (!CanPourInto(other)) throw new Exception("Cannot pour this cup into other");

            var colorCount = NumTopColors();
            if (other.Volume + colorCount > MaxSize) colorCount = MaxSize - other.Volume;
            other.Content += new string(TopColor(), colorCount);
            Content = Content[..(Volume - colorCount)];
        }
    }
}