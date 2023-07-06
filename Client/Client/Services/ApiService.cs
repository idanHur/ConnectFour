using Client.Utilities.Json;
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



    }
}
