var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
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

app.MapControllerRoute(
    name: "ActionRoute",
    pattern: "{controller}.{action}({a}, {b})");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();