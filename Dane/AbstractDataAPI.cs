using System.Reflection.Metadata.Ecma335;

namespace Dane
{
    public abstract class AbstractDataAPI
    {
        public static AbstractDataAPI CreateNewInstance()
        {
            return new DataAPI();
        }

        public abstract IBall CreateBall();
        public abstract void CreateBoard();
        public abstract int GetBoardW();
        public abstract int GetBoardH();

        private class DataAPI : AbstractDataAPI
        {
            internal double preDeterminedMass = 12;
            internal int preDeterminedRadius = 12;
            internal static int hardCodedBoardW = 600;
            internal static int hardCodedBoardH = 600;
            internal Random RNG = new Random();

            internal IBoard? board { get; set; }


            public override IBall CreateBall()
            {
                IPositioning coords = RandomPos(preDeterminedRadius);
                IPositioning v = RandomVector();
                IBall ball = IBall.CreateBall(preDeterminedMass, preDeterminedRadius, coords, v);
                return ball;
            }

            public override void CreateBoard()
            {
                this.board = IBoard.CreateBoard(hardCodedBoardW, hardCodedBoardH);
            }

            public override int GetBoardH()
            {
                if (this.board == null)
                {
                    return 0;
                }
                else
                {
                    return this.board.boardHeight;
                }
            }

            public override int GetBoardW()
            {
                if (this.board == null)
                {
                    return 0;
                }
                else
                {
                    return this.board.boardHeight;
                }
            }

            internal IPositioning RandomPos(double r)
            {
                double x = RNG.NextDouble() * (GetBoardW() - 2 * r) + r;
                double y = RNG.NextDouble() * (GetBoardH() - 2 * r) + r;
                return IPositioning.CreatePos(x, y);
            }

            internal IPositioning RandomVector()
            {
                // Generate random vector while making sure it's not a zero vector
                bool randomBool;
                double generatedX = (RNG.NextDouble() - 1.0) * 5.0;
                double generatedY = (RNG.NextDouble() - 1.0) * 5.0;

                randomBool = RNG.Next(0, 2) == 1;
                double x = randomBool ? generatedX * (-1.0) : generatedX;
                randomBool = RNG.Next(0, 2) == 1;
                double y = randomBool ? generatedX * (-1.0) : generatedX;

                return IPositioning.CreatePos(x, y);
            }
        }
    }
}