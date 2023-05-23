using FashionWeb.Domain.HostConfig;
using Microsoft.Extensions.Options;
using System.Net.Mime;
using System.Text;
using System.Text.Json;

namespace FashionWeb.Domain.Services.HttpClients
{
    public class HttpClientService : IHttpClientService
    {
        private readonly ApiConfig _hostAPIConfig;
        private readonly HttpClient _httpClient;
        public HttpClientService(IOptions<ApiConfig> options, HttpClient httpClient)
        {
            _hostAPIConfig = options.Value;
            _httpClient = httpClient;
        }
        public string GetBaseUrl()
        {
            return _hostAPIConfig.Url;
        }

        public string GetFileApiUrl(string fileName)
        {
            var fileUrl = GetBaseUrl() + "resource/" + fileName;
            return fileUrl;
        }

        public async Task<T> GetAsync<T>(string url)
        {
            var response = await _httpClient.GetAsync(url);
            if(response != null && response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<T>(jsonString);
            }

            return default;
        }

        public async Task<TResult> PostAsync<TBody, TResult>(TBody body, string url, string contentType = MediaTypeNames.Application.Json)
        {
            var jsonBody = JsonSerializer.Serialize(body);
            var response = await _httpClient.PostAsync(url, new StringContent(jsonBody, Encoding.UTF8, contentType));
            if (response != null && response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<TResult>(jsonString);
                return result;
            }

            return default;
        }
    }
}
