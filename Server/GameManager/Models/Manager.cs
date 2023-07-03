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
            return temp;
        }

        public bool MakeMoveForPlayer(int playerId, int column)
        {
            Player player = GetPlayer(playerId);
            if (player == null)
                return false;
            if (player.GetLastGame().gameStatus == GameStatus.OnGoing) // If a game is curently played
            {
                bool moveMade = player.GetLastGame().PlayerMove(column);
                return moveMade;
            }
            return false;
        }
        private Player GetPlayer(int id)
        {
            var player = _context.Players.FirstOrDefault(p => p.playerId == id);
            return player;
        }
        public void EndGameForPlayer(int playerId, int gameId)
        {
            Player player = GetPlayer(playerId);
            if (player == null) throw new InvalidOperationException($"Player not found, playerId: {playerId}");
            if (player.IsGameOver()) return; // The last game was already ended 
            player.EndLastGame(gameId);
        }

        public bool IsIdTaken(int id)
        {
            return _context.Players.Any(player => player.playerId == id);
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

        public List<Player> GetAllPlayers()
        {
            return _context.Players.ToList();
        }
    }
}
