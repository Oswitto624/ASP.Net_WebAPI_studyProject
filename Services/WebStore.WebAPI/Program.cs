using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebStore.DAL.Context;
using WebStore.Domain.Entities.Identity;
using WebStore.Interfaces.Services;
using WebStore.Services.Services.InMemory;
using WebStore.Services.Services.InSQL;

var builder = WebApplication.CreateBuilder(args);

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

services.AddControllers();
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();
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
