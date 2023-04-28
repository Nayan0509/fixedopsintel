using WoodenAutomative.Domain.Dtos.Request.ChangePassword;
using WoodenAutomative.Domain.Dtos.Request.Login;
using WoodenAutomative.Domain.Dtos.Response.Login;

namespace WoodenAutomative.EntityFramework.Interfaces.Services
{
    public interface IUserService
    {
        /// <summary>
        /// Update profile
        /// </summary>
        /// <param name="userProfileRequest">Modify user profile request parameter</param>
        public Task<bool> ModifyUserProfile(UserProfileRequest userProfileRequest);

        /// <summary>
        /// Get Details of User
        /// </summary>
        /// <param name="id">Get Details of current login users request parameter</param>
        public Task<UserProfileResponse> GetDetailsOfLoginUser(string id);

        /// <summary>
        /// Change password
        /// </summary>
        /// <param name="userId">Id of user</param>
        /// <param name="changePasswordRequest">Request param for users</param>
        public Task<bool> ChangePassword(string userId, ChangePasswordRequest changePasswordRequest);
    }
}
