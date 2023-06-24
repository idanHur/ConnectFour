﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GameLogic.Models
{
    public class Move
    {
        public int columnNumber { get; private set; }
        public Player Player { get; private set; }
        public int Id { get; private set; }

        public Move(int columnNumber, Player player, int id)
        {
            this.columnNumber = columnNumber;
            this.Player = player;
            Id = id;
        }

    }
}
