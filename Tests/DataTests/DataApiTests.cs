using Dane;

namespace Tests
{
    [TestClass]
    public class DataApiTests
    {
        [TestMethod]
        public void CreateNewInstanceTest()
        {
            DataApi dataApi1 = DataApi.CreateNewInstance();

            Assert.IsNotNull(dataApi1);
        }
        [TestMethod]
        public void GetBallListTest()
        {
            DataApi dataApi1 = DataApi.CreateNewInstance();
            List<Ball> ballList1;
            ballList1 = dataApi1.GetBallList();

            Assert.IsNotNull(ballList1);
        }
    }
}