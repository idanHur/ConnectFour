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
        public int PlayerId { get; set; }

        [Required(ErrorMessage = "Private Name is required.")]
        [MinLength(2, ErrorMessage = "Private Name should be at least 2 characters long.")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Phone Number is required.")]
        [RegularExpression(@"^\d{9,10}$", ErrorMessage = "Phone Number should be between 9 and 10 digits.")]
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage = "Country is required.")]
        public string Country { get; set; }
        [Required(ErrorMessage = "Password is required.")]
        [MinLength(4, ErrorMessage = "Password should be at least 4 characters long.")]
        public string Password { get; set; }

        // EF Core will automatically load the related Game entities when accessing this property.
        public ICollection<Game> Games { get; set; }


        // Parameterless constructor
        public Player()
        {
            Games = new List<Game>(); // Initialize the collection
        }
        public Player(string playerName, int playerId)
        {
            this.Name = playerName;
            this.PlayerId = playerId;
            Games = new List<Game>(); // Initialize the collection
        }
        public Game NewGame(int rows, int columns)
        {
            Game newGame = new Game(rows, columns, PlayerId);
            Games.Add(newGame);
            return newGame;
        }
        public void ChangeGamesForUpdateUser()
        {
            List<Game> tempGames = new List<Game>();
<<<<<<< HEAD
            foreach (var game in games)
            {
                // Create a new Game object and copy properties except gameId
                Game tempGame = new Game()
                {
                    board = game.board,
                    gameStatus = game.gameStatus,
                    currentPlayer = game.currentPlayer,
                    startTime = game.startTime,
                    gameDuration = game.gameDuration,
                    playerId = game.playerId,
=======
            foreach (var game in Games)
            {
                // Create a new Game object and copy properties except GameId
                Game tempGame = new Game()
                {
                    Board = game.Board,
                    Status = game.Status,
                    CurrentPlayer = game.CurrentPlayer,
                    StartTime = game.StartTime,
                    GameDuration = game.GameDuration,
                    PlayerId = game.PlayerId,
>>>>>>> Staging
                    Moves = game.Moves
                };

                // Add the new game to the tempGames list
                tempGames.Add(tempGame);
            }

<<<<<<< HEAD
            // Replace the games list with tempGames
            games = tempGames;
=======
            // Replace the Games list with tempGames
            Games = tempGames;
>>>>>>> Staging
        }
        public Game GetLastGame()
        {
            var gamesList = Games.ToList();
            var lastGame = gamesList[gamesList.Count - 1];
            return lastGame;
        }
        public void DeleteGame(int id)
<<<<<<< HEAD
        {
            // Find the game in the collection
            Game gameToRemove = games.FirstOrDefault(g => g.gameId == id);
            // Check if a game with that Id was found
            if (gameToRemove == null) throw new InvalidOperationException($"The game wasnt found");    
            // If found, remove it
            games.Remove(gameToRemove);
        }

        public void EndLastGame(int gameId)
=======
>>>>>>> Staging
        {
            // Find the game in the collection
            Game gameToRemove = Games.FirstOrDefault(g => g.GameId == id);
            // Check if a game with that Id was found
            if (gameToRemove == null) throw new InvalidOperationException($"The game wasnt found");    
            // If found, remove it
            Games.Remove(gameToRemove);
        }

        public Game EndLastGame(int gameId)
        {
            var gamesList = Games.ToList();
            var lastGame = gamesList[gamesList.Count - 1];
            if (lastGame == null) throw new InvalidOperationException($"There are no played Games");
            if (lastGame.GameId != gameId) throw new ArgumentException("Game Id doesn't match last game Id", nameof(gameId));// Make sure this is the game 

            lastGame.EndGame(didntFinish: true);
            return lastGame;
        }
    }
}
