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
                .OrderBy(p => p.Name.ToLower())
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
                    Name = p.Name,
                    LastGameDate = p.Games.OrderByDescending(g => g.StartTime).FirstOrDefault()?.StartTime
                })
                .OrderByDescending(p => p.Name.ToLower())
                .ToList();

            return PartialView("_PlayersLastGameSorted", players);
        }

        [HttpGet("[action]")]
        public IActionResult AllGames()
        {
            var games = _manager.GetAllPlayers()
                .SelectMany(p => p.Games)
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
                    PlayerName = p.Name,
                    GameCount = p.Games.Count
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
                    PlayerId = p.PlayerId,
                    PlayerName = p.Name,
                    PhoneNumber = p.PhoneNumber,
                    Country = p.Country,
                    GameCount = p.Games.Count
                })
                .OrderByDescending(p => p.GameCount)
                .ToList();

            return PartialView("_PlayersSortedByGameCount", players);
        }

        [HttpGet("[action]")]
        public IActionResult GamesWithUniquePlayers()
        {
            // Get all Games from all players
            var allGames = _manager.GetAllPlayers()
                .SelectMany(p => p.Games)
                .ToList();

            // Group Games by PlayerId, and select only the first game from each group
            var gamesWithUniquePlayers = allGames
                .GroupBy(g => g.PlayerId)
                .Select(group => group.First())
                .ToList();

            ViewData["Header"] = "Games With Unique Players";
            return PartialView("_Games", gamesWithUniquePlayers);
        }
        [HttpGet("[action]")]
        public IActionResult GetPlayerDropdown()
        {
            var players = _manager.GetAllPlayers()
                .OrderBy(p => p.Name)
                .Select(p => new SelectListItem
                {
                    Value = p.PlayerId.ToString(),
                    Text = p.Name
                })
                .Distinct()
                .ToList();

            return PartialView("_PlayerDropdown", players);
        }
        [HttpGet("[action]")]
        public IActionResult GetPlayerGames(int playerId)
        {
            var games = _manager.GetAllPlayers()
                .SelectMany(p => p.Games)
                .Where(g => g.PlayerId == playerId)
                .ToList();

            return PartialView("_Games", games);
        }

        [HttpGet("[action]")]
        public IActionResult GetPlayersGroupedByCountry()
        {
            var playersGroupedByCountry = _manager.GetAllPlayers()
                .GroupBy(p => p.Country)
                .Select(g => new PlayersGroupedByCountryViewModel { Country = g.Key, Players = g.ToList() })
                .Where(g => g.Players.Any())
                .ToList();

            return PartialView("_PlayersGroupedByCountry", playersGroupedByCountry);
        }

    }
}
