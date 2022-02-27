using Microsoft.AspNetCore.Mvc;

namespace WebStore.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return Content("Привет из контроллера!");
        }

        public IActionResult ContentString(string Id = "-id-")
        {
            return Content($"content: {Id}");
        }
    }
}
