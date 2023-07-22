using Client.Utilities.Json;
using GameLogicClient.Models;
using GameLogicClient.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Client.Services
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;
        private readonly AuthenticationService _authService;
        private readonly GameDatabaseService _dbService;

        public ApiService(AuthenticationService authService, GameDatabaseService dbService)
        {

            _httpClient = new HttpClient { BaseAddress = new Uri("https://localhost:56751/") };
            _authService = authService;
            _dbService = dbService;
        }

        public async Task<bool> LoginAsync(int playerId, string password)
        {
            var payload = new { PlayerId = playerId, Password = password };
            var jsonPayload = JsonConvert.SerializeObject(payload);
            var httpContent = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("api/Auth/login", httpContent);
            var jsonResponse = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                var errorResponse = JsonConvert.DeserializeAnonymousType(jsonResponse, new { error = "" });
                throw new Exception("Error in Login: " + errorResponse);
            }

            var data = JsonConvert.DeserializeAnonymousType(jsonResponse, new { token = "", player = "" });

            var jwt = data.token;

            // Deserialize player JSON into Player object
            var player = JsonConvert.DeserializeObject<Player>(data.player, new PlayerConverter());
            _dbService.AddPlayer(player);
            _authService.currentPlayerId = playerId;
            // Save the JWT token
            _authService.SaveJwtToken(jwt);

            return true;
        }

        public async Task StartGameAsync()
        {
            int playerId = _authService.currentPlayerId;
            Player player = _dbService.GetPlayer(playerId);

            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _authService.GetJwtToken());

            var response = await _httpClient.PostAsync($"api/{playerId}/start", null);

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var game = JsonConvert.DeserializeObject<Game>(jsonResponse);
                _dbService.AddGameToPlayer(playerId, game);
            }
            else
            {
                var errorResponse = await response.Content.ReadAsStringAsync();
                System.Diagnostics.Debug.WriteLine(errorResponse);
                throw new Exception("Error starting game: HTTP status " + response.StatusCode);
            }
        }

        public async Task<Move> MakeMoveAsync(int colMove)
        {
            int playerId = _authService.currentPlayerId;

            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _authService.GetJwtToken());

            var jsonPayload = JsonConvert.SerializeObject(colMove);
            var httpContent = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync($"api/{playerId}/move", httpContent);
            var jsonResponse = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                var errorResponse = JsonConvert.DeserializeAnonymousType(jsonResponse, new { error = "" });
                throw new Exception("Error making move: " + errorResponse.error);
            }

            // Update the game state from server data
            var gameState = JsonConvert.DeserializeObject<Game>(jsonResponse);
            _dbService.UpdateGame(gameState);

            var movesList = gameState.Moves.ToList<Move>();

            return movesList[movesList.Count - 1];
        }

        public async Task EndGameAsync()
        {
            int playerId = _authService.currentPlayerId;
            Game game = _dbService.GetLastGameOfPlayer(playerId);

            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _authService.GetJwtToken());

            var jsonPayload = JsonConvert.SerializeObject(game.GameId);
            var httpContent = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync($"api/{playerId}/endGame", httpContent);
            var jsonResponse = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                var errorResponse = JsonConvert.DeserializeAnonymousType(jsonResponse, new { error = "" });
                throw new Exception("Error ending the game: " + errorResponse.error);
            }
            var gameState = JsonConvert.DeserializeObject<Game>(jsonResponse);
            _dbService.UpdateGame(gameState);
        }

        public async Task<Move> AiMoveAsync()
        {
            int playerId = _authService.currentPlayerId;
            Game game = _dbService.GetLastGameOfPlayer(playerId);

            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _authService.GetJwtToken());

            var jsonPayload = JsonConvert.SerializeObject(game.GameId);
            var httpContent = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync($"api/{playerId}/aiMove", httpContent);
            var jsonResponse = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                var errorResponse = JsonConvert.DeserializeAnonymousType(jsonResponse, new { error = "" });
                throw new Exception("Error making AI move: " + errorResponse.error);
            }

            // Update the game state from server data
            var gameState = JsonConvert.DeserializeObject<Game>(jsonResponse);
            _dbService.UpdateGame(gameState);

            var movesList = gameState.Moves.ToList<Move>();

            return movesList[movesList.Count - 1];
        }
    }
}
