using GameManager.Models;
using System.Collections.Generic;

namespace Server.ViewModels
{
    public class GroupedByGameCountViewModel
    {
        public int GameCount { get; set; }
        public List<Player> Players { get; set; }
    }
}
