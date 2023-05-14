using System;
using System.Numerics;

namespace Dane
{
    internal class Ball : IBall
    {
        private Vector2 Coords;
        public override float Mass { get; set; }
        public override float Radius { get; set; }
        public override float DeltaTime { get; set; }
        public override bool StopTask { get; set; }
        public override bool StartMoving { get; set; }
        public override bool IsInACollision { get; set; }
        public override Vector2 VelVector { get; set; }

        public Ball(float mass, float radius, Vector2 coords, Vector2 vector, float delta)
        {
            this.Mass = mass;
            this.Coords = coords;
            this.VelVector = vector;
            this.DeltaTime = delta;
            this.StopTask = false;
            this.StartMoving = false;
            this.IsInACollision = false;
            this.Radius = radius;
            Task.Run(Moving);
        }

        public override Vector2 Coordinates
        {
            get { return Coords; }
            set
            {
                Coords = value;
            }
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
                await Task.Delay((int)DeltaTime);
            }
        }

        private void UpdateCoords()
        {
            this.Coords += this.VelVector * this.DeltaTime;
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