using GameLogic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Json;
using Client.Utilities.Json;
using Newtonsoft.Json;

namespace Client.Services
{
    public class HttpClientCrudService : IHttpClientServiceImplementation
    {
        private static readonly HttpClient _httpClient = new HttpClient();

        public HttpClientCrudService()
        {
            _httpClient.BaseAddress = new Uri("");  // TODO: Add server's base URI
            _httpClient.Timeout = new TimeSpan(0, 0, 30);  // Set a timeout for the request
        }

        public async Task<Game> StartGame(int playerId)
        {
            var response = await _httpClient.PostAsJsonAsync($"{playerId}/start", new { });

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var game = JsonConvert.DeserializeObject<Game>(content, new GameConverter());
                return game;
            }
            else
            {
                // Handle error.
                throw new Exception("Failed to start the game.");
            }
        }
    }
}
