using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Client.Services
{
    public class HttpClientCrudService : IHttpClientServiceImplementation
    {
        private static readonly HttpClient _httpClient = new HttpClient();

        private readonly JsonSerializerOptions _options;

        public HttpClientCrudService()
        {
            _httpClient.BaseAddress = new Uri("");  // TODO: Add server's base URI
            _httpClient.Timeout = new TimeSpan(0, 0, 30);  // Set a timeout for the request

            _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }

      

    }

}
