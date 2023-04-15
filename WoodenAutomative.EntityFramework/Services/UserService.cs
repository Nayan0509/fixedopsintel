using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WoodenAutomative.Domain.Dtos.Request.Login;
using WoodenAutomative.Domain.Dtos.Response.Login;
using WoodenAutomative.Domain.Models;
using WoodenAutomative.EntityFramework.Interfaces.Services;

namespace WoodenAutomative.EntityFramework.Services
{
    public class UserService : IUserService
    {
        #region
        private readonly WoodenAutomativeContext _context;
        #endregion

        #region Constructor
        public UserService(WoodenAutomativeContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
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
            ApplicationUser user = await _context.Users.FindAsync(id);

            return new UserProfileResponse()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                Email = user.Email
            };
        }
        #endregion
    }
}
