using ConcurrentProgramming0;

namespace AreaCalculatorTests
{
    [TestClass]
    public class AreaTests
    {
        [TestMethod]
        public void AreaTests1()
        {
            AreaCalculator ac1 = new AreaCalculator();
            //Triangle test
            Assert.AreEqual(expected: 6, ac1.TriangleArea(4, 3));
            Assert.AreEqual(expected: 0, ac1.TriangleArea(-3, 0));

            //Rectangle test
            Assert.AreEqual(expected: 12, ac1.RectangleArea(4, 3));
            Assert.AreEqual(expected: 0, ac1.RectangleArea(0, -2));

            //Square test
            Assert.AreEqual(expected: 9, ac1.SquareArea(3));
            Assert.AreEqual(expected: 0, ac1.SquareArea(-4));
            Assert.AreEqual(expected: 0, ac1.SquareArea(0));

            //Trapeze test
            Assert.AreEqual(expected: 7, ac1.TrapezeArea(4, 3, 2));
            Assert.AreEqual(expected: 0, ac1.TrapezeArea(0, -2, 3));
        }
    }
}