namespace WebStore.Infrastructure.Middleware;

public class TestMiddleware
{
    private readonly RequestDelegate _Next;
    public TestMiddleware(RequestDelegate Next) => _Next = Next; 
    public async Task Invoke(HttpContext Context)
    {
        //Context.Response.StatusCode = 404;

        await _Next(Context);
    }
}
