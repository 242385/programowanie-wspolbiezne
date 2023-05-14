using System.ComponentModel;
using System.Numerics;

namespace Dane
{
    public abstract class IBall : IObservable<IBall>
    {
        public abstract Vector2 Coordinates { get; set; }
        public abstract Vector2 VelVector { get; set; }
        public abstract float Mass { get; set; }
        public abstract bool StopTask { get; set; }
        public abstract float Radius { get; set; }

        public abstract bool StartMoving { get; set; }

        public abstract bool IsInACollision { get; set; }

        public static IBall CreateBall(float mass, float radius, Vector2 coords, Vector2 vector)
        {
            return new Ball(mass, radius, coords, vector);
        }

        public abstract IDisposable Subscribe(IObserver<IBall> observerObj);
    }
}
