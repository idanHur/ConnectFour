using Connect4Game;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GameManager
{
    public class Player
    {
        public int playerId { get; private set; }

        [Required(ErrorMessage = "Private Name is required.")]
        [MinLength(2, ErrorMessage = "Private Name should be at least 2 characters long.")]
        public string playerName { get; set; }
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
