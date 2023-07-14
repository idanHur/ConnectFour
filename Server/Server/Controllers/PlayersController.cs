using GameManager.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System;
using System.Linq;

namespace Server.Controllers
{
    public class PlayersController : Controller
    {
        private readonly Manager _manager;

        public PlayersController(Manager manager)
        {
            _manager = manager;
        }

        public IActionResult AllPlayers()
        {
            var players = _manager.GetAllPlayers();
            return View(players);
        }

        public IActionResult SortByName()
        {
            var players = _manager.GetAllPlayers()
                .OrderBy(p => p.playerName.ToLower())
                .ToList();

            return View(players);  // Use "Index" view to display sorted players
        }

        public IActionResult SortByNameAndGameDateDescending()
        {
            var players = _manager.GetAllPlayers()
                .Select(p => new
                {
                    Name = p.playerName,
                    LastGameDate = p.games.OrderByDescending(g => g.startTime).FirstOrDefault()?.startTime
                })
                .OrderByDescending(p => p.Name.ToLower())
                .ToList();

            return View(players);  // Use "Index" view to display sorted players
        }


        public IActionResult AllGames()
        {
            var games = _manager.GetAllPlayers()
                .SelectMany(p => p.games)
                .ToList();

            return View(games);
        }

        public IActionResult PlayerGameCounts()
        {
            var playerGameCounts = _manager.GetAllPlayers()
                .Select(p => new { PlayerName = p.playerName, GameCount = p.games.Count })
                .ToList();

            return View(playerGameCounts);
        }

        public IActionResult PlayersSortedByGameCount()
        {
            var players = _manager.GetAllPlayers()
                .OrderByDescending(p => p.games.Count)
                .ToList();

            return View(players);
        }
    }
}
