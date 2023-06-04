using System.ComponentModel;
using System.Numerics;
using System.Runtime.Serialization;

namespace Dane
{
    public abstract class IBall : IObservable<IBall>, ISerializable
    {
        public abstract int BallID { get; }
        public abstract Vector2 Coordinates { get; set; }
        public abstract Vector2 VelVector { get; set;}
        public abstract float DeltaTime { get; set; }
        public abstract float Mass { get; set; }
        public abstract bool StopTask { get; set; }
        public abstract float Radius { get; set; }

        public abstract bool StartMoving { get; set; }

        public abstract bool IsInACollision { get; set; }

        public static IBall CreateBall(int ballID, float mass, float radius, Vector2 coords, Vector2 vector, float delta)
        {
            return new Ball(ballID, mass, radius, coords, vector, delta);
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            throw new NotImplementedException();
        }

        public abstract IDisposable Subscribe(IObserver<IBall> observerObj);
    }
}
