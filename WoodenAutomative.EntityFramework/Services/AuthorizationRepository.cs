using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
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
        private readonly IConfiguration _configuration;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        #endregion


        public AuthorizationRepository(UserManager<ApplicationUser> userManager,
                            WoodenAutomativeContext context,
                            IConfiguration configuration,
                            SignInManager<ApplicationUser> signInManager
            , IHttpContextAccessor httpContextAccessor)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            _configuration=configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public async Task<bool> SetPassword(SetPasswordRequest setPasswordRequest)
        {
            var status = false;
            if (setPasswordRequest == null)
                    throw new ArgumentNullException(nameof(setPasswordRequest));

                var claimsIdentity = (ClaimsIdentity)_httpContextAccessor.HttpContext.User.Identity;
                var claim = claimsIdentity.FindFirst(ClaimTypes.Role);
                var claimName = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

               
                using (WoodenAutomativeContext db = new WoodenAutomativeContext(new DbContextOptionsBuilder<WoodenAutomativeContext>()
                                                .UseSqlServer(_configuration.GetConnectionString("WoodenAutomativeDbConString"))
                                                .Options))
                {
                    ApplicationUser user = await db.Users.FindAsync(claimName.Value);
                    user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, setPasswordRequest.Password);
                    var result = await db.SaveChangesAsync();
                    status = result > 0 ? true : false;
                }
                
                return status;
        }
    }
}
