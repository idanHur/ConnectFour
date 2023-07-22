using Connect4Game;
using GameManager.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Claims;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Server.Controllers
{
    [Route("api/")]
    [ApiController]
    public class GameManagerController : ControllerBase
    {
        const int ROWS = 6;
        const int COLS = 7;
        private readonly Manager _gameManager;

        public GameManagerController(Manager gameManager)
        {
            _gameManager = gameManager;
        }

        [HttpPost("{PlayerId}/start")]
        [Authorize]
        public IActionResult StartGame(int playerId)
        {
            // Get the authenticated user's ID
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // Compare the authenticated user's ID with the PlayerId parameter
            if (userId != playerId.ToString())
            {
                return Forbid(); // Return 403 Forbidden if the user is not authorized to start the game
            }

            try
            {
                // Start a new game
                Game newGame = _gameManager.StartNewGameForPlayer(playerId, ROWS, COLS);

                if (newGame != null)
                {
                    // Return the game ID
                    return Ok(newGame);
                }
                else
                {
                    return BadRequest(new { error = "Did not start a new game" });
                }
            }
            catch (Exception ex)
            {
                // If there was an error starting the game, return a server error
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpPut("{PlayerId}/move")]
        public IActionResult Move(int playerId, [FromBody] int playerMove)
        {
            

            try
            {
                // Try to apply the move
                Game gameState = _gameManager.MakeMoveForPlayer(playerId, playerMove);

                if (gameState != null)
                {

                    // If successful, return the game state
                    return Ok(gameState);
                }
                else
                {
                    return BadRequest(new { error = "Cannot make this move" });
                }
            }
            catch (Exception ex)
            {
                // If the move was invalid, return a bad request
                return BadRequest(new { error = ex.Message });
            }
        }
        [HttpPut("{PlayerId}/endGame")]
        [Authorize]
        public IActionResult EndGame(int playerId, [FromBody] int gameId)
        {
            // Get the authenticated user's ID
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // Compare the authenticated user's ID with the PlayerId parameter
            if (userId != playerId.ToString())
            {
                return Forbid(); // Return 403 Forbidden if the user is not authorized to make the move
            }

            try
            {
                // End the last game of this player
                Game endedGame = _gameManager.EndGameForPlayer(playerId, gameId);

                return Ok(endedGame);

            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
        [HttpPut("{PlayerId}/aiMove")]
        [Authorize]
        public IActionResult AiMove(int playerId, [FromBody] int gameId)
        {
            // Get the authenticated user's ID
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // Compare the authenticated user's ID with the PlayerId parameter
            if (userId != playerId.ToString())
            {
                return Forbid(); // Return 403 Forbidden if the user is not authorized to make the move
            }

            try
            {
                // End the last game of this player
                Game gameState = _gameManager.MakeAiMoveForPlayerGame(playerId, gameId);

                if (gameState != null)
                {
                    // If successful, return the game state
                    return Ok(gameState);
                }
                else
                {
                    return BadRequest(new { error = "Cannot make this move" });
                }

            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

    }
}
