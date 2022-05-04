using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Formatters;
using WebStore.DAL.Context;
using WebStore.Domain.Entities.Identity;
using WebStore.Interfaces.Services;
using WebStore.Logging;
using WebStore.Services.Services.InMemory;
using WebStore.Services.Services.InSQL;
using WebStore.WebAPI.Infrastructure.Middleware;
using WebStore.WebAPI.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.AddLog4Net();

#region Настройка сервисов приложения
var services = builder.Services;
var configuration = builder.Configuration;

services
    .AddWebStoreDB(configuration)
    .AddTransient<IDbInitializer, DbInitializer>();

services.AddIdentity<User, Role>(/*opt => opt.*/)
    .AddEntityFrameworkStores<WebStoreDB>()
    .AddDefaultTokenProviders();

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

//services.AddScoped<IEmployeesData, InMemoryEmployeesData>();
services.AddScoped<IEmployeesData, SqlEmployeesData>();
services.AddScoped<IProductData, SqlProductData>();
services.AddScoped<IOrderService, SqlOrderService>();

services.AddControllers(opt =>
{
    opt.InputFormatters.Add(new XmlSerializerInputFormatter(opt));
    opt.OutputFormatters.Add(new XmlSerializerOutputFormatter());
});
services.AddEndpointsApiExplorer();
services.AddSwaggerGen(opt =>
{
    const string webstore_webapi_xml = "WebStore.WebAPI.xml";
    const string webstore_domain_xml = "WebStore.Domain.xml";

    const string debug_path = "bin/Debug/net6.0";

    if (File.Exists(webstore_webapi_xml))
        opt.IncludeXmlComments(webstore_webapi_xml);
    else if (File.Exists(Path.Combine(debug_path, webstore_webapi_xml)))
        opt.IncludeXmlComments(Path.Combine(debug_path, webstore_webapi_xml));
    
    if (File.Exists(webstore_domain_xml))
        opt.IncludeXmlComments(webstore_domain_xml);
    else if (File.Exists(Path.Combine(debug_path, webstore_domain_xml)))
        opt.IncludeXmlComments(Path.Combine(debug_path, webstore_domain_xml));
});
#endregion

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db_initializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
    await db_initializer.InitializeAsync(RemoveBefore: true);
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ErrorHandlingMiddleware>();

app.UseAuthorization();

app.MapControllers();

app.Run();
