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
<<<<<<< HEAD
                // Get player details based on the id
                EditPlayer = _gameManager.GetPlayer(id.Value);
                SelectedPlayerId = EditPlayer.playerId;
=======
                // Get player details based on the Id
                EditPlayer = _gameManager.GetPlayer(id.Value);
                SelectedPlayerId = EditPlayer.PlayerId;
>>>>>>> Client
            }
        }
        public IActionResult OnPostCheckPassword()
        {
            var player = _gameManager.GetPlayer(SelectedPlayerId);

<<<<<<< HEAD
            if (player != null && player.password == PlayerPassword)
            {
                EditPlayer = player;
                playerOriginalId = player.playerId;
=======
            if (player != null && player.Password == PlayerPassword)
            {
                EditPlayer = player;
                playerOriginalId = player.PlayerId;
>>>>>>> Client
                ModelState.Clear();

                return Page();
            }
            else
            {
<<<<<<< HEAD
                ModelState.AddModelError("PlayerPassword", "Invalid password. Please try again.");
=======
                ModelState.AddModelError("PlayerPassword", "Invalid Password. Please try again.");
>>>>>>> Client
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
        public IActionResult OnPostDeletePlayerGame(int SelectedGameId)
        {
            _gameManager.DeleteGameForPlayer(playerOriginalId, SelectedGameId);

            // After the player is deleted, you might want to redirect to another page,
            // such as the index page.
            return RedirectToPage("/Index");
        }

        public IActionResult OnPostEditPlayer()
        {
<<<<<<< HEAD
            if ((playerOriginalId != EditPlayer.playerId) && (_gameManager.IsIdTaken(EditPlayer.playerId))) // If the id was changed check if the new id is taken
            {
                ModelState.AddModelError("playerId", "This ID is already taken.");
=======
            if ((playerOriginalId != EditPlayer.PlayerId) && (_gameManager.IsIdTaken(EditPlayer.PlayerId))) // If the Id was changed check if the new Id is taken
            {
                ModelState.AddModelError("PlayerId", "This ID is already taken.");
>>>>>>> Client
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
