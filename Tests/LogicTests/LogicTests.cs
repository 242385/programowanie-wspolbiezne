using Dane;
using Logika;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using System.Numerics;

namespace Tests
{
    [TestClass]
    public class LogicTests
    {
        internal class TestDataAPI : AbstractDataAPI
        {
            internal float preDeterminedMass = 12;
            internal float preDeterminedRadius = 12;
            internal static int hardCodedBoardW = 1000;
            internal static int hardCodedBoardH = 1000;
            internal Random RNG = new Random();
            internal bool CreateBoardFunctionUsed { get; set; }
            internal int boardW { get; set; }
            internal int boardH { get; set; }

            public override TestBall CreateBall()
            {
                double x = RNG.NextDouble() * (GetBoardW() - 2 * preDeterminedRadius) + preDeterminedRadius;
                double y = RNG.NextDouble() * (GetBoardH() - 2 * preDeterminedRadius) + preDeterminedRadius;
                Vector2 test = new Vector2((float)x, (float)y);
                x = RNG.NextDouble() * 10 - 4;
                y = RNG.NextDouble() * 10 - 4;
                Vector2 testV = new Vector2((float)x, (float)y);
                return new TestBall(preDeterminedMass, preDeterminedRadius, test, testV, 10);
            }

            public override void CreateBoard()
            {
                this.boardW = hardCodedBoardW;
                this.boardH = hardCodedBoardH;
                this.CreateBoardFunctionUsed = true;
            }

            public override int GetBoardH()
            {
                return this.boardH;
            }

            public override int GetBoardW()
            {
                return this.boardW;
            }
        }

        internal class TestBall : IBall
        {
            private Vector2 PrivCoords;
            public override Vector2 Coordinates { get; set; }
            public override Vector2 VelVector { get; set; }
            public override float DeltaTime { get; set; }
            public override float Mass { get; set; }
            public override bool StopTask { get; set; }
            public override float Radius { get; set; }
            public override bool StartMoving { get; set; }
            public override bool IsInACollision { get; set; }

            internal IObserver<IBall>? ObserverObject;

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

            public TestBall(float mass, float radius, Vector2 coords, Vector2 vector, float delta)
            {
                this.Mass = mass;
                this.PrivCoords = coords;
                this.VelVector = vector;
                this.DeltaTime = delta;
                this.StopTask = false;
                this.StartMoving = false;
                this.IsInACollision = false;
                this.Radius = radius;
                Task.Run(Moving);
            }
            private void Moving()
            {
                while (!this.StopTask)
                {
                    if (this.ObserverObject != null)
                    {
                        this.ObserverObject.OnNext(this);
                    }
                    this.UpdateCoords();
                }
            }
            private void UpdateCoords()
            {
                this.PrivCoords += this.VelVector * this.DeltaTime;
            }
        }

        [TestMethod]
        public void LogicAPICreateNewInstanceTest()
        {
            TestDataAPI testDataApi = new TestDataAPI();
            AbstractLogicAPI testLogicApi = AbstractLogicAPI.CreateNewInstance(testDataApi);
            Assert.IsNotNull(testLogicApi);
        }
        [TestMethod]
        public void LogicAPICreateBallsTest()
        {
            TestDataAPI testDataApi = new TestDataAPI();
            AbstractLogicAPI testLogicApi = AbstractLogicAPI.CreateNewInstance(testDataApi);
            testLogicApi.CreateBoard();
            testLogicApi.CreateBalls(5);
            List<List<double>> ballCoords = testLogicApi.GetBallsCoordsAndRadius();
            Assert.AreEqual(ballCoords.Count, 5);
        }

        [TestMethod]
        public void LogicAPICreateBoardTest()
        {
            TestDataAPI testDataApi = new TestDataAPI();
            AbstractLogicAPI testLogicApi = AbstractLogicAPI.CreateNewInstance(testDataApi);
            testLogicApi.CreateBoard();
            Assert.AreEqual(true, testDataApi.CreateBoardFunctionUsed);
            Assert.AreEqual(1000, testDataApi.GetBoardH());
            Assert.AreEqual(1000, testDataApi.GetBoardW());
        }
        [TestMethod]
        public void LogicAPIClearBoardTest()
        {
            TestDataAPI testDataApi = new TestDataAPI();
            AbstractLogicAPI testLogicApi = AbstractLogicAPI.CreateNewInstance(testDataApi);
            testLogicApi.CreateBoard();
            testLogicApi.CreateBalls(5);
            List<List<double>> ballCoords = testLogicApi.GetBallsCoordsAndRadius();
            Assert.AreEqual(ballCoords.Count, 5);
            testLogicApi.ClearBoard();
            ballCoords = testLogicApi.GetBallsCoordsAndRadius();
            Assert.AreEqual(ballCoords.Count, 0);
        }
    }
}

