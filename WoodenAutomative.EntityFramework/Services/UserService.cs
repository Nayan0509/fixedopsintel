using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using WoodenAutomative.Domain.Dtos.Request.ChangePassword;
using WoodenAutomative.Domain.Dtos.Request.Login;
using WoodenAutomative.Domain.Dtos.Response.Login;
using WoodenAutomative.Domain.Models;
using WoodenAutomative.EntityFramework.Interfaces.Services;

namespace WoodenAutomative.EntityFramework.Services
{
    public class UserService : IUserService
    {
        #region Fields
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;
        private readonly WoodenAutomativeContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        #endregion

        #region Constructor
        public UserService(UserManager<ApplicationUser> userManager, 
            WoodenAutomativeContext context, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }
        #endregion

        #region Implemente Interface
        public async Task<bool> ModifyUserProfile(UserProfileRequest userProfileRequest)
        {
            if (userProfileRequest == null)
                throw new ArgumentNullException(nameof(userProfileRequest));

            ApplicationUser user = await _context.Users.FindAsync(userProfileRequest.Id);

            user.FirstName = userProfileRequest.FirstName;
            user.LastName = userProfileRequest.LastName;
            user.PhoneNumber = userProfileRequest.PhoneNumber;
            user.Email = userProfileRequest.Email;
            var result = await _context.SaveChangesAsync();
            var status = result > 0 ? true : false;
            return status;
        }

        public async Task<UserProfileResponse> GetDetailsOfLoginUser(string id)
        {
            var user = _context.Users.Find(id);

            return new UserProfileResponse()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                Email = user.Email,
                LastPasswordModifiedDate= user.LastPasswordModifiedDate,
            };
        }
       
        public async Task<bool> ChangePassword(string userId, ChangePasswordRequest changePasswordRequest)
        {
            var status = false;
            if (changePasswordRequest == null)
                throw new ArgumentNullException(nameof(changePasswordRequest));

            using (WoodenAutomativeContext db = new WoodenAutomativeContext(new Microsoft.EntityFrameworkCore.DbContextOptionsBuilder<WoodenAutomativeContext>()
                                                .UseSqlServer(_configuration.GetConnectionString("WoodenAutomativeDbConString"))
                                                .Options))
            {
                ApplicationUser user = await db.Users.FindAsync(userId);
                if (user == null)
                    return false;

                var isPasswordValid = _userManager.PasswordHasher.VerifyHashedPassword(user, user.PasswordHash, changePasswordRequest.CurrentPassword);
                if (isPasswordValid != PasswordVerificationResult.Success)
                    return false;

                // added check to make sure the new password is different from the current password
                if (string.Equals(changePasswordRequest.NewPassword, changePasswordRequest.CurrentPassword))
                    return false;

                user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, changePasswordRequest.NewPassword);
                user.SecurityStamp = Guid.NewGuid().ToString();
                var result = await db.SaveChangesAsync();
                status = result > 0 ? true : false;
            }
            return status;
        }

        #endregion
    }
}
