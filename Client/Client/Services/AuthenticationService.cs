using GameLogic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Services
{
    public class AuthenticationService
    {
        private string _jwtToken;
        private Player _currentPlayer;

        public AuthenticationService(Player player)
        {
            _currentPlayer = player;
        }

        public void SaveJwtToken(string token)
        {
            _jwtToken = token;
        }

        public string GetJwtToken()
        {
            return _jwtToken;
        }

        public void SetCurrentPlayerFromServerData(Player player)
        {
            _currentPlayer.country = player.country;
            _currentPlayer.games = player.games.ToList();
            _currentPlayer.phoneNumber = player.phoneNumber;
            _currentPlayer.playerName = player.playerName;
            _currentPlayer.playerId = player.playerId;
        }

        public Player GetCurrentPlayer()
        {
            return _currentPlayer;
        }
    }

}
