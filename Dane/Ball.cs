using System;
using System.Diagnostics;
using System.Numerics;

namespace Dane
{
    internal class Ball : IBall
    {
        public override float Mass { get; set; }
        public override float Radius { get; set; }
        public override bool StopTask { get; set; }
        public override bool StartMoving { get; set; }
        public override bool IsInACollision { get; set; }
        public override Vector2 Coordinates { get; set; }
        public override Vector2 VelVector { get; set; }

        public Ball(float mass, float radius, Vector2 coords, Vector2 vector)
        {
            this.Mass = mass;
            this.Coordinates = coords;
            this.VelVector = vector;
            this.StopTask = false;
            this.StartMoving = false;
            this.IsInACollision = false;
            this.Radius = radius;
            Task.Run(Moving);
        }

        internal IObserver<IBall>? ObserverObject;

        private async void Moving()
        {
            while (!this.StopTask)
            {
                if (this.StartMoving)
                {
                    this.UpdateCoords();
                }
                if (this.ObserverObject != null)
                {
                    this.ObserverObject.OnNext(this);
                }
                this.IsInACollision = false;         
                await Task.Delay(10);
            }
        }

        private void UpdateCoords()
        {
            this.Coordinates += this.VelVector;
        }

        public override IDisposable Subscribe(IObserver<IBall> observerObj)
        {
            this.ObserverObject = observerObj;
            return new ObserverManager(observerObj);
        }

        private class ObserverManager : IDisposable
        {
            IObserver<IBall>? obs;

            public ObserverManager(IObserver<IBall> observerObj)
            {
                this.obs = observerObj;
            }

            public void Dispose()
            {
                this.obs = null;
            }
        }

    }
}