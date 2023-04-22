using AspNetCoreHero.ToastNotification;
using AspNetCoreHero.ToastNotification.Extensions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WoodenAutomative.Domain.Models;
using WoodenAutomative.EntityFramework;
using WoodenAutomative.EntityFramework.Interfaces.Services;
using WoodenAutomative.EntityFramework.Repositories;
using WoodenAutomative.EntityFramework.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
builder.Services.AddDbContext<WoodenAutomativeContext>(options => options.UseSqlServer(
    builder.Configuration.GetConnectionString("WoodenAutomativeDbConString")));


builder.Services.AddIdentity<ApplicationUser, IdentityRole>().AddDefaultTokenProviders()
        .AddEntityFrameworkStores<WoodenAutomativeContext>();
builder.Services.AddScoped<UserManager<ApplicationUser>, UserManager<ApplicationUser>>();
builder.Services.AddScoped<SignInManager<ApplicationUser>, SignInManager<ApplicationUser>>();
builder.Services.AddSingleton<ICurrentUserAccessor, CurrentUserAccessor>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddTransient<ILoginService, LoginService>();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IAuthorizationRepository, AuthorizationRepository>();
builder.Services.AddTransient<IEmailRepository, EmailRepository>();

builder.Services.AddNotyf(config =>
{
    config.DurationInSeconds = 4;
    config.IsDismissable = true;
    config.Position = NotyfPosition.BottomRight;
});

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(20);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
        .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme,
            options =>
            {
                options.LoginPath = "/Login/Index";
                options.LogoutPath = "/Login/LogOut";
                options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
                options.SlidingExpiration = true;
                options.AccessDeniedPath = "/Forbidden/";
            });

// authentication 
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
});


//LogForNet
builder.Logging.SetMinimumLevel(LogLevel.Error);
builder.Logging.SetMinimumLevel(LogLevel.Debug);
builder.Logging.SetMinimumLevel(LogLevel.Warning);
builder.Logging.AddLog4Net("log4net.config");
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseRouting();

app.UseStaticFiles();

app.UseAuthentication();

app.UseAuthentication();

app.UseAuthorization();

app.UseNotyf();

//app.Use(async (context, next) =>
//{
//    var user = context.User;
//    if (user.Identity.IsAuthenticated && user.HasClaim(c => c.Type == "SecurityStamp"))
//    {
//        var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
//        var securityStamp = user.FindFirstValue("SecurityStamp");

//        var userManager = context.RequestServices.GetService<UserManager<ApplicationUser>>();

//        var currentUser = await userManager.FindByIdAsync(userId);
//        if (currentUser != null && currentUser.SecurityStamp != securityStamp)
//        {
//            await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
//            return;
//        }
//    }
//    await next.Invoke();
//});

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
});

app.Run();
