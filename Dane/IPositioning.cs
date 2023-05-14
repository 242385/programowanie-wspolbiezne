using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dane
{
    public abstract class IPositioning
    {
        public abstract double X { get; set; }
        public abstract double Y { get; set; }

        public static IPositioning CreatePos(double x, double y)
        {
            return new Positioning(x, y);
        }

        public abstract IPositioning Add(IPositioning pos);
        public abstract IPositioning Multiply(double temp);
        public abstract IPositioning Subtract(IPositioning pos);
        public abstract double VectorLength();

        public abstract double Distance(IPositioning pos);

        public abstract double Dot(IPositioning pos);

        private class Positioning : IPositioning
        {
            public override double X { get; set; }
            public override double Y { get; set; }

            public Positioning(double x, double y)
            {
                this.X = x;
                this.Y = y;
            }

            // FUNKCJE DO DZIAŁAŃ NA KOORDYNATACH:

            public override IPositioning Add(IPositioning pos)
            {
                double x = this.X + pos.X;
                double y = this.Y + pos.Y;
                return CreatePos(x, y);
            }

            public override IPositioning Multiply(double temp)
            {
                double x = this.X * temp;
                double y = this.Y * temp;
                return CreatePos(x, y);
            }

            public override IPositioning Subtract(IPositioning pos)
            {
                double x = this.X - pos.X;
                double y = this.Y - pos.Y;
                return CreatePos(x, y);
            }

            public override double VectorLength()
            {
                double vectorLength = Math.Sqrt(Math.Pow(this.X, 2) + Math.Pow(this.Y, 2));
                return vectorLength;
            }

            public override double Distance(IPositioning pos)
            {
                double distance = Math.Sqrt(Math.Pow(this.X - pos.X, 2) +
                    Math.Pow(this.Y - pos.Y, 2));
                return distance;
            }

            public override double Dot(IPositioning pos)
            {
                return this.X * pos.X + this.Y * pos.Y;
            }
        }
    }
}
