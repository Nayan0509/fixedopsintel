﻿using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WoodenAutomative.Domain.Dtos.Request.Login;
using WoodenAutomative.Domain.Models;
using WoodenAutomative.EntityFramework;
using WoodenAutomative.EntityFramework.Interfaces.Services;
using WoodenAutomative.EntityFramework.Repositories;

namespace WoodenAutomative.Controllers
{
    public class LoginController : Controller
    {

        private readonly WoodenAutomativeContext _context;
        private readonly ILoginService _loginService;
        private readonly IEmailRepository _emailRepository;
        private readonly INotyfService _notyf;

        public LoginController(ILoginService loginService,
            IEmailRepository emailRepository, 
                               WoodenAutomativeContext context,
                               INotyfService notyf)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _loginService = loginService ?? throw new ArgumentNullException(nameof(loginService));
            _notyf = notyf ?? throw new ArgumentNullException(nameof(notyf));
            _emailRepository= emailRepository ?? throw new ArgumentNullException(nameof(_emailRepository));
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

                    var regStatus = await _loginService.SignIn(this.HttpContext, loginRequest);
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
                    else if(regStatus == LoginStatus.MobileVerification)
                    {
                        return RedirectToAction("SendOTP", "Authorization");
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
                _loginService.SignOut(this.HttpContext);
                return RedirectToAction("Index", "Login");
            }
            catch (Exception ex)
            {
                Logger.Error(ex.ToString());
                throw;
            }
        }
    }
}
