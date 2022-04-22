using Microsoft.AspNetCore.Mvc;
using WebStore.Domain.Entities;
using WebStore.Interfaces;
using WebStore.Interfaces.Services;

namespace WebStore.WebAPI.Controllers;

/// <summary>Сотрудники</summary>
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

    /// <summary>Все сотрудники</summary>
    /// <returns>Возвращает список всех сотрудников</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Employee>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public IActionResult GetAll()
    {
        var employees = _EmployeesData.GetAll();
        if (employees.Any())
            return Ok(employees);
        return NoContent();
    }

    /// <summary>Сотрудник с заданным идентификатором</summary>
    /// <param name="Id">Искомый идентификатор сотрудника</param>
    /// <returns>Сотрудник с искомым идентификатором, либо <c>null</c> в случае его необнаружения</returns>
    [HttpGet("{Id:int}")]
    [ProducesResponseType(typeof(Employee), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetById(int Id)
    {
        var employee = _EmployeesData.GetById(Id);
        if(employee is null)
            return NotFound();
        return Ok(employee);
    }

    /// <summary>Добавление сотрудника</summary>
    /// <param name="employee">Новый сотрудник</param>
    /// <returns>Созданный сотрудник</returns>
    [HttpPost]
    [ProducesResponseType(typeof(IEnumerable<Employee>), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public IActionResult Add(Employee employee)
    {
        var id = _EmployeesData.Add(employee);
        _Logger.LogInformation("Сотрудник {0} добавлен с идентификатором {1}.", employee, id);
        return CreatedAtAction(nameof(GetById), new { Id = id }, employee);
    }


    /// <summary>Редактирование сотрудника</summary>
    /// <param name="employee">Информация для редактирования сотрудника</param>
    /// <returns>true если редактирование выполнено успешно</returns>
    [HttpPut]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
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

    /// <summary>Удаление сотрудника</summary>
    /// <param name="Id">Идентификатор удаляемого сотрудника</param>
    /// <returns><c>true</c> если сотрудник был удалён</returns>
    [HttpDelete("{Id}")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(bool), StatusCodes.Status404NotFound)]
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
