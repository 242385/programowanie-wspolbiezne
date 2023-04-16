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
            List<Ball> listaKulek = logicApi.GetBallList();

            Assert.AreEqual(listaKulek.Count, numberOfBalls);
        }
    }
}
