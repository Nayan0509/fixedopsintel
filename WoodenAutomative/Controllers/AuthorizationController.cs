using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WoodenAutomative.EntityFramework.Interfaces.Services;
using WoodenAutomative.EntityFramework;
using WoodenAutomative.EntityFramework.Services;
using WoodenAutomative.Domain.Dtos.Request.Password;

namespace WoodenAutomative.Controllers
{
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public class AuthorizationController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUserService _userService;
        private readonly INotyfService _notyf;
        private readonly IAuthorizationRepository _authorization;

        public AuthorizationController(ILogger<HomeController> logger, IUserService userService, INotyfService notyf, IAuthorizationRepository authorization)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _notyf = notyf ?? throw new ArgumentNullException(nameof(notyf));
            _authorization = authorization ?? throw new ArgumentNullException(nameof(_authorization));
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> SetNewPassword()
        {
            try
            {
                ViewData["ErrorMsg"] = null;
                return View();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        
        public async Task<IActionResult> SelectAuthorizationType()
        {
            try
            {
                ViewData["ErrorMsg"] = null;
                return View();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        
        public async Task<IActionResult> Verification()
        {
            try
            {
                ViewData["ErrorMsg"] = null;
                return View();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> SavePassword(SetPasswordRequest setPasswordRequest)
        {
            try
            {
                var status=_authorization.SetPassword(setPasswordRequest);  
                ViewData["ErrorMsg"] = null;
                return View();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

    }
}