using GameManager.Models;
using System.Collections.Generic;

namespace Server.ViewModels
{
    public class PlayersGroupedByCountryViewModel
    {
        public string Country { get; set; }
        public List<Player> Players { get; set; }
    }

}
