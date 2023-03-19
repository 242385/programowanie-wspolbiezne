using System.Reflection.Metadata.Ecma335;

namespace ConcurrentProgramming0
{
    public class AreaCalculator
    {
        public int TriangleArea(int a, int h)
        {
            if (a > 0 && h > 0)
            {
                return (a * h) / 2;
            }
            else
            {
                return 0;
            }
        }

        public int RectangleArea(int a, int b)
        {
            if (a > 0 && b > 0)
            {
                return a * b;
            }
            else
            { return 0; }
        }

        public int SquareArea(int a)
        {
            if (a > 0)
            {
                return a * a;
            }
            else { return 0; }
        }

        public int TrapezeArea(int a, int b, int h)
        {
            if (a > 0 && b > 0 && h > 0)
            {
                return ((a + b) * h) / 2;
            }
            else
            {
                return 0;
            }

        }
    }
}