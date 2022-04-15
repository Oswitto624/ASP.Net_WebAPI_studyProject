using Microsoft.AspNetCore.Mvc;
using WebStore.Domain.Entities;
using WebStore.Interfaces.Services;

namespace WebStore.WebAPI.Controllers;

[ApiController]
[Route("api/employees")]
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
        if(employees.Any())
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
        return CreatedAtAction(nameof(GetById), new { Id = id }, employee);
    }

    [HttpPut]
    public IActionResult Edit(Employee employee)
    {
        var success = _EmployeesData.Edit(employee);
        return Ok(success);
    }

    [HttpDelete("Id")]
    public IActionResult Delete(int Id)
    {
        var result = _EmployeesData.Delete(Id);
        return result
            ? Ok(true)
            : NotFound(false);
    }
}
