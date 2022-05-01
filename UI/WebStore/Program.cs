using Microsoft.AspNetCore.Identity;
using Polly;
using Polly.Extensions.Http;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Json;
using WebStore.Domain.Entities.Identity;
using WebStore.Interfaces.Services;
using WebStore.Interfaces.TestAPI;
using WebStore.Logging;
using WebStore.Services.Services;
using WebStore.Services.Services.InCookies;
using WebStore.WebAPI.Clients.Employees;
using WebStore.WebAPI.Clients.Identity;
using WebStore.WebAPI.Clients.Orders;
using WebStore.WebAPI.Clients.Products;
using WebStore.WebAPI.Clients.Values;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.AddLog4Net();
//builder.Host.ConfigureLogging(log =>
//    log
//        .ClearProviders()
//        .AddConsole(opt => opt.FormatterName = "json")
//        .AddDebug()
//        .AddEventLog(opt =>
//        {
//            opt.LogName = "WebStore";
//            opt.SourceName = "GB";
//        })    
//        .AddFilter<ConsoleLoggerProvider>("Microsoft", LogLevel.Warning)
//    );

builder.Host.UseSerilog((host, log) => log.ReadFrom.Configuration(host.Configuration)
    .MinimumLevel.Debug()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .WriteTo.Console(
        outputTemplate: "[{Timestamp:HH:mm:ss.fff} {Level:u3}]{SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}"
        //, theme: new SystemConsoleTheme()
        )
    .WriteTo.RollingFile($@".\Logs\WebStore[{DateTime.Now:yyy-MM-ddTHH-mm-ss}].log")
    .WriteTo.File(new JsonFormatter(", ", true), $@".\Logs\WebStore[{DateTime.Now:yyy-MM-ddTHH-mm-ss}].log.json")
    .WriteTo.Seq(host.Configuration["SeqAddress"])
    );


var services = builder.Services;
services.AddControllersWithViews();

var configuration = builder.Configuration;

services.AddIdentity<User, Role>()
    .AddDefaultTokenProviders();

services.AddHttpClient("WebStoreAPIIdentity", client => client.BaseAddress = new(configuration["WebAPI"]))
    .AddTypedClient<IUserStore<User>, UsersClient>()
    .AddTypedClient<IUserRoleStore<User>, UsersClient>()
    .AddTypedClient<IUserPasswordStore<User>, UsersClient>()
    .AddTypedClient<IUserEmailStore<User>, UsersClient>()
    .AddTypedClient<IUserPhoneNumberStore<User>, UsersClient>()
    .AddTypedClient<IUserTwoFactorStore<User>, UsersClient>()
    .AddTypedClient<IUserClaimStore<User>, UsersClient>()
    .AddTypedClient<IUserLoginStore<User>, UsersClient>()
    .AddTypedClient<IRoleStore<Role>, RolesClient>()
    .AddPolicyHandler(GetRetryPolicy())
    .AddPolicyHandler(GetCircuitBreakerPolicy())
    .SetHandlerLifetime(TimeSpan.FromMinutes(15))
    ;

services.Configure<IdentityOptions>(opt =>
{
#if DEBUG
    opt.Password.RequireDigit = false;
    opt.Password.RequireLowercase = false;
    opt.Password.RequireUppercase = false;
    opt.Password.RequireNonAlphanumeric = false;
    opt.Password.RequiredLength = 3;
    opt.Password.RequiredUniqueChars = 3;
#endif

    opt.User.RequireUniqueEmail = false;
    opt.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";

    opt.Lockout.AllowedForNewUsers = false;
    opt.Lockout.MaxFailedAccessAttempts = 10;
    opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
});

services.ConfigureApplicationCookie(opt =>
{
    opt.Cookie.Name = "WebStore.GB";
    opt.Cookie.HttpOnly = true;

    opt.ExpireTimeSpan = TimeSpan.FromDays(10);

    opt.LoginPath = "/Account/Login";
    opt.LogoutPath = "/Account/Logout";
    opt.AccessDeniedPath = "/Account/AccessDenied";

    opt.SlidingExpiration = true;
});

services.AddScoped<ICartStore, InCookiesCartStore>();
services.AddScoped<ICartService, CartService>();
//services.AddScoped<ICartService, InCookiesCartService>();

services.AddHttpClient("WebStoreAPI", client => client.BaseAddress = new(configuration["WebAPI"]))
    .AddTypedClient<IValuesService, ValuesClient>()
    .AddTypedClient<IEmployeesData, EmployeesClient>()
    .AddTypedClient<IProductData, ProductsClient>()
    .AddTypedClient<IOrderService, OrdersClient>()
    .AddPolicyHandler(GetRetryPolicy())
    .AddPolicyHandler(GetCircuitBreakerPolicy())
    .SetHandlerLifetime(TimeSpan.FromMinutes(15))
    ;

static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy(int MaxRetryCount = 5, int MaxJitterTime = 1000)
{
    var jitter = new Random();
    return HttpPolicyExtensions
        .HandleTransientHttpError()
        .WaitAndRetryAsync(
            MaxRetryCount, RetryAttempt =>
                TimeSpan.FromSeconds(Math.Pow(2, RetryAttempt)) +
                TimeSpan.FromMilliseconds(jitter.Next(0, MaxJitterTime)));
}

static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy() => 
    HttpPolicyExtensions
        .HandleTransientHttpError()
        .CircuitBreakerAsync(handledEventsAllowedBeforeBreaking: 5, TimeSpan.FromSeconds(30));

services.AddAutoMapper(typeof(Program));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "areas",
        pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
    );

    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}"
    );
});

app.Run();

public partial class Program { }