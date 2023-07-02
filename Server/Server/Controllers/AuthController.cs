using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Server.Services;
using System.Threading.Tasks;

namespace Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(int playerId, string password)
        {
            var user = await _authService.Login(playerId, password);

            if (user == null)
            {
                return Unauthorized();
            }

            var token = _authService.GenerateJwtToken(user);

            // Return the token as part of the response
            return Ok(new { token });
        }
    }

}


