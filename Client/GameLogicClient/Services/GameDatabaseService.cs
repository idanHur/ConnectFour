using GameLogicClient.Data;
using GameLogicClient.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLogicClient.Services
{
    public class GameDatabaseService
    {
        private readonly GameContext _context;

        public GameDatabaseService(GameContext context)
        {
            _context = context;
        }

        public Player GetPlayer(int playerId)
        {
            return _context.Players.FirstOrDefault(p => p.playerId == playerId);
        }
        public void AddPlayer(Player player)
        {
            _context.Players.Add(player);
            _context.SaveChanges();
        }
        public void UpdatePlayer(Player updatedPlayerData)
        {
            var player = _context.Players.Find(updatedPlayerData.playerId);

            if (player == null)
            {
                throw new Exception($"No player found with ID {updatedPlayerData.playerId}");
            }

            player.playerName = updatedPlayerData.playerName;
            player.country = updatedPlayerData.country;
            player.phoneNumber = updatedPlayerData.phoneNumber;
            player.games = updatedPlayerData.games.ToList<Game>();

            _context.SaveChanges();
        }
        public void AddMoveToLastGameOfPlayer(int playerId, Move newMove)
        {
            // Retrieve the player
            var player = _context.Players.Include(p => p.games).FirstOrDefault(p => p.playerId == playerId);

            if (player == null)
            {
                throw new Exception($"No player found with ID {playerId}");
            }

            // Retrieve the player's last game
            var lastGame = player.games.OrderBy(g => g.gameId).LastOrDefault();

            if (lastGame == null)
            {
                throw new Exception($"Player with ID {playerId} has no games.");
            }

            // Add the move to the game
            lastGame.moves.Add(newMove);

            // Save changes to the database
            _context.SaveChanges();
        }
        public void AddGameToPlayer(int playerId, Game newGame)
        {
            // Retrieve the player
            var player = _context.Players.Include(p => p.games).FirstOrDefault(p => p.playerId == playerId);

            if (player == null)
            {
                throw new Exception($"No player found with ID {playerId}");
            }

            // Add the game to the player
            player.games.Add(newGame);

            // Save changes to the database
            _context.SaveChanges();
        }


    }
}
