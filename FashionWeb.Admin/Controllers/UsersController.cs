using FashionWeb.Admin.ViewModels.Users;
using FashionWeb.Domain.Dtos;
using FashionWeb.Domain.Model;
using FashionWeb.Domain.Services.Jwts;
using FashionWeb.Domain.Services.Users;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FashionWeb.Admin.Controllers
{
    [Route("[controller]")]
    public class UsersController : Controller
    {
        private readonly IUserService _userService;
        private readonly IJwtTokenService _jwtTokenService;

        public UsersController(IUserService userService, IJwtTokenService jwtTokenService)
        {
            _userService = userService;
            _jwtTokenService = jwtTokenService;
        }

        [HttpGet]
        [Route("login")]
        public IActionResult Index([FromQuery] string returnUrl)
        {
            if(User.Identity.IsAuthenticated)
            {
                if (!string.IsNullOrEmpty(returnUrl))
                {
                    return Redirect(returnUrl);
                }

                return RedirectToAction("Index", "Home");
            }

            return View("Login");
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromForm] UserLogin userLogin, [FromQuery] string returnUrl)
        {
            var user = new User
            {
                Email = userLogin.Email,
                Password = userLogin.Password,
            };

            var response = await this._userService.LoginAsync(user);
            if (response.IsSuccess)
            {
                var resultData = response.ToSuccessDataResult<string>();
                var token = resultData.Data;
                var claims = await this._jwtTokenService.GetClaims(token);
                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, new AuthenticationProperties()
                {
                    AllowRefresh = true,
                    IsPersistent = true,
                });

                if (!string.IsNullOrEmpty(returnUrl))
                {
                    return Redirect(returnUrl);
                }
            }

            return RedirectToAction("Index", "Home");
        }


        [HttpPost]
        [Route("logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index");
        }
    }
}
