using FashionWeb.Domain.Dtos;
using FashionWeb.Domain.Model;
using FashionWeb.Domain.Model.Users;
using FashionWeb.Domain.ResponseModel;
using FashionWeb.Domain.Services.HttpClients;
using FashionWeb.Domain.Services.Jwts;
using Microsoft.AspNetCore.Http;
using static System.Net.WebRequestMethods;

namespace FashionWeb.Domain.Services.Users
{
    public class UserService : IUserService
    {
        private readonly IHttpClientService _httpClientService;
        private readonly IJwtTokenService _jwtTokenService;
        private const string _apiResource = "users";

        public UserService(IHttpClientService httpClientService, IJwtTokenService jwtTokenService)
        {
            _httpClientService = httpClientService;
            _jwtTokenService = jwtTokenService;
        }

        public async Task<ResultDto> LoginAsync(User loginUser)
        {
            var apiUrl = $"{_apiResource}/login";
            var response = await _httpClientService.PostDataAsync<User, string>(loginUser, apiUrl);
            if (response.IsSuccess)
            {
                var result = response.ToSuccessDataResult<string>();
                var token = result.Data;
                var isVerify = await _jwtTokenService.ValidateToken(token);
                if (!isVerify)
                {
                    return new ErrorResult("Token is invalid");
                }
            }

            return response;
        }

        public async Task<ResultDto> GetUserProfileAsync(string token)
        {
            var apiUrl = $"{_apiResource}/profile";
            var response = await _httpClientService.GetDataAsync<UserProfile>(apiUrl, token);
            if (response.IsSuccess)
            {
                var user = response.ToSuccessDataResult<UserProfile>().Data;
                user.AvatarImage = _httpClientService.GetFileApiUrl(user.AvatarImage);
                return new SuccessDataResult<UserProfile>(response.Message, user);
            }

            return response;
        }
    }
}
