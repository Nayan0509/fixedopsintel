
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
        //private readonly INotyfService _notyf;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UserProfile(IUserService userService/*, INotyfService notyf*/, IWebHostEnvironment hostEnvironment, IHttpContextAccessor httpContextAccessor)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            //_notyf = notyf ?? throw new ArgumentNullException(nameof(notyf));
            _hostEnvironment = hostEnvironment ?? throw new ArgumentNullException(nameof(hostEnvironment));
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var claimsIdentity = (ClaimsIdentity)_httpContextAccessor.HttpContext.User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.Role);
            // Find the "email" claim
            var claimName = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            var userProfileResponse = await _userService.GetDetailsOfLoginUser("121edb68-3579-48b4-b8d5-b01d8fcb8ce3"); //Here please replace static userid to login userid 

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
