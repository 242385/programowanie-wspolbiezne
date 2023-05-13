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


        public Ball(double mass, double radius)
        {
            this.Mass = mass;
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