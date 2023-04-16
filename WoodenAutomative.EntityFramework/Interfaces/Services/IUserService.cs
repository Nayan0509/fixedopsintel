using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WoodenAutomative.Domain.Dtos.Request.Login;
using WoodenAutomative.Domain.Dtos.Response.Login;

namespace WoodenAutomative.EntityFramework.Interfaces.Services
{
    public interface IUserService
    {
        /// <summary>
        /// Update profile
        /// </summary>
        /// <param name="userProfileRequest">Mofidy user profile request parameter</param>
        public Task<bool> ModifyUserProfile(UserProfileRequest userProfileRequest);

        /// <summary>
        /// Get Details of User
        /// </summary>
        /// <param name="id">Get Details of current login users request parameter</param>
        public Task<UserProfileResponse> GetDetailsOfLoginUser(string id);
    }
}
