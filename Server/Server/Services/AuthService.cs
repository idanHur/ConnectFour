using GameManager.Data;
using GameManager.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System;
using Microsoft.Extensions.Configuration;

namespace Server.Services
{
    public class AuthService : IAuthService
    {
        // Assume you have a context to your user database
        private readonly IConfiguration _configuration;
        private readonly MyDbContext _context;

        public AuthService(IConfiguration configuration, MyDbContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        public async Task<Player> Login(int playerId, string password)
        {
            var user = await _context.Players
                .Where(u => u.playerId == playerId && u.password == password) // In real-world application, password should be hashed and salted!
                .FirstOrDefaultAsync();

            return user;
        }
        public string GenerateJwtToken(Player user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:SecretKey"]);

            // Define the token descriptor, which contains the claims, expiration, and signing credentials
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim(ClaimTypes.NameIdentifier, user.playerId.ToString()) }), // Include the user's playerId as the NameIdentifier claim
                Expires = DateTime.UtcNow.AddDays(7), // Set the token expiration time
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature), // Specify the signing credentials using the secret key and the HMACSHA256 signature algorithm
                Audience = _configuration["Jwt:Audience"], // Set the expected audience value for the token
                Issuer = _configuration["Jwt:Issuer"] // Set the issuer value for the token
            };

            var token = tokenHandler.CreateToken(tokenDescriptor); // Create the token based on the token descriptor
            var encodedToken = tokenHandler.WriteToken(token); // Write the token as a string

            return encodedToken; // Return the encoded token
        }




    }

}
