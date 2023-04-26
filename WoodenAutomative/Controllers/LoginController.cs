using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using WoodenAutomative.Domain.Dtos.Request.Login;
using WoodenAutomative.Domain.Dtos.Request.OTP;
using WoodenAutomative.Domain.Dtos.Request.Password;
using WoodenAutomative.Domain.Models;
using WoodenAutomative.EntityFramework;
using WoodenAutomative.EntityFramework.Interfaces.Services;

namespace WoodenAutomative.Controllers
{
    public class LoginController : Controller
    {
        private readonly INotyfService _notyf;
        private readonly IUnitOfWork _unitOfWork;

        public LoginController(INotyfService notyf,
                               IUnitOfWork unitOfWork)
        {
            _notyf = notyf ?? throw new ArgumentNullException(nameof(notyf));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<IActionResult> Index()
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
        public async Task<IActionResult> Index(LoginRequest loginRequest)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(loginRequest.Email) && !string.IsNullOrWhiteSpace(loginRequest.Password))
                {

                    var regStatus = await _unitOfWork.Login.SignIn(this.HttpContext, loginRequest);
                    if (regStatus == LoginStatus.Failed)
                    {
                        _notyf.Warning("Please enter valid Username / Password. !!");
                    }
                    else if(regStatus == LoginStatus.SetNewPassword)
                    {
                        return RedirectToAction("SetNewPassword", "Authorization");
                    }
                    else if(regStatus == LoginStatus.EmailVerification)
                    {
                        return RedirectToAction("SendOTPonEmail", "Authorization");
                    }
                    else if(regStatus == LoginStatus.SelectAuthorizationType)
                    {
                        return RedirectToAction("SelectAuthorizationType", "Authorization");
                    }
                    else
                    {
                        return RedirectToAction("Index","Home");
                    }
                    return RedirectToAction("Index", "Login");
                }
                return View();
            }
            catch (Exception ex)
            {
                Logger.Error(ex.ToString());
                throw;
            }
        }

        public IActionResult LogOut()
        {
            try
            {
                _unitOfWork.Login.SignOut(this.HttpContext);
                return RedirectToAction("Index", "Login");
            }
            catch (Exception ex)
            {
                Logger.Error(ex.ToString());
                throw;
            }
        }

        public async Task<IActionResult> ForgotPassword()
        {
            return View();
        }
        
        [HttpGet]
        public async Task<IActionResult> ReSendOTP(string email)
        {
            var status = await _unitOfWork.Email.SendEmailOTPForForgotpassword(email);
            if(status)
            {
                ViewBag.Email = email;
                _notyf.Success("OTP Send successfully !!");
                return View("Verification");
            }
            _notyf.Error("OTP Send Failed !!");
            return View("Verification");
        }

        [HttpPost]
        public async Task<IActionResult> Verification(ForgotPassword forgotPassword)
        {
            ViewBag.Email = forgotPassword.EmailAddress;
            var status =await _unitOfWork.Email.SendEmailOTPForForgotpassword(forgotPassword.EmailAddress);
            if(status)
            {
                return View();
            }
            _notyf.Error("Please enter valid Email !!");
            return View("ForgotPassword");
        }

        [HttpPost]
        public async Task<IActionResult> VerifyOTP(ForgotPasswordOTPRequest oTPRequest)
        {
            if (oTPRequest != null)
            {
                string otpValue = string.Concat(oTPRequest.Digit1,
                                                oTPRequest.Digit2,
                                                oTPRequest.Digit3,
                                                oTPRequest.Digit4,
                                                oTPRequest.Digit5,
                                                oTPRequest.Digit6);

                var status = await _unitOfWork.Email.VerifyOTPForforgotpassword(oTPRequest.Email, otpValue);
                if (status)
                {
                    ViewBag.Email = oTPRequest.Email;
                    return RedirectToAction("SetPassword", new { email = oTPRequest.Email });
                }
                else
                {
                    ViewBag.Email = oTPRequest.Email;
                    _notyf.Error("Please enter valid OTP !!");
                    return View("Verification");
                }
            }
            else
            {
                return View();
            }
        }

        public IActionResult SetPassword(string email)
        {
            ViewBag.Email = email;
            return View() ;
        }

        [HttpPost]
        public async Task<IActionResult> SetPassword(SetForgotPasswordRequest setPasswordRequest)
        {
            var status =await _unitOfWork.Login.SetPassword(setPasswordRequest);
            if (status)
            {
                _notyf.Success("Password change Successfully");
                return RedirectToAction("Index","Home");
            }
            else
            {
                _notyf.Error("Password not changed");
                return View("SetPassword");
            }
        }
    }
}
