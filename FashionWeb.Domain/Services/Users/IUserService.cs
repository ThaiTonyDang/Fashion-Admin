using FashionWeb.Domain.Dtos;
using FashionWeb.Domain.Model;

namespace FashionWeb.Domain.Services.Users
{
    public interface IUserService
    {
        Task<ResultDto> LoginAsync(User loginUser);
        Task<ResultDto> GetUserProfileAsync(string token);
    }
}
