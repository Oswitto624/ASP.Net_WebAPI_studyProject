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

    public IActionResult Create()
    {
        return View("Edit", new EmployeesViewModel());
    }

    public IActionResult Edit(int? Id)
    {
        if (Id is not { } id)
        {
            return View(new EmployeesViewModel());
        }

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
        if (Model.LastName == "Иванов" && Model.Age < 21)
            ModelState.AddModelError("", "Иванов должен быть старше 21 года");

        if(!ModelState.IsValid) return View(Model);

        var employee = new Employee
        {
            Id = Model.Id,
            FirstName = Model.FirstName,
            LastName = Model.LastName,
            Patronymic = Model.Patronymic,
            Age = Model.Age,
        };
        if(Model.Id == 0)
        {
            var id = _EmployeesData.Add(employee);
            return RedirectToAction(nameof(Details), new {id});
        }

        _EmployeesData.Edit(employee);
        return RedirectToAction(nameof(Index));
    }


    public IActionResult Delete(int id)
    {
        if(id<0)
            return BadRequest();

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
    public IActionResult DeleteConfirmed(int Id) 
    {
        if (!_EmployeesData.Delete(Id))
            return NotFound();

        return RedirectToAction(nameof(Index));
    }
}
