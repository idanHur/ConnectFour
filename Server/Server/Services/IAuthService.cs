using GameManager.Models;
using System.Threading.Tasks;

namespace Server.Services
{
    public interface IAuthService
    {
        string GenerateJwtToken(Player user);

        Task<Player> Login(int playerId, string password);
    }
}
