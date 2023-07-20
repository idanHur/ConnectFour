using GameManager.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Server.Pages
{
    public class EditPlayerModel : PageModel
    {
        [BindProperty]
        public Player EditPlayer { get; set; }

        [BindProperty]
        public int SelectedPlayerId { get; set; }

        [BindProperty]
        public string PlayerPassword { get; set; }

        public List<Player> Players { get; set; }
        [BindProperty]
        public int playerOriginalId { get; set; }

        private readonly Manager _gameManager;

        public EditPlayerModel(Manager gameManager)
        {
            _gameManager = gameManager;
            Players = _gameManager.GetAllPlayers();
        }

        public void OnGet(int? id)
        {
            if (id.HasValue)
            {
                // Get player details based on the id
                EditPlayer = _gameManager.GetPlayer(id.Value);
                SelectedPlayerId = EditPlayer.playerId;
            }
        }
        public IActionResult OnPostCheckPassword()
        {
            var player = _gameManager.GetPlayer(SelectedPlayerId);

            if (player != null && player.password == PlayerPassword)
            {
                EditPlayer = player;
                playerOriginalId = player.playerId;
                ModelState.Clear();

                return Page();
            }
            else
            {
                ModelState.AddModelError("PlayerPassword", "Invalid password. Please try again.");
                return Page();
            }
        }
        public IActionResult OnPostDeletePlayer()
        {
            _gameManager.DeletePlayer(SelectedPlayerId);

            // After the player is deleted, you might want to redirect to another page,
            // such as the index page.
            return RedirectToPage("/Index");
        }


        public IActionResult OnPost()
        {
            if ((playerOriginalId != EditPlayer.playerId) && (_gameManager.IsIdTaken(EditPlayer.playerId))) // If the id was changed check if the new id is taken
            {
                ModelState.AddModelError("playerId", "This ID is already taken.");
            }

            // Validate ModelState
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Update the player details
            _gameManager.UpdatePlayer(playerOriginalId, EditPlayer);

            // Redirect to Index page.
            return RedirectToPage("/Index");
        }
    }
}
