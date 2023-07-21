using GameManager.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Server.Pages
{
    public class RegisterModel : PageModel
    {
        [BindProperty]
        public Player NewPlayer { get; set; }
        private readonly Manager _gameManager;

        public RegisterModel(Manager gameManager)
        {
            _gameManager = gameManager;
            NewPlayer = new Player();
        }
        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            if (_gameManager.IsIdTaken(NewPlayer.playerId))
            {
                ModelState.AddModelError("playerId", "This ID is already taken.");
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Save the player 
            _gameManager.AddPlayer(NewPlayer);
            // Redirect to Index page.
            return RedirectToPage("/Index");
        }


    }
}