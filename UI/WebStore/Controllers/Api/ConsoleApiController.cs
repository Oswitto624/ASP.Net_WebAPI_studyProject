using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace WebStore.Controllers.Api;

#if DEBUG

[ApiController, Route("api/console")]
public class ConsoleApiController : ControllerBase
{
    [HttpGet("clear")]
    //[Conditional("DEBUG")]
    public void Clear() => Console.Clear();

    [HttpGet("write")]
    //[Conditional("DEBUG")]
    public void WriteLine(string Str) => Console.WriteLine(Str);

    [HttpGet("title")]
    //[Conditional("DEBUG")]
    public void SetTitle(string Str) => Console.Title = Str;
}

#endif