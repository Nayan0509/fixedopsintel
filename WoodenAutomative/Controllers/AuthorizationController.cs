using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WoodenAutomative.EntityFramework.Interfaces.Services;
using WoodenAutomative.EntityFramework;
using WoodenAutomative.EntityFramework.Services;
using WoodenAutomative.Domain.Dtos.Request.Password;
using WoodenAutomative.Domain.Dtos.Request.Authorization;
using System.Security.Claims;

namespace WoodenAutomative.Controllers
{
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public class AuthorizationController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUserService _userService;
        private readonly INotyfService _notyf;
        private readonly IAuthorizationRepository _authorization;
        private readonly IEmailRepository _emailRepository;

        public AuthorizationController(ILogger<HomeController> logger, IEmailRepository emailRepository, IUserService userService, INotyfService notyf, IAuthorizationRepository authorization)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _notyf = notyf ?? throw new ArgumentNullException(nameof(notyf));
            _authorization = authorization ?? throw new ArgumentNullException(nameof(authorization));
            _emailRepository = emailRepository ?? throw new ArgumentNullException(nameof(emailRepository));
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
                ViewData["ErrorMsg"] = null;
                return View();
        }

        [HttpPost]
        public async Task<IActionResult> SavePassword(SetPasswordRequest setPasswordRequest)
        {
                var status=await _authorization.SetPassword(setPasswordRequest);  
                if(status)
                {
                    _notyf.Success("Password change Successfully");
                    return RedirectToAction("SelectAuthorizationType");
                }
                else
                {
                    return View("SavePassword");
                }
        }
        
        [HttpPost]
        public async Task<IActionResult> SendOTP(AuthorizationTypeRequest authorizationTypeRequest)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.Role);
            var claimName = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            if (authorizationTypeRequest.AuthorizationType.Contains("Email"))
            {
                var status =await _emailRepository.SendEmailOTP(claimName.Value);
                if(status)
                {
                    return RedirectToAction("Verification");
                }
                return View();
            }
            else if(authorizationTypeRequest.AuthorizationType.Contains("MobileNo"))
            {
                return View();
            }
            else
            {
                return View();
            }
        }
        
        [HttpGet]
        public async Task<IActionResult> SendOTPonEmail()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.Role);
            var claimName = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
                var status =await _emailRepository.SendEmailOTP(claimName.Value);
                if(status)
                {
                    return RedirectToAction("Verification");
                }
                return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> VerifyOTP(string otpValue)
        {
            if (otpValue != null)
            {
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var claim = claimsIdentity.FindFirst(ClaimTypes.Role);
                var claimName = claimsIdentity.FindFirst(ClaimTypes.Email);

                var status = await _emailRepository.VerifyOTP(claimName.Value, otpValue);
                if (status)
                {
                    //redirect();
                    //return View();
                    return Redirect("/Home/Index");
                }
                else { return View(); }
            }
            else
            {
                return View();
            }
        }

        public async Task<IActionResult> redirect()
        {
            return RedirectToAction("UpdateProfile", "Home");
        }

    }
}