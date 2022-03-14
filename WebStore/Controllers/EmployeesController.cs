using Microsoft.AspNetCore.Mvc;
using WebStore.Models;

namespace WebStore.Controllers;

public class EmployeesController : Controller
{
    private static readonly List<Employee> __Employees = new()
    {
        new Employee { Id = 1, LastName = "Иванов", FirstName = "Иван", Patronymic = "Иваныч", Age = 23 },
        new Employee { Id = 2, LastName = "Сидорова", FirstName = "Сидорыня", Patronymic = "Сидоровна", Age = 43 },
        new Employee { Id = 3, LastName = "Петров", FirstName = "Петро", Patronymic = "Петрович", Age = 37 },
    };

    public IActionResult Index()
    {
        return View(__Employees);
    }

    public IActionResult Details(int Id)
    {
        var employee = __Employees.FirstOrDefault(e => e.Id == Id);

        if(employee == null)
            return NotFound();

        return View(employee);
    }
}
