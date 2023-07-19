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
                ModelState.Clear();
                return Page();
            }
            else
            {
                ModelState.AddModelError("PlayerPassword", "Invalid password. Please try again.");
                return Page();
            }
        }

     
    }
}
