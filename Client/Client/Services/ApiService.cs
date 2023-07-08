﻿using Client.Utilities.Json;
using GameLogic.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Client.Services
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;
        private readonly AuthenticationService _authService;

        public ApiService(AuthenticationService authService)
        {
            _httpClient = new HttpClient { BaseAddress = new Uri("http://your-aspnetcore-api-url/") };
            _authService = authService;
        }

        public HttpClient GetClient()
        {
            return _httpClient;
        }
       

        public async Task<bool> LoginAsync(string playerId, string password)
        {
            var payload = new { PlayerId = playerId, Password = password };
            var jsonPayload = JsonConvert.SerializeObject(payload);
            var httpContent = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("api/login", httpContent);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Error authenticating");
            }

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeAnonymousType(jsonResponse, new { token = "", player = "" });

            var jwt = data.token;

            // Deserialize player JSON into Player object
            var player = JsonConvert.DeserializeObject<Player>(data.player, new PlayerConverter());
            _authService.SetCurrentPlayerFromServerData(player);

            // Save the JWT token
            _authService.SaveJwtToken(jwt);

            return true;
        }

        public async Task StartGameAsync()
        {
            Player player = _authService.GetCurrentPlayer();
            int playerId = player.playerId;

            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _authService.GetJwtToken());

            var response = await _httpClient.PostAsync($"{playerId}/start", null);
            var jsonResponse = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                var errorResponse = JsonConvert.DeserializeAnonymousType(jsonResponse, new { error = "" });
                throw new Exception("Error starting game: " + errorResponse.error);
            }
            var game = JsonConvert.DeserializeObject<Game>(jsonResponse);
            player.games.Add(game);
        }

        public async Task<Move> MakeMoveAsync(Move move)
        {
            int playerId = _authService.GetCurrentPlayer().playerId;


            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _authService.GetJwtToken());

            var jsonPayload = JsonConvert.SerializeObject(move);
            var httpContent = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync($"{playerId}/move", httpContent);
            var jsonResponse = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                var errorResponse = JsonConvert.DeserializeAnonymousType(jsonResponse, new { error = "" });
                throw new Exception("Error making move: " + errorResponse.error);
            }

            // Update the game state from server data
            var game = JsonConvert.DeserializeObject<Game>(jsonResponse);
            _authService.SetCurrentGameFromServerData(game); 

            var movesList = game.moves.ToList<Move>();

            return movesList[movesList.Count - 1];
        }

        public async Task EndGameAsync()
        {
            Player player = _authService.GetCurrentPlayer();
            int playerId = player.playerId;
            int gameId = player.games[player.games.Count - 1].gameId;

            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _authService.GetJwtToken());

            var jsonPayload = JsonConvert.SerializeObject(gameId);
            var httpContent = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync($"{playerId}/endGame", httpContent);
            var jsonResponse = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                var errorResponse = JsonConvert.DeserializeAnonymousType(jsonResponse, new { error = "" });
                throw new Exception("Error ending the game: " + errorResponse.error);
            }    
            // TODO: Update the gamestate
        }

        public async Task<Move> AiMoveAsync(int gameId)
        {
            int playerId = _authService.GetCurrentPlayer().playerId;

            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _authService.GetJwtToken());

            var jsonPayload = JsonConvert.SerializeObject(gameId);
            var httpContent = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync($"{playerId}/aiMove", httpContent);
            var jsonResponse = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                var errorResponse = JsonConvert.DeserializeAnonymousType(jsonResponse, new { error = "" });
                throw new Exception("Error making AI move: " + errorResponse.error);
            }

            // Update the game state from server data
            var gameState = JsonConvert.DeserializeObject<Game>(jsonResponse);
            _authService.SetCurrentGameFromServerData(gameState); // Update the game from server game state

            var movesList = gameState.moves.ToList<Move>();

            return movesList[movesList.Count - 1];
        }
    }
}
