using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WoodenAutomative.Domain.Dtos.Request.Login;
using WoodenAutomative.EntityFramework.Interfaces.Services;
using WoodenAutomative.EntityFramework.Services;

namespace WoodenAutomative.Components
{
    public class User : ViewComponent
    {
        private readonly IDistributorService _distributorService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public User(IDistributorService distributorService, IHttpContextAccessor httpContextAccessor)
        {
            _distributorService = distributorService ?? throw new ArgumentNullException(nameof(distributorService));
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            ViewBag.territoryList = await _distributorService.GetAllTerritory();
            return View("Default");
        }
    }
}
