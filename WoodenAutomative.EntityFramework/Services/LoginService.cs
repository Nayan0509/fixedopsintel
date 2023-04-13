using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WoodenAutomative.Domain.Dtos.Request.Login;
using WoodenAutomative.Domain.Models;
using WoodenAutomative.EntityFramework.Interfaces.Services;

namespace WoodenAutomative.EntityFramework.Services
{

    public class LoginService : ILoginService
    {
        #region private Fields
        private readonly WoodenAutomativeContext _context;
        #endregion
        public LoginService(WoodenAutomativeContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public async Task<int> SignIn(HttpContext httpContext, LoginRequest loginRequest)
        {
            var compRegResult = await _context.Users.Where(d => (d.Email == loginRequest.Email &&
                                                                          d.Password == loginRequest.Password &&
                                                                          d.IsActive == true &&
                                                                          d.IsDeleted == false))
                                                                   .FirstOrDefaultAsync();

            if (compRegResult != null)
            {


                ClaimsIdentity identity = new ClaimsIdentity(
                                this.GetUserClaims(compRegResult, "NoLicense", ""),
                                CookieAuthenticationDefaults.AuthenticationScheme
                                    );
                ClaimsPrincipal principal = new ClaimsPrincipal(identity);
                await httpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
            }
            throw new NotImplementedException();
        }

        private IEnumerable<Claim> GetUserClaims(Users user, string roles, string peSession)
        {
            List<Claim> claims = new List<Claim>();

            claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString() ?? "0"));
            claims.Add(new Claim(ClaimTypes.Name, user.Email ?? ""));
            claims.Add(new Claim(ClaimTypes.Email, user.MobileNo ?? ""));
            claims.Add(new Claim(ClaimTypes.PostalCode, peSession));
            claims.Add(new Claim(ClaimTypes.Role, roles));
            return claims;
        }
    }
}
