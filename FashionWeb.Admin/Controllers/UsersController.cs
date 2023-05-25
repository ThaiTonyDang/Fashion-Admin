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
            if(response.IsSuccess)
            {
                var result = (Response<string>)response;
                var token = result.Data;

                var isValidToken = await this._jwtTokenService.ValidateToken(token);

                // read claim
                if(isValidToken)
                {
                    var claims = await this._jwtTokenService.GetClaims(token);
                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var authenticateionProp = new AuthenticationProperties
                    {
                        AllowRefresh = true,
                        IsPersistent = true,
                    };

                    await HttpContext.SignInAsync(new ClaimsPrincipal(claimsIdentity), authenticateionProp);

                    HttpContext.Session.SetString("JwtToken", token);

                    if (!string.IsNullOrEmpty(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }

                }
            }

            return RedirectToAction("Index", "Home");
        }


        [HttpPost]
        [Route("logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }
    }
}
