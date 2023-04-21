using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WoodenAutomative.Domain.Dtos.Request.Login;
using WoodenAutomative.EntityFramework.Interfaces.Services;

namespace WoodenAutomative.Controllers
{
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUserService _userService;
        private readonly ILoginService _loginService;
        private readonly INotyfService _notyf;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HomeController(ILogger<HomeController> logger, 
            IUserService userService, 
            ILoginService loginService,
            INotyfService notyf, 
            IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _loginService = loginService ?? throw new ArgumentNullException(nameof(loginService));
            _notyf = notyf ?? throw new ArgumentNullException(nameof(notyf));
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> UpdateProfile()
        {
            var user = User as ClaimsPrincipal;
            var claimsIdentity = user.Identity as ClaimsIdentity;
            var claimName = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            var userProfileResponse = await _userService.GetDetailsOfLoginUser(claimName.Value);
            UserProfileRequest userProfileRequest = new UserProfileRequest()
            {
                Id = userProfileResponse.Id,
                FirstName = userProfileResponse.FirstName,
                LastName = userProfileResponse.LastName,
                Email = userProfileResponse.Email,
                PhoneNumber = userProfileResponse.PhoneNumber
            };

            return View(userProfileRequest);
        }

        [HttpPost]
        public async Task<ActionResult> UpdateProfile(UserProfileRequest userProfileRequest)
        {
            var status = await _userService.ModifyUserProfile(userProfileRequest);
            if (status == true)
                _notyf.Success("User profile is successfully updated");
            else
                _notyf.Warning("You have not changed anything. Please make sure and change user profile.");

            await _loginService.GetUpdatedUserClaims(this.HttpContext);
            return RedirectToAction("UpdateProfile", "Home");
        }
    }
}