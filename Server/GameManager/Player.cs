using Connect4Game;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GameManager
{
    public class Player
    {
        [Required(ErrorMessage = "Player ID is required.")]
        [Remote(action: "IsPlayerIdAvailable", controller: "Player", ErrorMessage = "Player ID is already taken.")]
        public int playerId { get; private set; }

        [Required(ErrorMessage = "Private Name is required.")]
        [MinLength(2, ErrorMessage = "Private Name should be at least 2 characters long.")]
        public string playerName { get; set; }
        [RegularExpression(@"^\d{9,10}$", ErrorMessage = "Phone Number should be between 9 and 10 digits.")]
        public string phoneNumber { get; set; }
        [Required(ErrorMessage = "Country is required.")]
        public string country { get; set; }


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
