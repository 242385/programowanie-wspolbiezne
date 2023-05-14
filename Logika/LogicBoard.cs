using Dane;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace Logika
{
    internal class LogicBoard : AbstractLogicAPI
    {
        internal AbstractDataAPI dataApi;
        internal List<IDisposable>? ballObservers;
        internal IObserver<int>? observedObject;
        internal List<IBall> balls { get; set; }
        internal object locked = new object();  //sekcja krytyczna nizej

        public LogicBoard(AbstractDataAPI dataAPI)
        {
            this.dataApi = dataAPI;
            balls = new List<IBall>();
            ballObservers = new List<IDisposable>();
        }

        public override void CreateBoard()
        {
            dataApi.CreateBoard();
        }

        public override void CreateBalls(int num)
        {
            for (int i = 0; i < num; i++)
            {
                IBall ball = dataApi.CreateBall();
                balls.Add(ball);
            }
            foreach (IBall ball in balls)
            {
                IDisposable obs = ball.Subscribe(this);
                ballObservers?.Add(obs);
            }
        }

        public override void ClearBoard()
        {
            foreach (IBall ball in balls)
            {
                ball.StopTask = true;
            }
            balls.Clear();
        }

        public override List<List<double>> GetBallsCoordsAndRadius()
        {
            List<List<double>> coordsList = new List<List<double>>();

            for (int i = 0; i < balls.Count; i++)
            {
                double x = balls[i].Coordinates.X;
                double y = balls[i].Coordinates.Y;
                double r = balls[i].Radius;
                double vX = balls[i].DirVector.X;
                double vY = balls[i].DirVector.Y;
                double delta = balls[i].DeltaTime;

                List<double> list = new List<double>()
                    {
                        x, y, r, vX, vY, delta
                    };
                coordsList.Add(list);
            }
            return coordsList;
        }

        public override void StartBalls()
        {
            foreach (IBall ball in balls)
            {
                ball.StartMoving = true;
            }
        }

        private void OutOfBounds(IBall ball)
        {
            if (ball.Coordinates.X > dataApi.GetBoardW() - ball.Radius)
            {
                ball.Coordinates = new Vector2(dataApi.GetBoardW() - ball.Radius, ball.Coordinates.Y);
            }
            else if (ball.Coordinates.X < ball.Radius)
            {
                ball.Coordinates = new Vector2(ball.Radius, ball.Coordinates.Y);
            }
            if (ball.Coordinates.Y > dataApi.GetBoardH() - ball.Radius)
            {
                ball.Coordinates = new Vector2(ball.Coordinates.X, dataApi.GetBoardH() - ball.Radius);
            }
            else if (ball.Coordinates.Y < ball.Radius)
            {
                ball.Coordinates = new Vector2(ball.Coordinates.X, ball.Radius);
            }
        }

        private void WallCollision(IBall ball)
        {
            if (ball.Coordinates.X - ball.Radius <= 0 ||
                (ball.Coordinates.X + ball.Radius) > dataApi.GetBoardW())
            {
                ball.DirVector = new Vector2(-ball.DirVector.X, ball.DirVector.Y);
                //kolizja ze sciana: odwracamy wektor kierunku
            }
            if (ball.Coordinates.Y - ball.Radius <= 0 ||
               (ball.Coordinates.Y + ball.Radius) > dataApi.GetBoardW())
            {
                ball.DirVector = new Vector2(ball.DirVector.X, -ball.DirVector.Y);
                //kolizja ze sciana: odwracamy wektor kierunku
            }
        }

        private void BallsCollision(IBall ball)
        {
            List<IBall> collidingBalls = new List<IBall>();
            double distance;

            for (int i = 0; i < balls.Count; i++)
            {
                distance = Vector2.Distance(ball.Coordinates, balls[i].Coordinates);
                Vector2 nextPos = ball.Coordinates + ball.DirVector * ball.DeltaTime;
                Vector2 nextPos2 = balls[i].Coordinates + balls[i].DirVector * balls[i].DeltaTime;

                if (balls[i] != ball && distance <= 2 * ball.Radius &&
                    distance - Vector2.Distance(nextPos, nextPos2) > 0)
                {
                    collidingBalls.Add(balls[i]);
                }
            }

            foreach (IBall collidingBall in collidingBalls)
            {
                Vector2 ballDistance = ball.Coordinates - collidingBall.Coordinates;
                Vector2 Vdifference = ball.DirVector - collidingBall.DirVector;

                Vector2 secondPart = ballDistance * Vector2.Dot(Vdifference, ballDistance) / ballDistance.LengthSquared();
                Vector2 newV = ball.DirVector - secondPart * (2f * collidingBall.Mass / (ball.Mass + collidingBall.Mass));

                ballDistance = collidingBall.Coordinates - ball.Coordinates;
                Vdifference = collidingBall.DirVector - ball.DirVector;

                secondPart = ballDistance * Vector2.Dot(Vdifference, ballDistance) / ballDistance.LengthSquared();

                Vector2 newVcollidingBall = collidingBall.DirVector - secondPart * (2f * ball.Mass / (ball.Mass + collidingBall.Mass));

                ball.DirVector = Vector2.Normalize(newV);
                collidingBall.DirVector = Vector2.Normalize(newVcollidingBall);

                float ballInitialDeltaTime = ball.DeltaTime;
                float collidingBallInitialDeltaTime = collidingBall.DeltaTime;

                ball.DeltaTime = ((ball.Mass - collidingBall.Mass) / (ball.Mass + collidingBall.Mass)) * ballInitialDeltaTime + ((2 * collidingBall.Mass) / (ball.Mass + collidingBall.Mass)) * collidingBallInitialDeltaTime;
                collidingBall.DeltaTime = ((collidingBall.Mass - ball.Mass) / (ball.Mass + collidingBall.Mass)) * collidingBallInitialDeltaTime + ((2 * ball.Mass) / (ball.Mass + collidingBall.Mass)) * ballInitialDeltaTime;


                ball.IsInACollision = true;
                collidingBall.IsInACollision = true;
            }

        }
        public override void OnCompleted()
        {
            if (balls != null)
            {
                foreach (IDisposable obj in balls)
                {
                    obj.Dispose();
                }
            }
        }

        public override void OnError(Exception error)
        {
            throw new NotImplementedException();
        }

        public override void OnNext(IBall ball)
        {
            int i = balls.IndexOf(ball);
            lock (locked)
            {
                if (!ball.IsInACollision)
                {
                    WallCollision(balls[i]);
                    BallsCollision(balls[i]);
                }
                this.OutOfBounds(ball);
            }
            if (this.observedObject != null)
            {
                this.observedObject.OnNext(i);
            }
        }

        public override IDisposable Subscribe(IObserver<int> observerObj)
        {
            this.observedObject = observerObj;
            return new ObserverManager(observerObj);
        }

        private class ObserverManager : IDisposable
        {
            IObserver<int>? obs;

            public ObserverManager(IObserver<int> observer)
            {
                this.obs = observer;
            }

            public void Dispose()
            {
                this.obs = null;
            }
        }
    }
}

