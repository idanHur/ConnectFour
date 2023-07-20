using GameManager.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System;
using System.Linq;
using Server.Pages.Players;
using Server.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;

namespace Server.Controllers
{
    [Route("api/[controller]")]

    public class PlayersController : Controller
    {
        private readonly Manager _manager;

        public PlayersController(Manager manager)
        {
            _manager = manager;
        }
        [HttpGet("[action]")]
        public IActionResult AllPlayers()
        {
            var players = _manager.GetAllPlayers();
            ViewData["Header"] = "All Players";
            return PartialView("_AllPlayers", players);
        }
        [HttpGet("[action]")]
        public IActionResult SortByName()
        {
            var players = _manager.GetAllPlayers()
                .OrderBy(p => p.playerName.ToLower())
                .ToList();
            ViewData["Header"] = "All Players Sorted Ascending";
            return PartialView("_AllPlayers", players); 
        }
        [HttpGet("[action]")]
        public IActionResult PlayersLastGameSorted()
        {
            var players = _manager.GetAllPlayers()
                .Select(p => new PlayersLastGameSortedViewModel
                {
                    Name = p.playerName,
                    LastGameDate = p.games.OrderByDescending(g => g.startTime).FirstOrDefault()?.startTime
                })
                .OrderByDescending(p => p.Name.ToLower())
                .ToList();

            return PartialView("_PlayersLastGameSorted", players);
        }

        [HttpGet("[action]")]
        public IActionResult AllGames()
        {
            var games = _manager.GetAllPlayers()
                .SelectMany(p => p.games)
                .ToList();

            ViewData["Header"] = "All Games";
            return PartialView("_Games", games);
        }
        [HttpGet("[action]")]
        public IActionResult PlayerGameCounts()
        {
            var playerGameCounts = _manager.GetAllPlayers()
                .Select(p => new PlayerGameCountViewModel
                {
                    PlayerName = p.playerName,
                    GameCount = p.games.Count
                })
                .ToList();

            return PartialView("_PlayerGameCounts", playerGameCounts);
        }

        [HttpGet("[action]")]
        public IActionResult PlayersSortedByGameCount()
        {
            var players = _manager.GetAllPlayers()
                .Select(p => new PlayerSortedByGameCountViewModel
                {
                    PlayerId = p.playerId,
                    PlayerName = p.playerName,
                    PhoneNumber = p.phoneNumber,
                    Country = p.country,
                    GameCount = p.games.Count
                })
                .OrderByDescending(p => p.GameCount)
                .ToList();

            return PartialView("_PlayersSortedByGameCount", players);
        }

        [HttpGet("[action]")]
        public IActionResult GamesWithUniquePlayers()
        {
            // Get all games from all players
            var allGames = _manager.GetAllPlayers()
                .SelectMany(p => p.games)
                .ToList();

            // Group games by playerId, and select only the first game from each group
            var gamesWithUniquePlayers = allGames
                .GroupBy(g => g.playerId)
                .Select(group => group.First())
                .ToList();

            ViewData["Header"] = "Games With Unique Players";
            return PartialView("_Games", gamesWithUniquePlayers);
        }
        [HttpGet("[action]")]
        public IActionResult GetPlayerDropdown()
        {
            var players = _manager.GetAllPlayers()
                .OrderBy(p => p.playerName)
                .Select(p => new SelectListItem
                {
                    Value = p.playerId.ToString(),
                    Text = p.playerName
                })
                .Distinct()
                .ToList();

            return PartialView("_PlayerDropdown", players);
        }
        [HttpGet("[action]")]
        public IActionResult GetPlayerGames(int playerId)
        {
            var games = _manager.GetAllPlayers()
                .SelectMany(p => p.games)
                .Where(g => g.playerId == playerId)
                .ToList();

            return PartialView("_Games", games);
        }

        [HttpGet("[action]")]
        public IActionResult GetPlayersGroupedByCountry()
        {
            var playersGroupedByCountry = _manager.GetAllPlayers()
                .GroupBy(p => p.country)
                .Select(g => new PlayersGroupedByCountryViewModel { Country = g.Key, Players = g.ToList() })
                .Where(g => g.Players.Any())
                .ToList();

            return PartialView("_PlayersGroupedByCountry", playersGroupedByCountry);
        }

    }
}
