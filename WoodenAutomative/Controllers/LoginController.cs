using Microsoft.AspNetCore.Mvc;
using WoodenAutomative.Domain.Dtos.Request.Login;
using WoodenAutomative.EntityFramework;
using WoodenAutomative.EntityFramework.Interfaces.Services;

namespace WoodenAutomative.Controllers
{
    public class LoginController : Controller
    {

        private readonly WoodenAutomativeContext _context;
        private readonly ILoginService _loginService;

        public LoginController(ILoginService loginService, 
                               WoodenAutomativeContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _loginService = loginService ?? throw new ArgumentNullException(nameof(loginService));
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                ViewData["ErrorMsg"] = null;
                return View();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> Index(LoginRequest loginRequest)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(loginRequest.Email) && !string.IsNullOrWhiteSpace(loginRequest.Password))
                {

                    var regStatus = await _loginService.SignIn(this.HttpContext, loginRequest);
                    if (regStatus == 0)
                    {
                        TempData["ErrorMsg"] = "Please enter valid Username / Password.";
                        return RedirectToAction("Index", "Login");
                    }
                    else
                    {
                        return RedirectToAction("RedirectToDashboard");
                    }

                }
                return View();
            }
            catch (Exception ex)
            {
                Logger.Error(ex.ToString());
                throw;
            }
        }


        public IActionResult Roles()
        {
            try
            {
                //var list=_context.Role.ToList();
                ViewData["ErrorMsg"] = null;
                return View();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
