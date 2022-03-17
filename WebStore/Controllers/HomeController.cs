using Microsoft.AspNetCore.Mvc;
using WebStore.Models;

namespace WebStore.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration _Configuration;

        public HomeController(IConfiguration Configuration) { _Configuration = Configuration; }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ContentString(string Id = "-id-")
        {
            return Content($"content: {Id}");
        }

        public IActionResult ConfigStr()
        {
            return Content($"config: {_Configuration["ServerGreetings"]}");
        }

        public IActionResult Sum(int a, int b)
        {
            return Content((a + b).ToString());
        }
    }
}
