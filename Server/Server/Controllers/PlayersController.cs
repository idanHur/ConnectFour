using GameManager.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System;
using System.Linq;
using Server.Pages.Players;
using Server.ViewModels;

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
            return PartialView("_AllPlayers", players);
        }
        [HttpGet("[action]")]
        public IActionResult SortByName()
        {
            var players = _manager.GetAllPlayers()
                .OrderBy(p => p.playerName.ToLower())
                .ToList();

            return PartialView("_SortByName", players); 
        }
        [HttpGet("[action]")]
        public IActionResult SortByNameAndGameDateDescending()
        {
            var players = _manager.GetAllPlayers()
                .Select(p => new PlayersByNameDescendingViewModel
                {
                    Name = p.playerName,
                    LastGameDate = p.games.OrderByDescending(g => g.startTime).FirstOrDefault()?.startTime
                })
                .OrderByDescending(p => p.Name.ToLower())
                .ToList();

            return PartialView("_SortByNameAndGameDateDescending", players);
        }

        [HttpGet("[action]")]
        public IActionResult AllGames()
        {
            var games = _manager.GetAllPlayers()
                .SelectMany(p => p.games)
                .ToList();

            return PartialView("_AllGames", games);
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
                .OrderByDescending(p => p.games.Count)
                .ToList();

            return PartialView("_PlayersSortedByGameCount", players);
        }
    }
}
