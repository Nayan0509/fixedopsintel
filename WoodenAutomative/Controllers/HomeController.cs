using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
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
        private readonly INotyfService _notyf;

        public HomeController(ILogger<HomeController> logger, IUserService userService,INotyfService notyf)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _notyf = notyf ?? throw new ArgumentNullException(nameof(notyf));
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> UpdateProfile(UserProfileRequest userProfileRequest)
        {
            var status = await _userService.ModifyUserProfile(userProfileRequest);

            if (status == true)
                _notyf.Success("User profile is successfully updated !!");
            else
                _notyf.Warning("User profile is not updated !!");
            return RedirectToAction("Index", "Home");
        }
    }
}