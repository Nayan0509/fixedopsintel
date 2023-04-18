
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Security.Claims;
using WoodenAutomative.Domain.Dtos.Request.Login;
using WoodenAutomative.Domain.Dtos.Response.Login;
using WoodenAutomative.EntityFramework.Interfaces.Services;

namespace WoodenAutomative.Components
{
    public class UserProfile : ViewComponent
    {
        private readonly IUserService _userService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserProfile(IUserService userService, IHttpContextAccessor httpContextAccessor)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var claimsIdentity = (ClaimsIdentity)_httpContextAccessor.HttpContext.User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.Role);
            var claimName = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            var userProfileResponse = await _userService.GetDetailsOfLoginUser(claimName.Value);
            UserProfileRequest userProfileRequest = new UserProfileRequest()
            {
                Id = userProfileResponse.Id,
                FirstName = userProfileResponse.FirstName,
                LastName= userProfileResponse.LastName,
                Email= userProfileResponse.Email,
                PhoneNumber = userProfileResponse.PhoneNumber
            };
            return View("Default", userProfileRequest);
        }

    }
}
