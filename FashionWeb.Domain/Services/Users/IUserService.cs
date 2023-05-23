using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FashionWeb.Domain.Services.Users
{
    public interface IUserService
    {
        Task<string> LoginAsync(string userName, string passWord);
    }
}
