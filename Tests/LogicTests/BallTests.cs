
using Logika;

namespace Tests
{
    [TestClass]
    public class BallTests
    {
        [TestMethod]
        public void ConstructorTest()
        {
            int x = 123;
            int y = 456;
            Ball ball1 = new Ball(x, y);
            Assert.AreEqual(ball1.x, x);
            Assert.AreEqual(ball1.y, y);
        }
        [TestMethod]
        public void SetTest()
        {
            int x = 50;
            int y = 100;
            Ball ball2 = new Ball(0, 0);
            ball2.x = x;
            ball2.y = y;
            Assert.AreEqual(ball2.x, x);
            Assert.AreEqual(ball2.y, y);
        }
    }
}
