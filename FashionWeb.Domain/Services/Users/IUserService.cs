using FashionWeb.Domain.Dtos;

namespace FashionWeb.Domain.Services.Users
{
    public interface IUserService
    {
        Task<string> LoginAsync(User user);
    }
}
