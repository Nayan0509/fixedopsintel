﻿using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WoodenAutomative.Domain.Dtos.Request;
using WoodenAutomative.Domain.Dtos.Request.Login;
using WoodenAutomative.Domain.Models;
using WoodenAutomative.EntityFramework.Interfaces.Services;

namespace WoodenAutomative.EntityFramework.Services
{

    public class LoginService : ILoginService
    {
        #region private Fields
        private readonly WoodenAutomativeContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        #endregion


        public LoginService(UserManager<ApplicationUser> userManager,
                            WoodenAutomativeContext context,
                            SignInManager<ApplicationUser> signInManager)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
        }

        public async Task<LoginStatus> SignIn(HttpContext httpContext, LoginRequest loginRequest)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == loginRequest.Email &&
                                                                    u.IsActive == true &&
                                                                    u.IsDeleted == false);
                var result = await _signInManager.CheckPasswordSignInAsync(user, loginRequest.Password, false);
                if (result.Succeeded)
                {
                    var roleNames = await _userManager.GetRolesAsync(user);
                    ClaimsIdentity identity = new ClaimsIdentity(
                                    this.GetUserClaims(user, roleNames.ToString(), ""),
                                    CookieAuthenticationDefaults.AuthenticationScheme
                                        );
                    ClaimsPrincipal principal = new ClaimsPrincipal(identity);
                    await httpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                    if(user.LastPasswordModifiedDate == null || user.LastPasswordModifiedDate.Value.AddDays(60)<=DateTime.Now)
                    {
                        return LoginStatus.SetNewPassword;
                    }
                    return LoginStatus.Succeeded;
                }
                return LoginStatus.Failed;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private IEnumerable<Claim> GetUserClaims(ApplicationUser user, string roles, string peSession)
        {
            List<Claim> claims = new List<Claim>();

            claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString() ?? "0"));
            claims.Add(new Claim(ClaimTypes.Email, user.Email ?? ""));
            claims.Add(new Claim(ClaimTypes.PostalCode, peSession));
            claims.Add(new Claim(ClaimTypes.Role, roles));
            return claims;
        }

    }
}
