using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Client.Utilities.Errors
{
    public static class ErrorCodes
    {
        public const string PlayerNotFound = "Player not found";
        public const string GameNotForPlayerFound = "No Games found for player";
        public const string GamesNotFound = "There are no played Games";
        public const string DBGamesNotFound = "has no Games";
    }

}
