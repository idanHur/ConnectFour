using Connect4Game;
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

        public Game currentGame { get; private set; }

        // EF Core will automatically load the related Game entities when accessing this property.
        public ICollection<Game> Games { get; set; }


        // Parameterless constructor
        public Player()
        {
        }
        public Player(string playerName, int playerId)
        {
            this.playerName = playerName;
            this.playerId = playerId;
            currentGame = null;
        }
        public Game NewGame(int rows, int columns)
        {
            currentGame = new Game(rows, columns, Games.Count + 1, playerId);
            Games.Add(currentGame);
            return currentGame;
        }
    }
}
