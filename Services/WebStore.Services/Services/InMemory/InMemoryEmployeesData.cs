using Microsoft.Extensions.Logging;
using WebStore.Domain.Entities;
using WebStore.Interfaces.Services;
using WebStore.Services.Data;

namespace WebStore.Services.Services.InMemory;

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

    public Task<IEnumerable<Employee>> GetAllAsync(CancellationToken Cancel = default)
    {
        return Cancel.IsCancellationRequested
            ? Task.FromCanceled<IEnumerable<Employee>>(Cancel)
            : Task.FromResult(_Employees.AsEnumerable());
    }

    public Task<Employee?> GetByIdAsync(int id, CancellationToken Cancel = default)
    {
        if(Cancel.IsCancellationRequested)
            return Task.FromCanceled<Employee?>(Cancel);

        var employee = _Employees.FirstOrDefault(employee => employee.Id == id);
        return Task.FromResult(employee);
    }

    public Task<int> AddAsync(Employee employee, CancellationToken Cancel = default)
    {
        if (employee is null)
            throw new ArgumentNullException(nameof(employee));

        if(Cancel.IsCancellationRequested)
            return Task.FromCanceled<int>(Cancel);

        //удалить потом, только для данного сервиса
        if (_Employees.Contains(employee))
            return Task.FromResult(employee.Id);

        employee.Id = _LastFreeId++;
        _Employees.Add(employee);

        return Task.FromResult(employee.Id);
    }

    public async Task<bool> EditAsync(Employee employee, CancellationToken Cancel = default)
    {
        if (employee is null)
            throw new ArgumentNullException(nameof(employee));

        Cancel.ThrowIfCancellationRequested();

        //удалить потом, только для данного сервиса
        if (_Employees.Contains(employee))
            return true;

        var db_employee = await GetByIdAsync(employee.Id, Cancel).ConfigureAwait(false);
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

    public async Task<bool> DeleteAsync(int id, CancellationToken Cancel = default)
    {
        var db_employee = await GetByIdAsync(id, Cancel).ConfigureAwait(false);
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
