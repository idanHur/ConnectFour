using Connect4Game;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

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

        public bool IsIdTaken(int id)
        {
            return _playersRecord.Any(player => player.playerId == id);
        }
        public void AddPlayer(Player player)
        {
            _playersRecord.Add(player);
        }
    }
}
