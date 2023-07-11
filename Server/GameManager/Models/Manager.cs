﻿using Connect4Game;
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
            Game gameFromDB = _context.Games.Where(g => g.playerId == playerId).OrderByDescending(g => g.startTime).FirstOrDefault();
            temp.gameId = gameFromDB.gameId;
            return temp;
        }

        public Game MakeMoveForPlayer(int playerId, int column)
        {
            Player player = GetPlayer(playerId);
            if (player == null) throw new InvalidOperationException($"Player not found, playerId: {playerId}");
            Game lastGame = GetPlayerLastGame(playerId);
            if (lastGame.gameStatus == GameStatus.OnGoing) // If a game is curently played
            {
                Move moveMade = lastGame.PlayerMove(column);
                return player.GetLastGame(); // Return the new game state after the move
            }
            throw new ArgumentException("There is no game in progress");
        }
        public Game GetPlayerLastGame(int playerId)
        {
            var player = GetPlayer(playerId);
            if (player == null) throw new InvalidOperationException($"Player not found, playerId: {playerId}");

            var lastGame = player.games.OrderByDescending(g => g.startTime).FirstOrDefault();

            if (lastGame == null) throw new InvalidOperationException($"No games found for player, playerId: {playerId}");

            return lastGame;
        }
        public Game MakeAiMoveForPlayerGame(int playerId, int gameId)
        {
            Player player = GetPlayer(playerId);
            if (player == null) throw new InvalidOperationException($"Player not found, playerId: {playerId}");
            if (player.GetLastGame().gameStatus == GameStatus.OnGoing) // If a game is curently played
            {
                player.GetLastGame().AiMove();
                return player.GetLastGame(); // Return the new game state after the move
            }
            throw new ArgumentException("There is no game in progress");
        }
        public Player GetPlayer(int id)
        {
            var player = _context.Players
                .Include(p => p.games)
                    .ThenInclude(g => g.board) // Load the Board related to the Game
                .Include(p => p.games)
                    .ThenInclude(g => g.Moves) // Load the Moves related to the Game
                .FirstOrDefault(p => p.playerId == id);
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
