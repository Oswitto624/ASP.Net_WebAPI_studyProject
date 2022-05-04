using WebStore.Domain;
using WebStore.Domain.Entities;

namespace WebStore.Interfaces.Services;

public interface IEmployeesData
{
    Task<int> CountAsync(CancellationToken Cancel = default);

    Task<IEnumerable<Employee>> GetAsync(int Skip, int Take, CancellationToken Cancel = default);

    Task<Page<Employee>> GetPageAsync(int PageIndex, int PageSize, CancellationToken Cancel = default);

    Task<IEnumerable<Employee>> GetAllAsync(CancellationToken CancellationToken = default);

    Task<Employee?> GetByIdAsync(int id, CancellationToken CancellationToken = default);

    Task<int> AddAsync(Employee employee, CancellationToken CancellationToken = default);

    Task<bool> EditAsync(Employee employee, CancellationToken CancellationToken = default);

    Task<bool> DeleteAsync(int id, CancellationToken CancellationToken = default);
}
