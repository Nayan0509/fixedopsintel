using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WoodenAutomative.EntityFramework.Interfaces.Services;

namespace WoodenAutomative.EntityFramework.Services
{
    public class CurrentUserAccessor : ICurrentUserAccessor
    {
        #region Properties
        public string Role { get; }
        public string FirstName { get; }
        public string LastName { get; }

        #endregion

        #region Constants

        private const string ClaimTypeName = "name";

        #endregion

        #region Constructor

        public CurrentUserAccessor(IHttpContextAccessor httpContextAccessor)
        {
            if (httpContextAccessor == null)
                throw new ArgumentNullException(nameof(httpContextAccessor));

            if (httpContextAccessor.HttpContext != null)
            {
                var firstName = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Name);
                var lastName = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Surname);
                var role = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Role);

                FirstName = firstName?.Value;
                LastName = lastName?.Value;
                Role = role?.Value;
            }
        }

        #endregion
    }
}
