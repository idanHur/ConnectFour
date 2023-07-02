using GameManager.Data;
using GameManager.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Server.Services
{
    public class AuthService : IAuthService
    {
        // Assume you have a context to your user database
        private readonly MyDbContext _context;

        public AuthService(MyDbContext context)
        {
            _context = context;
        }

        public async Task<Player> Login(int playerId, string password)
        {
            var user = await _context.Players
                .Where(u => u.playerId == playerId && u.password == password) // In real-world application, password should be hashed and salted!
                .FirstOrDefaultAsync();

            return user;
        }
    }

}
