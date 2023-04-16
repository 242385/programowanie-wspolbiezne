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

    }
}
