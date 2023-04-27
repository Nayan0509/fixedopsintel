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
        private readonly ILoginService _loginService;
        private readonly INotyfService _notyf;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ICurrentUserAccessor _currentUserAccessor;
        private readonly IDistributorService _distributorService;

        public DistributorController(ILogger<HomeController> logger,
            ILoginService loginService,
            INotyfService notyf,
            IHttpContextAccessor httpContextAccessor,
            ICurrentUserAccessor currentUserAccessor, 
            IDistributorService distributorService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _loginService = loginService ?? throw new ArgumentNullException(nameof(loginService));
            _notyf = notyf ?? throw new ArgumentNullException(nameof(notyf));
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            _currentUserAccessor = currentUserAccessor ?? throw new ArgumentNullException(nameof(currentUserAccessor));
            _distributorService = distributorService ?? throw new ArgumentNullException(nameof(distributorService));
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AddDistributor()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddDistributor(DistributorRequest distributorRequest)
        {
            if (ModelState.IsValid)
            {
                await _distributorService.AddDistributorData(distributorRequest);
                ModelState.Clear();
                _notyf.Success("Distributor is successfully Added");
                return View();
            }
            return View(distributorRequest);
        }

        [HttpPost]
        public async Task<JsonResult> GetDistributorList(DistributorListRequest distributorListRequest)
        {
            try
            {
                var data = await _distributorService.GetDistributorList(distributorListRequest);

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
