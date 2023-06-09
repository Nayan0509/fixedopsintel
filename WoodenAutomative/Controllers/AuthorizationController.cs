﻿using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WoodenAutomative.Domain.Dtos.Request.Authorization;
using WoodenAutomative.Domain.Dtos.Request.OTP;
using WoodenAutomative.Domain.Dtos.Request.Password;
using WoodenAutomative.EntityFramework.Interfaces.Services;

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

        [HttpPost]
        public async Task<IActionResult> SavePassword(SetPasswordRequest setPasswordRequest)
        {
            var status = await _authorization.SetPassword(setPasswordRequest);
            if (status)
            {
                _notyf.Success("Password change Successfully");
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return View("SavePassword");
            }
        }

        public async Task<IActionResult> SetNewPassword()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.Role);
            var claimName = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            var user = await _userService.GetDetailsOfLoginUser(claimName.Value);
            if (user != null && user.LastPasswordModifiedDate == null)
            {
                return View();
            }
            return RedirectToAction("Index", "Home");
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

        [HttpPost]
        public async Task<IActionResult> SendOTP(AuthorizationTypeRequest authorizationTypeRequest)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.Role);
            var claimName = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            if (authorizationTypeRequest.AuthorizationType.Contains("Email"))
            {
                var status = await _emailRepository.SendEmailOTP(claimName.Value);
                if (status)
                {
                    TempData["authorizationType"] = "Email";
                    _notyf.Success("OTP send successfully !!");
                    return RedirectToAction("Verification");
                }
                _notyf.Error("Failed to send OTP !!");
                return View();
            }
            else if (authorizationTypeRequest.AuthorizationType.Contains("MobileNo"))
            {
                var status = await _emailRepository.SendMobileOTP(claimName.Value);
                if (status)
                {
                    TempData["authorizationType"] = "Mobile";
                    _notyf.Success("OTP send successfully !!");
                    return RedirectToAction("Verification");
                }
                _notyf.Error("Failed to send OTP !!");
                return View();
            }
            else
            {
                return View();
            }
        }

        public async Task<IActionResult> Verification()
        {
            var type = TempData["authorizationType"];
            if (type != null && Convert.ToString(type) == "Mobile")
            {
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var mobileno = claimsIdentity.FindFirst(ClaimTypes.MobilePhone).Value;
                TempData["Type"] = mobileno.Substring(0, 2) + new string('*', mobileno.Length - 4) + mobileno.Substring(mobileno.Length - 2, 2);
                TempData["Resendurl"] = "SendOTPonMobile";
                TempData["authorizationType"] = "Mobile";
            }
            else
            {
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var email = claimsIdentity.FindFirst(ClaimTypes.Email).Value;
                TempData["Type"] = email.Substring(0, 3) + new string('*', email.Length - 6) + email.Substring(email.Length - 3, 3);
                TempData["Resendurl"] = "SendOTPonEmail";
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> SendOTPonEmail()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.Role);
            var claimName = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            var status = await _emailRepository.SendEmailOTP(claimName.Value);
            if (status)
            {
                TempData["authorizationType"] = "Email";
                _notyf.Success("OTP send successfully !!");
                return RedirectToAction("Verification");
            }
            TempData["verificationError"]="Failed to send OTP !!";
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> SendOTPonMobile()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.Role);
            var claimName = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            var status = await _emailRepository.SendMobileOTP(claimName.Value);
            if (status)
            {
                _notyf.Success("OTP send successfully !!");
                return RedirectToAction("Verification");
            }
            _notyf.Error("Failed to send OTP !!");
            return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> VerifyOTP(OTPRequest oTPRequest)
        {
            if (oTPRequest != null)
            {
                string otpValue = string.Concat(oTPRequest.Digit1,
                                                oTPRequest.Digit2,
                                                oTPRequest.Digit3,
                                                oTPRequest.Digit4,
                                                oTPRequest.Digit5,
                                                oTPRequest.Digit6);

                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var claim = claimsIdentity.FindFirst(ClaimTypes.Role);
                var claimName = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
                var status = await _emailRepository.VerifyOTP(claimName.Value, otpValue);
                if (status)
                {
                    return RedirectToAction("SetNewPassword");
                }
                else {
                    TempData["verificationError"]="Please enter valid OTP !!";
                    return RedirectToAction("Verification");
                }
            }
            else
            {
                return View();
            }
        }


    }
}