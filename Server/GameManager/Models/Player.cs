using Connect4Game;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace GameManager.Models
{
    public class Player
    {
        [Required(ErrorMessage = "Player ID is required.")]
        [Range(1, 1000, ErrorMessage = "Player ID must be between 1 and 1000.")]
        public int playerId { get; set; }

        [Required(ErrorMessage = "Private Name is required.")]
        [MinLength(2, ErrorMessage = "Private Name should be at least 2 characters long.")]
        public string playerName { get; set; }
        [Required(ErrorMessage = "Phone Number is required.")]
        [RegularExpression(@"^\d{9,10}$", ErrorMessage = "Phone Number should be between 9 and 10 digits.")]
        public string phoneNumber { get; set; }
        [Required(ErrorMessage = "Country is required.")]
        public string country { get; set; }
        [Required(ErrorMessage = "Password is required.")]
        [MinLength(4, ErrorMessage = "Password should be at least 4 characters long.")]
        public string password { get; set; }

        // EF Core will automatically load the related Game entities when accessing this property.
        public ICollection<Game> games { get; set; }


        // Parameterless constructor
        public Player()
        {
            games = new List<Game>(); // Initialize the collection
        }
        public Player(string playerName, int playerId)
        {
            this.playerName = playerName;
            this.playerId = playerId;
            games = new List<Game>(); // Initialize the collection
        }
        public Game NewGame(int rows, int columns)
        {
            Game newGame = new Game(rows, columns, playerId);
            games.Add(newGame);
            return newGame;
        }
        public Game GetLastGame()
        {
            var gamesList = games.ToList();
            var lastGame = gamesList[gamesList.Count - 1];
            return lastGame;
        }
        public void EndLastGame(int gameId)
        {
            var gamesList = games.ToList();
            var lastGame = gamesList[gamesList.Count - 1];
            if (lastGame == null) throw new InvalidOperationException($"There are no played games");
            if (lastGame.gameId != gameId) throw new ArgumentException("Game id doesn't match last game id", nameof(gameId));// Make sure this is the game 

            lastGame.EndGame(didntFinish: true);
        }
        public bool IsGameOver()
        {
            var gamesList = games.ToList();
            var lastGame = gamesList[gamesList.Count - 1];
            return lastGame.gameStatus != GameStatus.OnGoing;
        }
    }
}
