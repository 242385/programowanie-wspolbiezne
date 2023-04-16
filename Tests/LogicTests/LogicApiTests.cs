using Dane;
using Logika;

namespace Tests
{ 
    [TestClass]
    public class LogicApiTests
    {
        [TestMethod]
        public void CreateNewInstanceTest()
        {
            DataApi dataApi1 = DataApi.CreateNewInstance();
            LogicApi logicApi1 = LogicApi.CreateNewInstance(dataApi1);

            Assert.IsNotNull(logicApi1);
        }
        [TestMethod]
        public void GenerateBallsTest()
        {
            DataApi dataApi1 = DataApi.CreateNewInstance();
            LogicApi logicApi = LogicApi.CreateNewInstance(dataApi1);
            int numberOfBalls = 20;

            logicApi.GenerateBalls(numberOfBalls);
            List<Ball> ballList1 = logicApi.GetBallList();
  

            Assert.AreEqual(ballList1.Count, numberOfBalls);
        }

        [TestMethod]
        public void BallsActuallyMoveTest()
        {
            DataApi dataApi1 = DataApi.CreateNewInstance();
            LogicApi logicApi1 = LogicApi.CreateNewInstance(dataApi1);
            int numberOfBalls = 1;

            logicApi1.GenerateBalls(numberOfBalls);
            List<Ball> ballList1 = logicApi1.GetBallList();
            int y1 = ballList1[0].y;
            int x1 = ballList1[0].x;
            logicApi1.CreateThreads();
            Thread.Sleep(150);

            Assert.AreNotEqual(ballList1[0].x, x1);
            Assert.AreNotEqual(ballList1[0].y, y1);
        }
    }
}
