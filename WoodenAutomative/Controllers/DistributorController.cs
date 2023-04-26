using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WoodenAutomative.Domain.Dtos.Core;
using WoodenAutomative.Domain.Dtos.Request.Distributor;
using WoodenAutomative.EntityFramework.Interfaces.Services;

namespace WoodenAutomative.Controllers
{
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    [Authorize(Roles = RoleAuthenticate.Admin)]
    public class DistributorController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUserService _userService;
        private readonly ILoginService _loginService;
        private readonly INotyfService _notyf;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ICurrentUserAccessor _currentUserAccessor;

        public DistributorController(ILogger<HomeController> logger,
            IUserService userService,
            ILoginService loginService,
            INotyfService notyf,
            IHttpContextAccessor httpContextAccessor,
            ICurrentUserAccessor currentUserAccessor)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _loginService = loginService ?? throw new ArgumentNullException(nameof(loginService));
            _notyf = notyf ?? throw new ArgumentNullException(nameof(notyf));
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            _currentUserAccessor = currentUserAccessor ?? throw new ArgumentNullException(nameof(currentUserAccessor));
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> GetDistributorList(DistributorListRequest distributorListRequest)
        {
            try
            {
                var data = await _userService.GetDistributorList(distributorListRequest);

                return Json(new { draw = distributorListRequest.Draw, 
                    recordsFiltered = data.TotalRecords, 
                    recordsTotal = data.TotalRecords, 
                    data = data.DistributorDetails 
                });
            }
            catch (Exception ex)
            {
                throw;
            }

        }
    }
}
