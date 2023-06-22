using Connect4Game;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameManagerController : ControllerBase
    {
        const int ROWS = 6;
        const int COLS = 7;
        private readonly GameManager.Manager _gameManager;

        public GameManagerController(GameManager.Manager gameManager)
        {
            _gameManager = gameManager;
        }

        [HttpPost("{playerId}/start")]
        public IActionResult StartGame(int playerId)
        {
            try
            {
                // Start a new game
                var newGame = _gameManager.StartNewGameForPlayer(playerId, ROWS, COLS);

                if (newGame != null)
                {
                    // Return the game ID
                    return Ok(newGame); // The object will be serialized to JSON format automatically 
                }
                else
                {
                    return BadRequest(new { error = "Didnt start a new game" });
                }
            }
            catch (Exception ex)
            {
                // If there was an error starting the game, return a server error
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpPost("{playerId}/move")]
        public IActionResult Move(int playerId, [FromBody] int playerMove)
        {
            try
            {
                // Try to apply the move
                var gameState = _gameManager.MakeMoveForPlayer(playerId, playerMove);

                if (gameState)
                {
                    // If successful, return the game state
                    return Ok(gameState);
                }
                else
                {
                    return BadRequest(new { error = "Cant make this move" });
                }        
            }
            catch (Exception ex)
            {
                // If the move was invalid, return a bad request
                return BadRequest(new { error = ex.Message });
            }
        }

        // GET: api/<GameManagerController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<GameManagerController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<GameManagerController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<GameManagerController>/5
        [HttpPut("{column}")]
        public void PutMove([FromBody] int column)
        {
            
        }

        // DELETE api/<GameManagerController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
