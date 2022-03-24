using Microsoft.EntityFrameworkCore;
using WebStore.DAL.Context;
using WebStore.Infrastructure.Conventions;
using WebStore.Infrastructure.Middleware;
using WebStore.Services;
using WebStore.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

//регистрация сервисов
var services = builder.Services;
builder.Services.AddControllersWithViews( opt =>
{
    opt.Conventions.Add(new TestConvention());
});

var configuration = builder.Configuration;
services.AddDbContext<WebStoreDB> (opt => opt.UseSqlServer(configuration.GetConnectionString("SqlServer")));

services.AddScoped<IEmployeesData, InMemoryEmployeesData>();
services.AddScoped<IProductData, InMemoryProductData>();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseStaticFiles();

app.UseRouting();


app.MapGet("/throw", () =>
{
    throw new ApplicationException("Пример ошибки приложения");
});


app.MapGet("/greetings", () => app.Configuration["ServerGreetings"]);

app.MapDefaultControllerRoute();

app.UseMiddleware<TestMiddleware>();

app.MapControllerRoute(
    name: "ActionRoute",
    pattern: "{controller}.{action}({a}, {b})");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();