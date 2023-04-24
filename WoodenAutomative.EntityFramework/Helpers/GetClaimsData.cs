using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using WoodenAutomative.Domain.Models;

namespace WoodenAutomative.EntityFramework.Helpers
{
    public class GetClaimsData
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GetClaimsData(UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(_httpContextAccessor));
        }

        public async Task<ApplicationUser> GetCurrentUserDetails()
        {
            var claimName = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Email);
            return await _userManager.FindByIdAsync(claimName.Value);
        }
    }
}
