using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebStore.Domain.Entities;
using WebStore.Domain.Entities.Identity;
using WebStore.Domain.ViewModels;
using WebStore.Interfaces.Services;

namespace WebStore.Controllers;

//[Route("Staff/{action=Index}/{id?}")]
[Authorize]
public class EmployeesController : Controller
{
    private readonly IEmployeesData _EmployeesData;
    private readonly ILogger<EmployeesController> _Logger;

    public EmployeesController(IEmployeesData EmployeesData, ILogger<EmployeesController> Logger)
    {
        _EmployeesData = EmployeesData;
        _Logger = Logger;
    }

    public async Task<IActionResult> Index()
    {
        var employees = await _EmployeesData.GetAllAsync();
        return View(employees);
    }

    //[Route("~/employees/info-({id:int})")]
    public async Task<IActionResult> Details(int Id)
    {
        var employee = await _EmployeesData.GetByIdAsync(Id);

        if (employee == null)
            return NotFound();

        return View(employee);
    }

    [Authorize(Roles = Role.Administrators)]
    public IActionResult Create() => View("Edit", new EmployeesViewModel());

    [Authorize(Roles = Role.Administrators)]
    public async Task<IActionResult> Edit(int? Id)
    {
        if (Id is not { } id)
        {
            return View(new EmployeesViewModel());
        }

        var employee = await _EmployeesData.GetByIdAsync(id);
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
    [Authorize(Roles = Role.Administrators)]
    public async Task<IActionResult> Edit(EmployeesViewModel Model)
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
            var id = await _EmployeesData.AddAsync(employee);
            return RedirectToAction(nameof(Details), new {id});
        }

        await _EmployeesData.EditAsync(employee);
        return RedirectToAction(nameof(Index));
    }

    [Authorize(Roles = Role.Administrators)]
    public async Task<IActionResult> Delete(int id)
    {
        if(id<0)
            return BadRequest();

        var employee = await _EmployeesData.GetByIdAsync(id);
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
    [Authorize(Roles = Role.Administrators)]
    public async Task<IActionResult> DeleteConfirmed(int Id) 
    {
        if (!await _EmployeesData.DeleteAsync(Id))
            return NotFound();

        return RedirectToAction(nameof(Index));
    }
}
