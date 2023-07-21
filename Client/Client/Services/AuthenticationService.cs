using GameLogicClient.Models;
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

        public AuthenticationService()
        {
        }

        public void SaveJwtToken(string token)
        {
            _jwtToken = token;
        }

        public string GetJwtToken()
        {
            return _jwtToken;
        }

    }

}
