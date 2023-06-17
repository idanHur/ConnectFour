using System;
using System.Collections.Generic;
using System.Text;

namespace Connect4Game
{
    public class Move
    {
        public int columnNumber { get; private set; }
        public bool humenMove { get; private set; }

        public Move(int columnNumber, bool humenMove)
        {
            this.columnNumber = columnNumber;
            this.humenMove = humenMove;
        }

    }
}
