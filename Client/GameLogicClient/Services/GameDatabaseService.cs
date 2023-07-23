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
                    .Include(p => p.Games)
                        .ThenInclude(g => g.Moves)
                    .FirstOrDefault(p => p.PlayerId == playerId);
        }
        public Game GetGame(int gameId)
        {
            return _context.Games.Include(g => g.Moves).FirstOrDefault(g => g.GameId == gameId);
        }
        public void AddPlayer(Player player)
        {
            // Check if a player with the same Id already exists
            if (!_context.Players.Any(p => p.PlayerId == player.PlayerId))
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
            var player = GetPlayer(updatedPlayerData.PlayerId);

            if (player == null)
            {
                throw new Exception($"No player found with ID {updatedPlayerData.PlayerId}");
            }

            player.Name = updatedPlayerData.Name;
            player.Country = updatedPlayerData.Country;
            player.PhoneNumber = updatedPlayerData.PhoneNumber;

            List<int> playerGameIds = GetPlayerGameIds(player.PlayerId);

            // Add Games from the server
            foreach (var game in updatedPlayerData.Games)
            {
                var existingGame = player.Games.FirstOrDefault(g => g.GameId == game.GameId);

                playerGameIds.Remove(game.GameId); // Remove the game Id from the gameIds list to state that this game still exist in the server

                if (existingGame != null)
                {
                    // Update existing game if found
                    UpdateGame(existingGame);
                }
                else
                {
                    // Add new game if not found
                    player.Games.Add(game);
                }
            }
            DeleteGames(playerGameIds); // Delete all the Games that were deleted on the server
            _context.SaveChanges();
        }
        private void DeleteGames(List<int> gameIds)
        {
            foreach (int id in gameIds)
            {
                var game = _context.Games.FirstOrDefault(g => g.GameId == id);

                if (game != null)
                {
                    _context.Games.Remove(game);
                    _context.SaveChanges();
                }
            }   
        }
        public List<int> GetPlayerGameIds(int playerId)
        {
            var gameIds = _context.Games
                                  .Where(g => g.PlayerId == playerId)
                                  .Select(g => g.GameId)
                                  .ToList();

            return gameIds;
        }

        public void UpdateGame(Game gameFromServer)
        {
            // Retrieve the game
            var gameInDb = _context.Games.Include(g => g.Moves).FirstOrDefault(g => g.GameId == gameFromServer.GameId);

            if (gameInDb == null)
            {
                throw new Exception($"No game found with ID {gameFromServer.GameId}");
            }

            // Update the game's properties.
            gameInDb.Board = gameFromServer.Board;
            gameInDb.Status = gameFromServer.Status;

            // Process each move from the server.
            foreach (var move in gameFromServer.Moves)
            {
                var existingMove = gameInDb.Moves.FirstOrDefault(m => m.Id == move.Id);

                if (existingMove == null)
                {
                    // The move doesn't exist in the database yet. Add it.
                    gameInDb.Moves.Add(move);
                }
            }

            // Save changes to the database
            _context.SaveChanges();
        }

        public Game GetLastGameOfPlayer(int playerId)
        {
            // Retrieve the player with the Games
            var player = GetPlayer(playerId);

            if (player == null)
            {
                throw new Exception($"No player found with ID {playerId}");
            }

            // Retrieve the player's last game
            var lastGame = player.Games.OrderByDescending(g => g.GameId).FirstOrDefault();

            if (lastGame == null)
            {
                throw new Exception($"Player with ID {playerId} has no Games.");
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
            player.Games.Add(newGame);

            // Save changes to the database
            _context.SaveChanges();
        }


    }
}
