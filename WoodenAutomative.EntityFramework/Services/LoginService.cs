using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Principal;
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
        private readonly IHttpContextAccessor _httpContextAccessor;
        #endregion


        public LoginService(UserManager<ApplicationUser> userManager,
                            WoodenAutomativeContext context,
                            SignInManager<ApplicationUser> signInManager,
                            IHttpContextAccessor httpContextAccessor)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }


        public async Task<LoginStatus> SignIn(HttpContext httpContext, LoginRequest loginRequest)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == loginRequest.Email &&
                                                                    u.IsActive == true &&
                                                                    u.IsDeleted == false);
                //var result = await _signInManager.CheckPasswordSignInAsync(user, loginRequest.Password, true);
                var result = _userManager.PasswordHasher.VerifyHashedPassword(user,user.PasswordHash, loginRequest.Password);
                if (result.ToString().Contains("Success"))
                {
                    var roleNames = await _userManager.GetRolesAsync(user);

                    ClaimsIdentity identity = new ClaimsIdentity(
                                    this.GetUserClaims(user, roleNames[0].ToString()),
                                    CookieAuthenticationDefaults.AuthenticationScheme);

                    ClaimsPrincipal principal = new ClaimsPrincipal(identity);
                    await httpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                    if (user.LastPasswordModifiedDate == null || user.LastPasswordModifiedDate.Value.AddDays(60) <= DateTime.Now)
                    {
                        return LoginStatus.SetNewPassword;
                    }
                    else if(!user.EmailConfirmed)
                    {
                        return LoginStatus.EmailVerification;
                    }
                    else if(!user.PhoneNumberConfirmed)
                    {
                        return LoginStatus.MobileVerification;
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

        public async void SignOut(HttpContext httpContext)
        {
            await httpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }
        public async Task<bool> GetUpdatedUserClaims(HttpContext httpContext)
        {
            var claimsIdentity = (ClaimsIdentity)_httpContextAccessor.HttpContext.User.Identity;
            var identity = _httpContextAccessor.HttpContext.User.Identities;
            var claimName = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == claimName.Value);

            // Add claims
            claimsIdentity.RemoveClaim(claimsIdentity.FindFirst(ClaimTypes.Name));
            claimsIdentity.RemoveClaim(claimsIdentity.FindFirst(ClaimTypes.Surname));
            claimsIdentity.RemoveClaim(claimsIdentity.FindFirst(ClaimTypes.Email));
            claimsIdentity.AddClaim(new Claim(ClaimTypes.Name, user.FirstName ?? ""));
            claimsIdentity.AddClaim(new Claim(ClaimTypes.Surname, user.LastName ?? ""));
            claimsIdentity.AddClaim(new Claim(ClaimTypes.Email, user.Email ?? ""));

            ClaimsPrincipal principal = new ClaimsPrincipal(identity);
            await httpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            return true;
        }

        private IEnumerable<Claim> GetUserClaims(ApplicationUser user, string roles)
        {
            List<Claim> claims = new List<Claim>();

            claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString() ?? "0"));
            claims.Add(new Claim(ClaimTypes.Name, user.FirstName ?? ""));
            claims.Add(new Claim(ClaimTypes.Surname, user.LastName ?? ""));
            claims.Add(new Claim(ClaimTypes.Email, user.Email ?? ""));
            claims.Add(new Claim(ClaimTypes.Uri, value: user.LastName ?? ""));
            claims.Add(new Claim(ClaimTypes.Role, roles));
            claims.Add(new Claim("SecurityStamp", user.SecurityStamp));
            claims.Add(new Claim("IsEmailverify",user.EmailConfirmed.ToString() ?? ""));
            claims.Add(new Claim("IsMobileVerify", user.PhoneNumberConfirmed.ToString() ?? ""));

            return claims;
        }

    }
}
