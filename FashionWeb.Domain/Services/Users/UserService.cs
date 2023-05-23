using FashionWeb.Domain.Services.HttpClients;

namespace FashionWeb.Domain.Services.Users
{
    public class UserService : IUserService
    {
        private readonly IHttpClientService _httpClientService;
        private readonly string _apiUrl = "api/users";

        public UserService(IHttpClientService httpClientService)
        {
            _httpClientService = httpClientService;
        }

        public async Task<string> LoginAsync(string userName, string passWord)
        {
            var url = _httpClientService.GetBaseUrl() + $"/{_apiUrl}/login";
            return string.Empty;
        }
    }
}
