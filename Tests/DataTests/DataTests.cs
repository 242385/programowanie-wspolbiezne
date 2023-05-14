using Dane;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace Tests
{
    [TestClass]
    public class DataTests
    {
        [TestMethod]
        public void ApiCreatingBallTest()
        {
            AbstractDataAPI testApi1 = AbstractDataAPI.CreateNewInstance();
            testApi1.CreateBoard();

            IBall ball1 = testApi1.CreateBall();
            Assert.IsNotNull(ball1);
        }

        [TestMethod]
        public void CreatingBoardTest()
        {
            IBoard board1 = IBoard.CreateBoard(500, 500);
            Assert.IsNotNull(board1);
            Assert.AreEqual(board1.boardHeight, 500);
            Assert.AreEqual(board1.boardWidth, 500);
        }
        [TestMethod]
        public void CreatingBallTest()
        {
            float testMass = 10;
            float testRadius = 10;
            int x = 0;
            int y = 0;
            int x2 = 1;
            int y2 = 2;
            Vector2 coords = new Vector2(x, y);
            Vector2 v = new Vector2(x2, y2);
            IBall ball1 = IBall.CreateBall(testMass, testRadius, coords, v);
            Assert.IsNotNull(ball1);
        }

        [TestMethod]
        public void DataAPITests()
        {
            AbstractDataAPI testApi1 = AbstractDataAPI.CreateNewInstance();
            IBall ball1 = testApi1.CreateBall();
            testApi1.CreateBoard();
            Assert.IsNotNull(ball1);
            Assert.AreEqual(testApi1.GetBoardH(), 600);
            Assert.AreEqual(testApi1.GetBoardW(), 600);
        }
    }
}


