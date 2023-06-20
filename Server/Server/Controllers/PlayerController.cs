using GameManager;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Server.Controllers
{
    public class PlayerController : Controller
    {
        private readonly Manager _gameManager;

        public PlayerController(Manager gameManager)
        {
            _gameManager = gameManager;
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Player model)
        {
            if (ModelState.IsValid)
            {
                // Add the player to the game manager
                _gameManager.AddPlayer(model);

                return RedirectToAction("Index", "Home"); // Redirect to a success page or a different page
            }

            // The model is invalid, return the view to display the validation messages.
            return View(model);
        }

        [HttpGet]
        public IActionResult IsPlayerIdAvailable(int playerId)
        {
            // Perform the necessary logic to check if the player ID is available
            bool isAvailable = !_gameManager.playersRecord.Any(p => p.playerId == playerId);

            return Json(isAvailable);
        }
    }
}
