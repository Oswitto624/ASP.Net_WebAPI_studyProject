using Microsoft.AspNetCore.Mvc;
using WebStore.Interfaces.Services;
using WebStore.Services.Mapping;

namespace WebStore.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration _Configuration;

        public HomeController(IConfiguration Configuration) => _Configuration = Configuration; 

        public IActionResult Index([FromServices] IProductData ProductData)
        {
            var products = ProductData.GetProducts()
                .Items
                .OrderBy(p => p.Order)
                .Take(6)
                .ToView();

            ViewBag.Products = products;

            return View();
        }

        public IActionResult ContentString(string Id = "-id-")
        {
            if(Id is null) throw new ArgumentNullException(nameof(Id));
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
