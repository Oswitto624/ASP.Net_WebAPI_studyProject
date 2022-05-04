using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebStore.DAL.Context;
using WebStore.Domain;
using WebStore.Domain.Entities;
using WebStore.Interfaces.Services;

namespace WebStore.Services.Services.InSQL;

public class SqlEmployeesData : IEmployeesData
{
    private readonly WebStoreDB _db;
    private readonly ILogger<SqlEmployeesData> _Logger;

    public SqlEmployeesData(WebStoreDB db, ILogger<SqlEmployeesData> Logger)
    {
        _Logger = Logger;
        _db = db;
    }

    public async Task<int> CountAsync(CancellationToken Cancel = default)
    {
        var count = await _db.Employees.CountAsync(Cancel).ConfigureAwait(false);
        return count;
    }

    public async Task<IEnumerable<Employee>> GetAsync(int Skip, int Take, CancellationToken Cancel = default)
    {
        if(Take==0)
            return Enumerable.Empty<Employee>();

        var items = await _db.Employees
            .Skip(Skip)
            .Take(Take)
            .ToArrayAsync(Cancel)
            .ConfigureAwait(false);
        
        return items;
    }

    public async Task<Page<Employee>> GetPageAsync(int PageIndex, int PageSize, CancellationToken Cancel = default)
    {
        var total_count = await _db.Employees.CountAsync(Cancel).ConfigureAwait(false);

        if (PageSize == 0)
            return new(Enumerable.Empty<Employee>(), PageIndex, PageSize, total_count);

        var items = await _db.Employees
            .Skip(PageIndex * PageSize)
            .Take(PageSize)
            .ToArrayAsync(Cancel);

        return new(items, PageIndex, PageSize, total_count);
    }

    public async Task<IEnumerable<Employee>> GetAllAsync(CancellationToken Cancel = default)
    {
        var employees = await _db.Employees.ToArrayAsync(Cancel).ConfigureAwait(false);
        return employees;
    }

    public async Task<Employee?> GetByIdAsync(int id, CancellationToken Cancel = default)
    {
        var employee = await _db.Employees
            .FirstOrDefaultAsync(e => e.Id == id, Cancel)
            .ConfigureAwait(false);

        return employee;
    }

    public async Task<int> AddAsync(Employee employee, CancellationToken Cancel = default)
    {
        if (employee is null)
            throw new ArgumentNullException(nameof(employee));

        await _db.Employees.AddAsync(employee, Cancel).ConfigureAwait(false);
        await _db.SaveChangesAsync(Cancel);

        return employee.Id;
    }

    public async Task<bool> EditAsync(Employee employee, CancellationToken Cancel = default)
    {
        if (employee is null)
            throw new ArgumentNullException(nameof(employee));

        if (!await _db.Employees.AnyAsync(e => e.Id == employee.Id, Cancel).ConfigureAwait(false))
            return false;

        _db.Update(employee);
        await _db.SaveChangesAsync(Cancel);

        _Logger.LogInformation("Сотрудник (id:{0}){1} добавлен.", employee.Id, employee);

        return true;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken Cancel = default)
    {
        var db_employee = await _db.Employees
            .Select(e => new Employee { Id = e.Id })
            .FirstOrDefaultAsync(e => e.Id == id, Cancel)
            .ConfigureAwait(false);

        if (db_employee is null)
        {
            _Logger.LogWarning("Попытка удаления несуществующего сотрудника с id:{0}", id);
            return false;
        }

        _db.Remove(db_employee);
        await _db.SaveChangesAsync(Cancel);

        _Logger.LogInformation("Сотрудник c id:{0} удалён.", id);

        return true;
    }
}
