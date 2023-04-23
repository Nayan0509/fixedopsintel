using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WoodenAutomative.Domain.Dtos.Request.Login;
using WoodenAutomative.Domain.Dtos.Request.Password;
using WoodenAutomative.Domain.Models;

namespace WoodenAutomative.EntityFramework.Interfaces.Services
{
    public interface IAuthorizationRepository
    {
        /// <summary>
        /// Update profile
        /// </summary>
        /// <param name="userProfileRequest">Mofidy user profile request parameter</param>
        public Task<bool> SetPassword(SetPasswordRequest setPasswordRequest);
    }
}
