using Microsoft.AspNetCore.Mvc;

namespace FashionWeb.Admin.Controllers
{
    [Route("[controller]")]
    public class UsersController : Controller
    {
        [HttpGet("login")]
        public IActionResult Index()
        {
            return View("Login");
        }
    }
}
