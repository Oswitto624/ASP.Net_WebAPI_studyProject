using Microsoft.AspNetCore.Mvc;
using WebStore.Models;

namespace WebStore.Controllers
{
    public class HomeController : Controller
    {
        private static readonly List<Employee> __Employees = new()
        {
            new Employee { Id = 1, LastName = "Иванов", FirstName = "Иван", Patronymic = "Иваныч", Age = 23 },
            new Employee { Id = 2, LastName = "Сидорова", FirstName = "Сидорыня", Patronymic = "Сидоровна", Age = 43 },
            new Employee { Id = 3, LastName = "Петров", FirstName = "Петро", Patronymic = "Петрович", Age = 37 },
        };

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

        public IActionResult EmployeesList()
        {
            return View(__Employees);
        }
    }
}
