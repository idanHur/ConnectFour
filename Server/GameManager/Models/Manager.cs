using Connect4Game;
using GameManager.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace GameManager.Models
{
    public class Manager
    {
        private readonly MyDbContext _context;

        public Manager(MyDbContext context)
        {
            _context = context;
        }

        public Game StartNewGameForPlayer(int playerId, int row, int column)
        {
            Player player = GetPlayer(playerId);
            if (player == null)
                return null;
            Game temp = player.NewGame(row, column);
            _context.SaveChanges();
            Game gameFromDB = _context.Games.Where(g => g.PlayerId == playerId).OrderByDescending(g => g.StartTime).FirstOrDefault();
            temp.GameId = gameFromDB.GameId;
            return temp;
        }

        public Game MakeMoveForPlayer(int playerId, int column)
        {
            Player player = GetPlayer(playerId);
            if (player == null) throw new InvalidOperationException($"Player not found, PlayerId: {playerId}");
            Game lastGame = GetPlayerLastGame(playerId);
            if (lastGame.Status == GameStatus.OnGoing) // If a game is curently played
            {
                Move moveMade = lastGame.PlayerMove(column);
                _context.SaveChanges();
                return player.GetLastGame(); // Return the new game state after the move
            }
            throw new ArgumentException("There is no game in progress");
        }
        public Game GetPlayerLastGame(int playerId)
        {
            var player = GetPlayer(playerId);
            if (player == null) throw new InvalidOperationException($"Player not found, PlayerId: {playerId}");

            var lastGame = player.Games.OrderByDescending(g => g.StartTime).FirstOrDefault();

            if (lastGame == null) throw new InvalidOperationException($"No Games found for player, PlayerId: {playerId}");

            return lastGame;
        }
        public Game MakeAiMoveForPlayerGame(int playerId, int gameId)
        {
            Player player = GetPlayer(playerId);
            if (player == null) throw new InvalidOperationException($"Player not found, PlayerId: {playerId}");
            if (player.GetLastGame().Status == GameStatus.OnGoing) // If a game is curently played
            {
                player.GetLastGame().AiMove();
                _context.SaveChanges();
                return player.GetLastGame(); // Return the new game state after the move
            }
            throw new ArgumentException("There is no game in progress");
        }
        public Player GetPlayer(int id)
        {
            var player = _context.Players
                .Include(p => p.Games)
                    .ThenInclude(g => g.Board) // Load the Board related to the Game
                .Include(p => p.Games)
                    .ThenInclude(g => g.Moves) // Load the Moves related to the Game
                .FirstOrDefault(p => p.PlayerId == id);
            return player;
        }
        public Game EndGameForPlayer(int playerId, int gameId)
        {
            Player player = GetPlayer(playerId);
            if (player == null) throw new InvalidOperationException($"Player not found, PlayerId: {playerId}");
            Game endedGame = player.EndLastGame(gameId);
            _context.SaveChanges();
            return endedGame;
        }

        public bool IsIdTaken(int id)
        {
            return _context.Players.Any(player => player.PlayerId == id);
        }

        public void AddPlayer(Player player)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                _context.Database.ExecuteSqlCommand("SET IDENTITY_INSERT Players ON");
                _context.Players.Add(player);
                _context.SaveChanges();
                _context.Database.ExecuteSqlCommand("SET IDENTITY_INSERT Players OFF");
                transaction.Commit();
            }

        }
        public void UpdatePlayer(int originalId, Player editedPlayer)
        {
            Player player = GetPlayer(originalId);
<<<<<<< HEAD
            if (player == null) throw new InvalidOperationException($"Player not found, playerId: {originalId}");
            if (editedPlayer == null) throw new InvalidOperationException("Must pass player");

            if(originalId != editedPlayer.playerId)
            {
                editedPlayer.games = player.games;
=======
            if (player == null) throw new InvalidOperationException($"Player not found, PlayerId: {originalId}");
            if (editedPlayer == null) throw new InvalidOperationException("Must pass player");

            if(originalId != editedPlayer.PlayerId)
            {
                editedPlayer.Games = player.Games;
>>>>>>> Staging
                editedPlayer.ChangeGamesForUpdateUser();
                DeletePlayer(originalId);
                AddPlayer(editedPlayer);
            }
            else
            {
<<<<<<< HEAD
                player.playerName = editedPlayer.playerName;
                player.phoneNumber = editedPlayer.phoneNumber;
                player.country = editedPlayer.country;
                player.password = editedPlayer.password;
=======
                player.Name = editedPlayer.Name;
                player.PhoneNumber = editedPlayer.PhoneNumber;
                player.Country = editedPlayer.Country;
                player.Password = editedPlayer.Password;
>>>>>>> Staging
                _context.SaveChanges();
            }
        }
        public void DeletePlayer(int playerId)
        {
            Player player = GetPlayer(playerId);
<<<<<<< HEAD
            if (player == null) throw new InvalidOperationException($"Player not found, playerId: {playerId}");
=======
            if (player == null) throw new InvalidOperationException($"Player not found, PlayerId: {playerId}");
>>>>>>> Staging

            _context.Players.Remove(player);
            _context.SaveChanges();
        }
        public void DeleteGameForPlayer(int playerId, int gameId)
        {
            Player player = GetPlayer(playerId);
<<<<<<< HEAD
            if (player == null) throw new InvalidOperationException($"Player not found, playerId: {playerId}");
=======
            if (player == null) throw new InvalidOperationException($"Player not found, PlayerId: {playerId}");
>>>>>>> Staging

            player.DeleteGame(gameId);
            _context.SaveChanges();
        }
        public List<Player> GetAllPlayers()
        {
            return _context.Players
<<<<<<< HEAD
                .Include(p => p.games)
                    .ThenInclude(g => g.board)
                .Include(p => p.games)
=======
                .Include(p => p.Games)
                    .ThenInclude(g => g.Board)
                .Include(p => p.Games)
>>>>>>> Staging
                    .ThenInclude(g => g.Moves)
                .ToList();
        }

    }
}
