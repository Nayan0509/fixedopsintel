using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace WoodenAutomative.EntityFramework.Helpers
{
    public class EmailVerificationMiddleware
    {
        private readonly RequestDelegate _next;

        public EmailVerificationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (!context.User.Identity.IsAuthenticated)
            {
                // If user is not authenticated, do not redirect
                await _next(context);
                return;
            }

            // Check if email is verified
            bool isEmailVerified = CheckEmailVerificationStatus(context);

            if (!isEmailVerified)
            {
                // Redirect to specified path
                context.Response.Redirect("/authorization/SelectAuthorizationType");
                return;
            }

            // Continue to next middleware
            await _next(context);
        }

        private bool CheckEmailVerificationStatus(HttpContext context)
        {
            var user=context.User;
            bool securityStamp = Convert.ToBoolean(user.FindFirstValue("IsEmailverify"));
            return securityStamp;
            // Check email verification status logic goes here
            // Return true if email is verified, false otherwise
        }
    }

}
