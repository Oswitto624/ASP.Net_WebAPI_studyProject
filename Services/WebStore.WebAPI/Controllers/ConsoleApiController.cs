using Microsoft.AspNetCore.Mvc;
using WebStore.Interfaces;

namespace WebStore.WebAPI.Controllers;

[ApiController, Route(WebAPIAddresses.V1.Console)]
public class ConsoleApiController : ControllerBase
{
    [HttpGet("clear")]
    public void Clear() => Console.Clear();

    [HttpGet("write")]
    public void WriteLine(string Str) => Console.WriteLine(Str);

    [HttpGet("title")]
    public void SetTitle(string Str) => Console.Title = Str;
}