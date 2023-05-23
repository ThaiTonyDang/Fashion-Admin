using FashionWeb.Admin.ViewModels.Users;
using FashionWeb.Domain.Dtos;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FashionWeb.Admin.Controllers
{
    [Route("[controller]")]
    public class UsersController : Controller
    {
        [HttpGet]
        [Route("login")]
        public IActionResult Index()
        {
            return View("Login");
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromForm] UserLogin userLogin)
        {
            var user = new User
            {
                Email = userLogin.Email,
                Password = userLogin.Password,
            };

            return Ok();
        }
    }
}
