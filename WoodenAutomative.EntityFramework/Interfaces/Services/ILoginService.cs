using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WoodenAutomative.Domain.Dtos.Request.Login;
using WoodenAutomative.Domain.Dtos.Response.Login;
using WoodenAutomative.Domain.Models;

namespace WoodenAutomative.EntityFramework.Interfaces.Services
{
    public interface ILoginService
    {
        /// <summary>
        /// Sign-In
        /// </summary>
        /// <param name="httpContext">HttpContext</param>
        /// <param name="loginRequest">Login request parameter</param>
        public Task<LoginStatus> SignIn(HttpContext httpContext, LoginRequest loginRequest);

        /// <summary>
        /// Sign-Out
        /// </summary>
        /// <param name="httpContext">HttpContext</param>
        public void SignOut(HttpContext httpContext);

        /// <summary>
        /// Get details of updated current users details
        /// </summary>
        /// <param name="httpContext">HttpContext</param>
        public Task<bool> GetUpdatedUserClaims(HttpContext httpContext);
    }
}
