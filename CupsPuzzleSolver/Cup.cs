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
            if (content.StartsWith("<"))
            {
                HasTap = true;
                content = content.Substring(1);
            }
            else
            {
                HasTap = false;
            }

            if (content.EndsWith(")"))
            {
                HasLid = true;
                content = content.Substring(0, content.Length - 1);
            }
            else
            {
                HasLid = false;
            }

            if (content == "-") content = "";
            Content = content;
        }
        
        public override string ToString()
        {
            return (HasTap ? "<" : "") + Content + (HasLid ? ")" : "");
        }

        public int Volume => Content.Length;
        public bool Empty => Volume == 0;
        public bool Full => Volume == MaxSize;

        public string Content { get; private set; } = "";

        public bool HasTap { get; set; }

        public bool HasLid { get; set; }

        public char Color(int index)
        {
            return Content[index];
        }

        public char BottomColor()
        {
            if (Volume == 0) throw new Exception("Cup is empty");
            return Color(0);
        }

        public char TopColor()
        {
            if (Volume == 0) throw new Exception("Cup is empty");
            return Color(Volume - 1);
        }

        public int NumBottomColors()
        {
            if (Empty) return 0;
            for (var i = 1; i < MaxSize; ++i)
                if (Volume <= i || Color(i) != BottomColor())
                    return i;

            return MaxSize;
        }

        public int NumTopColors()
        {
            if (Empty) return 0;
            for (var i = 1; i < MaxSize; ++i)
                if (Volume <= i || Color(Volume - i - 1) != TopColor())
                    return i;

            return MaxSize;
        }

        public string DistinctColors()
        {
            if (Empty) return "";
            string result = new(Color(0), 1);
            for (var i = 1; i < Volume; ++i)
                if (Color(i) != result[^1])
                    result += Color(i);

            return result;
        }

        public int MovesToEmpty()
        {
            if (Empty) return 0;
            var moves = 1;
            var lastSeenColor = Color(0);
            for (var i = 1; i < MaxSize; ++i)
                if (Color(i) != lastSeenColor)
                {
                    lastSeenColor = Color(i);
                    ++moves;
                }

            return moves;
        }

        public bool CanPourInto(Cup other, bool tap = false)
        {
            if (other.HasLid || other.Full || Empty || NumTopColors() == 4) return false;
            if (other.Empty) return true;
            if (tap)
                return other.TopColor() == BottomColor();
            else
                return other.TopColor() == TopColor();
        }

        public void PourInto(Cup other, bool tap = false)
        {
            if (!CanPourInto(other, tap: tap)) throw new Exception("Cannot pour this cup into other");

            var colorCount = tap ? NumBottomColors() : NumTopColors();
            if (other.Volume + colorCount > MaxSize) colorCount = MaxSize - other.Volume;
            var moveColor = tap ? BottomColor() : TopColor();
            other.Content += new string(moveColor, colorCount);
            if (tap)
            {
                Content = Content[colorCount..];
            }
            else
            {
                Content = Content[..(Volume - colorCount)];
            }
        }
    }
}