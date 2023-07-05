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
        private readonly Player _player;

        public ApiService(AuthenticationService authService, Player player)
        {
            _httpClient = new HttpClient { BaseAddress = new Uri("http://your-aspnetcore-api-url/") };
            _authService = authService;
            _player = player;
        }

        public HttpClient GetClient()
        {
            return _httpClient;
        }

        public async Task<string> LoginAsync(string playerId, string password)
        {
            var payload = new { PlayerId = playerId, Password = password };
            var jsonPayload = JsonConvert.SerializeObject(payload);
            var httpContent = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("api/login", httpContent);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Error authenticating");
            }

            var jwt = await response.Content.ReadAsStringAsync();

            // Save the JWT token
            _authService.SaveJwtToken(jwt);

            return jwt;
        }



    }
}
