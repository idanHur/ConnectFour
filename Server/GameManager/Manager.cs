using Connect4Game;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace GameManager
{
    public class Manager
    {
        private List<Player> _playersRecord = new List<Player>();

        public ReadOnlyCollection<Player> playersRecord => _playersRecord.AsReadOnly();

        public Manager()
        {
            _playersRecord = new List<Player>();
        }

        public void AddPlayer(Player player)
        {
            _playersRecord.Add(player);
        }
    }
}
