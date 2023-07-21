using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLogicClient.Models
{
    public class Player
    {
        public int playerId { get; set; }
        public string playerName { get; set; }
        public string phoneNumber { get; set; }
        public string country { get; set; }

        public List<Game> games { get; set; }

        public Player() { 
            games = new List<Game>();
        }
    }
}
