using Microsoft.AspNetCore.Mvc;
using WebStore.Models;
using WebStore.Services.Interfaces;

namespace WebStore.Controllers;

//[Route("Staff/{action=Index}/{id?}")]
public class EmployeesController : Controller
{
    private readonly IEmployeesData _EmployeesData;
    private readonly ILogger<EmployeesController> _Logger;

    public EmployeesController(IEmployeesData EmployeesData, ILogger<EmployeesController> Logger)
    {
        _EmployeesData= EmployeesData;
        _Logger = Logger;
    }

    public IActionResult Index()
    {
        var employees = _EmployeesData.GetAll();
        return View(employees);
    }

    //[Route("~/employees/info-({id:int})")]
    public IActionResult Details(int Id)
    {
        var employee = _EmployeesData.GetById(Id);

        if(employee == null)
            return NotFound();

        return View(employee);
    }

    //public IActionResult add()
    //{
    //    return View();
    //}

    public IActionResult Edit(int id)
    {
        return View();
    }
    //public IActionResult Delete(int id)
    //{
    //    return View();
    //}
}
