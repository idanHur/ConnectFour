using Connect4Game;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace GameManager
{
    public class Manager
    {
        public string playerName;
        public Game currentGame { get; private set; }
        private List<Game> _gamesRecord = new List<Game>();

        public ReadOnlyCollection<Game> gamesRecord => _gamesRecord.AsReadOnly();

        public Manager()
        {
            _gamesRecord = new List<Game>();
            currentGame = null;
        }

    }
}
