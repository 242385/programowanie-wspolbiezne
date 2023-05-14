using System;

namespace Dane
{
    internal class Ball : IBall
    {
        public override double Mass { get; set; }
        public override double Radius { get; set; }
        public override bool StopTask { get; set; }
        public override bool StartMoving { get; set; }
        public override bool IsInACollision { get; set; }
        public override IPositioning Coordinates { get; }
        public override IPositioning VelVector { get; set; }

        public Ball(double mass, double radius, IPositioning coords, IPositioning vector)
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
                await Task.Delay(1);
            }
        }

        private void UpdateCoords()
        {
            this.Coordinates.X += this.VelVector.X;
            this.Coordinates.Y += this.VelVector.Y;
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