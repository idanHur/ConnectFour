using GameManager.Models;
using GameManager.Utilities.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Server.Models;
using Server.Services;
using System;
using System.Threading.Tasks;

namespace Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly Manager _gameManager;


        public AuthController(IAuthService authService, Manager gameManager)
        {
            _authService = authService;
            _gameManager = gameManager;

        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            try
            {
                var user = await _authService.Login(model.PlayerId, model.Password);

                if (user == null)
                {
                    return Unauthorized();
                }

                var player = _gameManager.GetPlayer(model.PlayerId);

                if (player == null)
                {
                    return BadRequest(new { error = "There is no player with this Id" });
                }

                // Create JSON object from Player using the custom PlayerConverter
                string playerJson = JsonConvert.SerializeObject(player, new PlayerConverter());

                var token = _authService.GenerateJwtToken(user);

                // Return the token and player object as part of the response
                return Ok(new { token, player = playerJson });
            }
            catch (Exception ex)
            {
                // Return the exception message as the error response
                return StatusCode(500, new { error = ex.Message });
            }
        }

    }
}


