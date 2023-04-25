using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using WoodenAutomative.Domain.Models;
using WoodenAutomative.EntityFramework.Interfaces.Services;
using WoodenAutomative.EntityFramework.Repositories;

namespace WoodenAutomative.EntityFramework.Services
{
    public class UnitOfWork:IUnitOfWork
    {
        private WoodenAutomativeContext _db;
        private UserManager<ApplicationUser> _userManager;
        private SignInManager<ApplicationUser> _signInManager;
        private IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;

        public UnitOfWork(WoodenAutomativeContext db,
                          UserManager<ApplicationUser> userManager,
                          SignInManager<ApplicationUser> signInManager,
                          IHttpContextAccessor httpContextAccessor,
                          IConfiguration configuration)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db)); ;
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));

            Login = new LoginService(_userManager,_db,_signInManager,_httpContextAccessor);
            User = new UserService(_userManager,_db, _configuration,_httpContextAccessor);
            CurrentUserAccessor = new CurrentUserAccessor(_httpContextAccessor);
            Email = new EmailRepository(_userManager, _configuration, _db);
            Authorization = new AuthorizationRepository(_userManager, _db, _configuration, _signInManager, _httpContextAccessor);

        }

        public ILoginService Login { get; private set; }

        public IUserService User { get; private set; }

        public ICurrentUserAccessor CurrentUserAccessor { get; private set; }

        public IEmailRepository Email { get; private set; }

        public IAuthorizationRepository Authorization { get; private set; }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
