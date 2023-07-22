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
            return _context.Players
                    .Include(p => p.games)
                        .ThenInclude(g => g.moves)
                    .FirstOrDefault(p => p.playerId == playerId);
        }

        public void AddPlayer(Player player)
        {
            // Check if a player with the same id already exists
            if (!_context.Players.Any(p => p.playerId == player.playerId))
            {
                _context.Players.Add(player);
                _context.SaveChanges();
            }
            else
            {
                UpdatePlayer(player);
            }
        }

        public void UpdatePlayer(Player updatedPlayerData)
        {
            var player = GetPlayer(updatedPlayerData.playerId);

            if (player == null)
            {
                throw new Exception($"No player found with ID {updatedPlayerData.playerId}");
            }

            player.playerName = updatedPlayerData.playerName;
            player.country = updatedPlayerData.country;
            player.phoneNumber = updatedPlayerData.phoneNumber;

            // Add games from the server
            foreach (var game in updatedPlayerData.games)
            {
                var existingGame = player.games.FirstOrDefault(g => g.gameId == game.gameId);

                if (existingGame != null)
                {
                    // Update existing game if found
                    UpdateGame(existingGame);
                }
                else
                {
                    // Add new game if not found
                    player.games.Add(game);
                }
            }
            _context.SaveChanges();
        }
        public void UpdateGame(Game gameFromServer)
        {
            // Retrieve the game
            var gameInDb = _context.Games.Include(g => g.moves).FirstOrDefault(g => g.gameId == gameFromServer.gameId);

            if (gameInDb == null)
            {
                throw new Exception($"No game found with ID {gameFromServer.gameId}");
            }

            // Update the game's properties.
            gameInDb.board = gameFromServer.board;
            gameInDb.gameStatus = gameFromServer.gameStatus;

            // Process each move from the server.
            foreach (var move in gameFromServer.moves)
            {
                var existingMove = gameInDb.moves.FirstOrDefault(m => m.id == move.id && m.GameId == move.GameId);

                if (existingMove == null)
                {
                    // The move doesn't exist in the database yet. Add it.
                    gameInDb.moves.Add(move);
                }
            }

            // Save changes to the database
            _context.SaveChanges();
        }

        public Game GetLastGameOfPlayer(int playerId)
        {
            // Retrieve the player with the games
            var player = GetPlayer(playerId);

            if (player == null)
            {
                throw new Exception($"No player found with ID {playerId}");
            }

            // Retrieve the player's last game
            var lastGame = player.games.OrderByDescending(g => g.gameId).FirstOrDefault();

            if (lastGame == null)
            {
                throw new Exception($"Player with ID {playerId} has no games.");
            }

            // Return the last game
            return lastGame;
        }

        public void AddGameToPlayer(int playerId, Game newGame)
        {
            // Retrieve the player
            var player = GetPlayer(playerId);

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
