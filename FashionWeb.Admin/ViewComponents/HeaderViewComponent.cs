using FashionWeb.Domain.Model.Users;
using FashionWeb.Domain.Services.Users;
using FashionWeb.Utilities.GlobalHelpers;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FashionWeb.Admin.ViewComponents
{
    public class HeaderComponentViewModel
    {
        public string FullName { get; set; }
        public string Avatar { get; set; }
    }


    public class HeaderViewComponent : ViewComponent
    {
        private readonly IUserService _userService;
        public HeaderViewComponent(IUserService userService)
        {
            this._userService = userService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var viewModel = new HeaderComponentViewModel();
            if (UserClaimsPrincipal.Identity.IsAuthenticated)
            {
                var token = UserClaimsPrincipal.FindFirstValue(JwtClaimType.Token);
                var result = await _userService.GetUserProfileAsync(token);
                if (result.IsSuccess)
                {
                    var user = result.ToSuccessDataResult<UserProfile>().Data;
                    viewModel.FullName = $"{user.FirstName} {user.LastName}";
                    viewModel.Avatar = user.AvatarImage;
                }
            }

            return View(viewModel);
        }
    }
}
