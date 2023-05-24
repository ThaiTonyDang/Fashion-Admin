using FashionWeb.Domain.Dtos;
using FashionWeb.Domain.Model;

namespace FashionWeb.Domain.Services.Users
{
    public interface IUserService
    {
        Task<IResponse> LoginAsync(User user);
    }
}
