using System.ComponentModel;

namespace Dane
{
    public abstract class IBall : IObservable<IBall>
    {
        public abstract IPositioning Coordinates { get; }
        public abstract IPositioning VelVector { get; set; }
        public abstract double Mass { get; set; }
        public abstract bool StopTask { get; set; }
        public abstract double Radius { get; set; }

        public abstract bool StartMoving { get; set; }

        public abstract bool IsInACollision { get; set; }

        public static IBall CreateBall(double mass, double radius, IPositioning coords, IPositioning vector)
        {
            return new Ball(mass, radius, coords, vector);
        }

        public abstract IDisposable Subscribe(IObserver<IBall> observerObj);
    }
}
