using FashionWeb.Domain.Dtos;
using FashionWeb.Domain.Model;
using FashionWeb.Domain.ResponseModel;
using FashionWeb.Domain.Services.HttpClients;
using Microsoft.AspNetCore.Http;

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

        public async Task<IResponse> LoginAsync(User user)
        {
            var loginApiUrl = $"{_apiUrl}/login";
            var response = await this._httpClientService.PostAsync<User, string>(user, loginApiUrl);
            if (response.IsSuccess)
            {
                var result = (ResponseApiData<string>)response;
                return new Response<string>
                {
                    IsSuccess = result.IsSuccess,
                    Data = result.Data,
                    Message = result.Message
                };
            }

            var errorResult = (ErrorResponseApi<string[]>)response;

            return new Response<string[]>
            {
                IsSuccess = errorResult.IsSuccess,
                Message = errorResult.Message,
                Data = errorResult.Errors
            };
        }
    }
}
