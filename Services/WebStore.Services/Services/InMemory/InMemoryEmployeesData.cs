using Microsoft.Extensions.Logging;
using WebStore.Data;
using WebStore.Domain.Entities;
using WebStore.Services.Interfaces;

namespace WebStore.Services.InMemory;

public class InMemoryEmployeesData : IEmployeesData
{
    private int _LastFreeId;
    private readonly ILogger<InMemoryEmployeesData> _Logger;
    private readonly ICollection<Employee> _Employees;

    public InMemoryEmployeesData(ILogger<InMemoryEmployeesData> Logger)
    {
        _Logger = Logger;
        _Employees = TestData.Employees;
        _LastFreeId = _Employees.Count == 0 ? 1 : _Employees.Max(e => e.Id) + 1; //только для тестового сервиса
    }

    public IEnumerable<Employee> GetAll()
    {
        return _Employees;
    }

    public Employee? GetById(int id)
    {
        var employee = _Employees.FirstOrDefault(employee => employee.Id == id);
        return employee;
    }

    public int Add(Employee employee)
    {
        if(employee is null)
            throw new ArgumentNullException(nameof(employee));

        //удалить потом, только для данного сервиса
        if (_Employees.Contains(employee))
            return employee.Id;

        employee.Id = _LastFreeId++;
        _Employees.Add(employee);
        return employee.Id;
    }

    public bool Edit(Employee employee)
    {
        if (employee is null)
            throw new ArgumentNullException(nameof(employee));

        //удалить потом, только для данного сервиса
        if (_Employees.Contains(employee))
            return true;

        var db_employee = GetById(employee.Id);
        if (db_employee is null)
        {
            _Logger.LogWarning("Попытка редактировать несуществующего сотрудника с id:{0}", employee.Id);
            return false;
        }

        db_employee.LastName = employee.LastName;
        db_employee.FirstName = employee.FirstName;
        db_employee.Patronymic = employee.Patronymic;
        db_employee.Age = employee.Age;

        //если сервис в БД, вызываем SaveChanges()

        _Logger.LogInformation("Сотрудник (id:{0}){1} добавлен.", employee.Id, employee);

        return true;
    }

    public bool Delete(int id)
    {
        var db_employee = GetById(id);
        if (db_employee is null)
        {
            _Logger.LogWarning("Попытка удаления несуществующего сотрудника с id:{0}", id);
            return false;
        }
        
        _Employees.Remove(db_employee);
        _Logger.LogInformation("Сотрудник c id:{0}) удалён.", id);

        return true;
    }


}
