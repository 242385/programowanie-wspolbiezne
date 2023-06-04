using System.Numerics;
using System.Reflection.Metadata.Ecma335;

namespace Dane
{
    public abstract class AbstractDataAPI
    {
        public static AbstractDataAPI CreateNewInstance()
        {
            return new DataAPI();
        }

        public abstract IBall CreateBall(int id);
        public abstract void CreateBoard();
        public abstract int GetBoardW();
        public abstract int GetBoardH();
        public abstract void CreateLogger();

        private class DataAPI : AbstractDataAPI
        {
            internal float preDeterminedMass = 12;
            internal int preDeterminedRadius = 12;
            internal static int hardCodedBoardW = 600;
            internal static int hardCodedBoardH = 600;
            internal Random RNG = new Random();
            internal ILogger? logger = null;

            internal IBoard? board { get; set; }


            public override IBall CreateBall(int id)
            {
                Vector2 coords = RandomPos(preDeterminedRadius);
                Vector2 v = RandomDirection();
                IBall ball = IBall.CreateBall(id, preDeterminedMass, preDeterminedRadius, coords, v, RandomTickRate(), this.logger);
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

            internal Vector2 RandomPos(float r)
            {
                double x = RNG.NextDouble() * (GetBoardW() - 2 * r) + r;
                double y = RNG.NextDouble() * (GetBoardH() - 2 * r) + r;
                return new Vector2((float)x, (float)y);
            }

            internal Vector2 RandomDirection()
            {
                // Generate random vector while making sure it's not a zero vector
                bool randomBool;
                double generatedX = (RNG.NextDouble() - 1.0) * 5.0;
                double generatedY = (RNG.NextDouble() - 1.0) * 5.0;

                randomBool = RNG.Next(0, 2) == 1;
                double x = randomBool ? generatedX * (-1.0) : generatedX;
                randomBool = RNG.Next(0, 2) == 1;
                double y = randomBool ? generatedY * (-1.0) : generatedY;

                Vector2 v = new Vector2((float)x, (float)y);
                v = Vector2.Normalize(v);

                return v;
            }

            internal float RandomTickRate()
            {
                return (float)RNG.NextDouble() * 5 + 1;
            }

            public override void CreateLogger()
            {
                this.logger = ILogger.CreateLogger();
            }
        }
    }
}