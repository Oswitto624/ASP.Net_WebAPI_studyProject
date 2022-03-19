using Microsoft.AspNetCore.Mvc;
using WebStore.Models;
using WebStore.Services.Interfaces;
using WebStore.ViewModels;

namespace WebStore.Controllers;

//[Route("Staff/{action=Index}/{id?}")]
public class EmployeesController : Controller
{
    private readonly IEmployeesData _EmployeesData;
    private readonly ILogger<EmployeesController> _Logger;

    public EmployeesController(IEmployeesData EmployeesData, ILogger<EmployeesController> Logger)
    {
        _EmployeesData = EmployeesData;
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

        if (employee == null)
            return NotFound();

        return View(employee);
    }

    //public IActionResult add()
    //{
    //    return View();
    //}

    public IActionResult Edit(int id)
    {
        var employee = _EmployeesData.GetById(id);
        if (employee is null)
            return NotFound();

        var model = new EmployeesViewModel
        {
            Id = employee.Id,
            LastName = employee.LastName,
            FirstName = employee.FirstName,
            Patronymic = employee.Patronymic,
            ShortName = employee.ShortName,
            Age = employee.Age,
        };

        return View(model);
    }

    [HttpPost]
    public IActionResult Edit(EmployeesViewModel Model)
    {
        var employee = new Employee
        {
            Id = Model.Id,
            FirstName = Model.FirstName,
            LastName = Model.LastName,
            Patronymic = Model.Patronymic,
            Age = Model.Age,
        };
        _EmployeesData.Edit(employee);
        return RedirectToAction(nameof(Index));
    }


    //public IActionResult Delete(int id)
    //{
    //    return View();
    //}
}
