using Microsoft.AspNetCore.Mvc;
using WebStore.Domain.Entities;
using WebStore.Interfaces;
using WebStore.Interfaces.Services;

namespace WebStore.WebAPI.Controllers;

[ApiController]
[Route(WebAPIAddresses.V1.Employees)]
public class EmployeesApiController : ControllerBase
{
    private readonly IEmployeesData _EmployeesData;
    private readonly ILogger<EmployeesApiController> _Logger;

    public EmployeesApiController(IEmployeesData EmployeesData, ILogger<EmployeesApiController> Logger)
    {
        _EmployeesData = EmployeesData;
        _Logger = Logger;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var employees = _EmployeesData.GetAll();
        if (employees.Any())
            return Ok(employees);
        return NoContent();
    }

    [HttpGet("{Id:int}")]
    public IActionResult GetById(int Id)
    {
        var employee = _EmployeesData.GetById(Id);
        if(employee is null)
            return NotFound();
        return Ok(employee);
    }

    [HttpPost]
    public IActionResult Add(Employee employee)
    {
        var id = _EmployeesData.Add(employee);
        _Logger.LogInformation("Сотрудник {0} добавлен с идентификатором {1}.", employee, id);
        return CreatedAtAction(nameof(GetById), new { Id = id }, employee);
    }

    [HttpPut]
    public IActionResult Edit(Employee employee)
    {
        var success = _EmployeesData.Edit(employee);
        if (success)
        {
            _Logger.LogInformation("Сотрудник {0} отредактирован.", employee);
        }
        else
        {
            _Logger.LogWarning("Проблема при редактировании сотрудника {0}.", employee);
        }
        return Ok(success);
    }

    [HttpDelete("{Id}")]
    public IActionResult Delete(int Id)
    {
        var result = _EmployeesData.Delete(Id);

        if (result)
        {
            _Logger.LogInformation("Сотрудник c id:{0} удалён.", Id);
            return Ok(result);
        }
        _Logger.LogWarning("Сотрудник c id:{0} при удалении не найден.", Id);
        return NotFound(false);        
    }
}
