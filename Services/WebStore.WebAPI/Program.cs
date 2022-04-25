using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using WebStore.DAL.Context;
using WebStore.Domain.Entities.Identity;
using WebStore.Interfaces.Services;
using WebStore.Logging;
using WebStore.Services.Services.InMemory;
using WebStore.Services.Services.InSQL;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.AddLog4Net();

#region Настройка сервисов приложения
var services = builder.Services;
var configuration = builder.Configuration;

var db_connection_string_name = configuration["Database"];
var db_connection_string = configuration.GetConnectionString(db_connection_string_name);

switch (db_connection_string_name)
{
    case "SqlServer":
    case "DockerDB":
        services.AddDbContext<WebStoreDB>(opt => opt.UseSqlServer(db_connection_string));
        break;

    case "Sqlite":
        services.AddDbContext<WebStoreDB>(opt => opt.UseSqlite(db_connection_string, o => o.MigrationsAssembly("WebStore.DAL.Sqlite")));
        break;
}
services.AddTransient<IDbInitializer, DbInitializer>();

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

services.AddScoped<IEmployeesData, InMemoryEmployeesData>();
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
    await db_initializer.InitializeAsync(RemoveBefore: false);
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
