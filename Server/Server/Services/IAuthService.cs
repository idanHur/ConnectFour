using GameManager.Models;
using System.Threading.Tasks;

namespace Server.Services
{
    public interface IAuthService
    {
        Task<Player> Login(int playerId, string password);
    }
}
