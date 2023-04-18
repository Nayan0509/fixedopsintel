using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WoodenAutomative.Domain.Dtos.Request.Login;
using WoodenAutomative.Domain.Dtos.Request.Password;
using WoodenAutomative.Domain.Models;
using WoodenAutomative.EntityFramework.Interfaces.Services;

namespace WoodenAutomative.EntityFramework.Services
{
    public class AuthorizationRepository: IAuthorizationRepository
    {
        #region private Fields
        private readonly WoodenAutomativeContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IPasswordHasher<ApplicationUser> _passwordHasher;
        #endregion


        public AuthorizationRepository(UserManager<ApplicationUser> userManager,
                            WoodenAutomativeContext context,
                            SignInManager<ApplicationUser> signInManager,
                            IPasswordHasher<ApplicationUser> passwordHasher
            , IHttpContextAccessor httpContextAccessor)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            _passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
        }

        public async Task<bool> SetPassword(SetPasswordRequest setPasswordRequest)
        {
            try
            {
                if (setPasswordRequest == null)
                    throw new ArgumentNullException(nameof(setPasswordRequest));

                var claimsIdentity = (ClaimsIdentity)_httpContextAccessor.HttpContext.User.Identity;
                var claim = claimsIdentity.FindFirst(ClaimTypes.Role);
                var claimName = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

                ApplicationUser user = await _context.Users.FindAsync(claimName.Value);
                var st = _userManager.ChangePasswordAsync(user,user.PasswordHash,setPasswordRequest.Password);
                var result = await _context.SaveChangesAsync();
                var status = result > 0 ? true : false;
                return status;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}
