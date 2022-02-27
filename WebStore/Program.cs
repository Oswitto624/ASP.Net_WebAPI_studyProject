var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseRouting();
app.MapGet("/", () => app.Configuration["ServerGreetings"]);
//app.MapDefaultControllerRoute();
app.Run();