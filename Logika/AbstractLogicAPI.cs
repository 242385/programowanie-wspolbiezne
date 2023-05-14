using Dane;

namespace Logika
{
    public abstract class AbstractLogicAPI : IObserver<IBall>, IObservable<int>
    {
        public abstract void CreateBoard();
        public abstract void CreateBalls(int num);
        public abstract void ClearBoard();
        public abstract void StartBalls();
        public abstract List<List<double>> GetBallsCoordsAndRadius();

        public abstract void OnCompleted();

        public abstract void OnError(Exception error);

        public abstract void OnNext(IBall value);

        public abstract IDisposable Subscribe(IObserver<int> observerObj);

        public static AbstractLogicAPI CreateNewInstance(AbstractDataAPI? DataAPI = default)
        {
            return new LogicAPI(DataAPI == null ? AbstractDataAPI.CreateNewInstance() : DataAPI);
        }

        internal sealed class LogicAPI : AbstractLogicAPI
        {
            internal AbstractDataAPI dataApi;
            internal List<IDisposable>? ballObservers;
            internal IObserver<int>? observedObject;
            internal List<IBall> balls { get; set; }
            internal object locked = new object();  //sekcja krytyczna nizej

            public LogicAPI(AbstractDataAPI dataAPI)
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
                    double vX = balls[i].VelVector.X;
                    double vY = balls[i].VelVector.Y;

                    List<double> list = new List<double>()
                    {
                        x, y, r, vX, vY
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
                    ball.Coordinates.X = dataApi.GetBoardW() - ball.Radius;
                }
                else if (ball.Coordinates.X < ball.Radius)
                {
                    ball.Coordinates.X = ball.Radius;
                }
                if (ball.Coordinates.Y > dataApi.GetBoardH() - ball.Radius)
                {
                    ball.Coordinates.Y = dataApi.GetBoardH() - ball.Radius;
                }
                else if (ball.Coordinates.Y < ball.Radius)
                {
                    ball.Coordinates.Y = ball.Radius;
                }
            }

            private void WallCollision(IBall ball)
            {
                if (ball.Coordinates.X - ball.Radius <= 0 ||
                    (ball.Coordinates.X + ball.Radius) > dataApi.GetBoardW())
                {
                    ball.VelVector.X = -ball.VelVector.X;
                    //kolizja ze sciana: odwracamy wektor predkosci
                }
                if (ball.Coordinates.Y - ball.Radius <= 0 ||
                   (ball.Coordinates.Y + ball.Radius) > dataApi.GetBoardW())
                {
                    ball.VelVector.Y = -ball.VelVector.Y;
                    //kolizja ze sciana: odwracamy wektor predkosci
                }
            }

            private void BallsCollision(IBall ball)
            {
                List<IBall> collidingBalls = new List<IBall>();
                double distance;

                for (int i = 0; i < balls.Count; i++)
                {
                    distance = ball.Coordinates.Distance(balls[i].Coordinates);
                    IPositioning nextPos = ball.Coordinates.Add(ball.VelVector);
                    IPositioning nextPos2 = balls[i].Coordinates.Add(balls[i].VelVector);

                    if (balls[i] != ball && distance <= 2 * ball.Radius &&
                        distance - nextPos.Distance(nextPos2) > 0)
                    {
                        collidingBalls.Add(balls[i]);
                    }
                }

                foreach (IBall collidingBall in collidingBalls)
                {
                    IPositioning ballDistance = ball.Coordinates.Subtract(collidingBall.Coordinates);
                    IPositioning Vdifference = ball.VelVector.Subtract(collidingBall.VelVector);

                    IPositioning secondPart = ballDistance.Multiply(Vdifference.Dot(ballDistance)
                        / Math.Pow(ballDistance.VectorLength(), 2));
                    IPositioning newV = ball.VelVector.Subtract(secondPart.Multiply(2f * collidingBall.Mass /
                        (ball.Mass + collidingBall.Mass)));

                    ballDistance = collidingBall.Coordinates.Subtract(ball.Coordinates);
                    Vdifference = collidingBall.VelVector.Subtract(ball.VelVector);

                    secondPart = ballDistance.Multiply(Vdifference.Dot(ballDistance)
                        / Math.Pow(ballDistance.VectorLength(), 2));

                    IPositioning newVcollidingBall = collidingBall.VelVector.Subtract(secondPart.Multiply(2f * ball.Mass
                        / (ball.Mass + collidingBall.Mass)));

                    ball.VelVector = newV;
                    collidingBall.VelVector = newVcollidingBall;
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
}
