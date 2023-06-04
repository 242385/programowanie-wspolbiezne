using System;
using System.Diagnostics;
using System.Numerics;
using System.Reflection;
using System.Text.Json;
using Newtonsoft.Json;

namespace Dane
{
    internal class Ball : IBall
    {
        public override int BallID { get; }    
        private Vector2 Coords;
        [JsonIgnore]
        public override float Mass { get; set; }
        [JsonIgnore]
        public override float Radius { get; set; }
        [JsonIgnore]
        public override float DeltaTime { get; set; }
        [JsonIgnore]
        public override bool StopTask { get; set; }
        [JsonIgnore]
        public override bool StartMoving { get; set; }
        [JsonIgnore]
        public override bool IsInACollision { get; set; }
        [JsonIgnore]
        public override Vector2 VelVector { get; set; }
        [JsonIgnore]
        private Stopwatch stopwatch;
        [JsonIgnore]
        private ILogger logger;

        public Ball(int ballID, float mass, float radius, Vector2 coords, Vector2 vector, float delta, ILogger? logger)
        {
            this.BallID = ballID;
            this.Mass = mass;
            this.Coords = coords;
            this.VelVector = vector;
            this.DeltaTime = delta;
            this.StopTask = false;
            this.StartMoving = false;
            this.IsInACollision = false;
            this.Radius = radius;
            this.logger = logger;
            stopwatch = new Stopwatch();
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
            int delay = 0;
            while (!this.StopTask)
            {
                stopwatch.Restart();
                stopwatch.Start();

                if (this.StartMoving)
                {
                    this.UpdateCoords();
                    if (this.ObserverObject != null)
                    {
                        this.ObserverObject.OnNext(this);
                    }
                    this.IsInACollision = false;
                }
                if (this.logger != null)
                {
                    logger.AddBallToQueue(this);
                }

                stopwatch.Stop();
                if (this.DeltaTime - stopwatch.ElapsedMilliseconds < 0)
                {
                    delay = 10;
                }
                else
                {
                    delay=(int)this.DeltaTime-(int)stopwatch.ElapsedMilliseconds;
                }
                await Task.Delay(delay);
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

        public override void Dispose()
        {
            this.StopTask = true;
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