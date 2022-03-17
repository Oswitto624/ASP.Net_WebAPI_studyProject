using Microsoft.AspNetCore.Mvc;
using WebStore.Models;

namespace WebStore.Controllers;

//[Route("Staff/{action=Index}/{id?}")]
public class EmployeesController : Controller
{

    public IActionResult Index()
    {
        return View(__Employees);
    }

    //[Route("~/employees/info-({id:int})")]
    public IActionResult Details(int Id)
    {
        var employee = __Employees.FirstOrDefault(e => e.Id == Id);

        if(employee == null)
            return NotFound();

        return View(employee);
    }
}
