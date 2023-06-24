using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLogic.Models
{
    public class Player
    {
        public int id { get; set; }
        public List<Game> games { get; set; }

        public Player() { 
            games = new List<Game>();
        }
    }
}
