using AspNetCoreHero.ToastNotification;
using AspNetCoreHero.ToastNotification.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WoodenAutomative.Domain.Models;
using WoodenAutomative.EntityFramework;
using WoodenAutomative.EntityFramework.Interfaces.Services;
using WoodenAutomative.EntityFramework.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
builder.Services.AddDbContext<WoodenAutomativeContext>(options => options.UseSqlServer(
    builder.Configuration.GetConnectionString("WoodenAutomativeDbConString")));
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
        .AddEntityFrameworkStores<WoodenAutomativeContext>()
        .AddDefaultTokenProviders();

builder.Services.AddScoped<UserManager<ApplicationUser>, UserManager<ApplicationUser>>();
builder.Services.AddScoped<SignInManager<ApplicationUser>, SignInManager<ApplicationUser>>();

builder.Services.AddTransient<ILoginService, LoginService>();
builder.Services.AddTransient<IUserService, UserService>(); 

builder.Services.AddNotyf(config => { config.DurationInSeconds = 4; 
                                      config.IsDismissable = true; 
                                      config.Position = NotyfPosition.BottomRight; 
                         });

builder.Services.AddAuthentication("Cookies")
        .AddCookie("Cookies", options =>
        {
            options.LoginPath = "/Account/Login";
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

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseNotyf();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Index}/{id?}");

app.Run();
