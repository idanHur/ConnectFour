using Client.Utilities.Json;
using GameLogic.Models;
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

        public ApiService(AuthenticationService authService)
        {
            _httpClient = new HttpClient { BaseAddress = new Uri("https://localhost:5001/") };
            _authService = authService;
        }

        public HttpClient GetClient()
        {
            return _httpClient;
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
            System.Diagnostics.Debug.WriteLine(_authService.GetJwtToken()); // Add this line to log the response
            System.Diagnostics.Debug.WriteLine(playerId); // Add this line to log the response

            var response = await _httpClient.PostAsync($"api/{playerId}/start", null);

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var game = JsonConvert.DeserializeObject<Game>(jsonResponse);
                player.games.Add(game);
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
            int playerId = _authService.GetCurrentPlayer().playerId;


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

            var response = await _httpClient.PutAsync($"api/{playerId}/endGame", httpContent);
            var jsonResponse = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                var errorResponse = JsonConvert.DeserializeAnonymousType(jsonResponse, new { error = "" });
                throw new Exception("Error ending the game: " + errorResponse.error);
            }    
            // TODO: Update the gamestate
        }

        public async Task<Move> AiMoveAsync()
        {
            Player player = _authService.GetCurrentPlayer();
            int playerId = player.playerId;
            int gameId = player.games[player.games.Count - 1].gameId;

            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _authService.GetJwtToken());

            var jsonPayload = JsonConvert.SerializeObject(gameId);
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
            _authService.SetCurrentGameFromServerData(gameState); // Update the game from server game state

            var movesList = gameState.moves.ToList<Move>();

            return movesList[movesList.Count - 1];
        }
    }
}
