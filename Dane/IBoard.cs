using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dane
{
    public abstract class IBoard
    {
        public abstract int boardWidth { get; set; }

        public abstract int boardHeight { get; set; }

        public static IBoard CreateBoard(int w, int h)
        {
            return new Board(w, h);
        }

        private class Board : IBoard
        {
            public override int boardWidth { get; set; }
            public override int boardHeight { get; set; }

            public Board(int w, int h)
            {
                this.boardWidth = w;
                this.boardHeight = h;
            }
        }
    }
}
