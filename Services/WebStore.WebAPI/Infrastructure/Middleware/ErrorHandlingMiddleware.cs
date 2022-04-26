namespace WebStore.WebAPI.Infrastructure.Middleware;

public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _Next;
    private readonly ILogger<ErrorHandlingMiddleware> _Logger;

    public ErrorHandlingMiddleware(RequestDelegate Next, ILogger<ErrorHandlingMiddleware> Logger)
    {
        _Next = Next;
        _Logger = Logger;
    }

    public async Task InvokeAsync(HttpContext Context)
    {
        try
        {
            await _Next(Context);
        }
        catch (Exception e)
        {
            HandleException(Context, e);
            throw;
        }
    }

    private void HandleException(HttpContext Context, Exception Error)
    {
        _Logger.LogError(Error, "Ошибка при обработке запроса {0}", Context.Request.Path);
    }
}
