using Connect4Game;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace GameManager
{
    public class Manager
    {
        private List<Player> _playersRecord = new List<Player>();

        public ReadOnlyCollection<Player> playersRecord => _playersRecord.AsReadOnly();

        public Manager()
        {
            _playersRecord = new List<Player>();
        }

        public Game StartNewGameForPlayer(int playerId, int row, int column)
        {
            Player player = GetPlayer(playerId);
            if (player == null)
                return null;
            Game temp = player.NewGame(row, column);    
            return temp;
        }

        public bool MakeMoveForPlayer(int playerId, int column)
        {
            Player player = GetPlayer(playerId);
            if (player == null)
                return false;
            if (player.currentGame.gameStatus == GameStatus.OnGoing) // If a game is curently played
            {
                bool moveMade = player.currentGame.PlayerMove(column);
                return moveMade;
            }
            return false;        
        }
        private Player GetPlayer(int id)
        {
            var player = _playersRecord.FirstOrDefault(p => p.playerId == id);
            return player;
        }
        public bool IsIdTaken(int id)
        {
            return _playersRecord.Any(player => player.playerId == id);
        }
        public void AddPlayer(Player player)
        {
            _playersRecord.Add(player);
        }
    }
}
