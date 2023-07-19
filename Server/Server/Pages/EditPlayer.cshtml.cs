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

      
    }
}
