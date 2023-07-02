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
        public async Task<IActionResult> Login(int id, string password)
        {
            var user = await _authService.Login(id, password);

            if (user == null)
            {
                return Unauthorized();
            }

            // TODO: create token for identification

            return Ok(new {  });
        }
    }

}
