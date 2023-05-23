using FashionWeb.Domain.Dtos;
using FashionWeb.Domain.Services.HttpClients;

namespace FashionWeb.Domain.Services.Users
{
    public class UserService : IUserService
    {
        private readonly IHttpClientService _httpClientService;
        private readonly string _apiPathUrl = "api/users";
        private readonly string _apiUrl;

        public UserService(IHttpClientService httpClientService)
        {
            _httpClientService = httpClientService;
            _apiUrl = _httpClientService.GetBaseUrl() + $"/{_apiPathUrl}";
        }

        public async Task<string> LoginAsync(User user)
        {
            var loginApiUrl = $"{_apiUrl}/login";
            return string.Empty;
        }
    }
}
