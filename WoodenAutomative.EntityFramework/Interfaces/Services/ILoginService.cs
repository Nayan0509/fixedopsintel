using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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

    }
}
