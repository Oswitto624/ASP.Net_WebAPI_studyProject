using WebStore.Data;
using WebStore.Models;
using WebStore.Services.Interfaces;

namespace WebStore.Services;

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

    IEnumerable<Employee> GetAll()
    {
        return _Employees;
    }

    Employee? GetById(int id)
    {
        var employee = _Employees.FirstOrDefault(employee => employee.Id == id);
        return employee;
    }

    int Add(Employee employee)
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

    bool Edit(Employee employee)
    {
        if (employee is null)
            throw new ArgumentNullException(nameof(employee));

        //удалить потом, только для данного сервиса
        if (_Employees.Contains(employee))
            return employee.Id;

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

        //если сервис в БД, вызываем SaveChandges()

        _Logger.LogInformation("Сотрудник с id:{0} добавлен.", employee.Id);

        return true;
    }

    bool Delete(int id)
    {
        var db_employee = GetById(id);
        if (db_employee is null)
        {
            _Logger.LogWarning("Попытка удаления несуществующего сотрудника с id:{0}", employee.Id);
            return false;
        }
        
        _Employees.Remove(db_employee);
        _Logger.LogInformation("Сотрудник (id:{0)){1) удалён.", employee.Id, Employee);

        return true;
    }


}
