using Connect4Game;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace GameManager
{
    public class Player
    {
        public int playerId { get; private set; }
        public string playerName;
        public Game currentGame { get; private set; }
        private List<Game> _gamesRecord = new List<Game>();

        public ReadOnlyCollection<Game> gamesRecord => _gamesRecord.AsReadOnly();

        public Player(string playerName, int playerId)
        {
            this.playerName = playerName;
            this.playerId = playerId;
            _gamesRecord = new List<Game>();
            currentGame = null;
        }
    }
}
