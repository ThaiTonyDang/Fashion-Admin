using FashionWeb.Admin.ViewModels.Users;
using FashionWeb.Domain.Dtos;
using FashionWeb.Domain.Model;
using FashionWeb.Domain.Services.Jwts;
using FashionWeb.Domain.Services.Users;
using FashionWeb.Utilities.GlobalHelpers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FashionWeb.Admin.Controllers
{
    [AllowAnonymous]
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

                    if (!string.IsNullOrEmpty(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }

                    return RedirectToAction("Index", "Home");
                }
            }

            var errors = response.Message;
            if (!errors.Contains("Access Denied"))
            {
                TempData[TEMPDATA.LABEL_WARNING] = errors;
            }
            else
            {
                TempData[TEMPDATA.ACCESS_DENIED] = errors;
            }

            return RedirectToAction("Login", "Users");
        }

        [HttpPost]
        [Route("logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Route("access-denied")]
        public async Task<IActionResult> AccessDenied()
        {         
            return await Task.FromResult(View());
        }
    }
}
