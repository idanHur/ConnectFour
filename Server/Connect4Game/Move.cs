using System;
using System.Collections.Generic;
using System.Text;

namespace Connect4Game
{
    public class Move
    {
        public int ColumnNumber { get; private set; }
        public Player Player { get; private set; }
        public int Id { get; private set; }

        public Move(int columnNumber, Player player)
        {
            this.ColumnNumber = columnNumber;
            this.Player = player;
        }

    }
}
