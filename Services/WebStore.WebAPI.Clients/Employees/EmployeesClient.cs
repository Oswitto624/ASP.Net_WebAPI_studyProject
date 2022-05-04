using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Http.Json;
using WebStore.Domain;
using WebStore.Domain.Entities;
using WebStore.Interfaces;
using WebStore.Interfaces.Services;
using WebStore.WebAPI.Clients.Base;

namespace WebStore.WebAPI.Clients.Employees;

public class EmployeesClient : BaseClient, IEmployeesData
{
    private readonly ILogger<EmployeesClient> _Logger;

    public EmployeesClient(HttpClient Client, ILogger<EmployeesClient> Logger) 
        : base(Client, WebAPIAddresses.V1.Employees)
    {
        _Logger = Logger;
    }

    public async Task<int> CountAsync(CancellationToken Cancel = default)
    {
        var count = await GetAsync<int>($"{Address}/count", Cancel).ConfigureAwait(false);
        return count;
    }

    public async Task<IEnumerable<Employee>> GetAsync(int Skip, int Take, CancellationToken Cancel = default)
    {
        var response = await Http.GetAsync($"{Address}/({Skip}:{Take})", Cancel).ConfigureAwait(false);
        
        if(response.StatusCode == HttpStatusCode.NoContent)
            return Enumerable.Empty<Employee>();

        var items = await response
            .EnsureSuccessStatusCode()
            .Content
            .ReadFromJsonAsync<IEnumerable<Employee>>(cancellationToken: Cancel)
            ?? throw new InvalidOperationException("Не удалось получить список сотрудников");

        return items;
    }

    public async Task<Page<Employee>> GetPageAsync(int PageIndex, int PageSize, CancellationToken Cancel = default)
    {
        var page = await GetAsync<Page<Employee>>($"{Address}/page({PageIndex}:{PageSize})", Cancel)
            .ConfigureAwait(false)
            ?? throw new InvalidOperationException("Не удалось получить список сотрудников");
        return page;
    }

    public async Task<IEnumerable<Employee>> GetAllAsync(CancellationToken Cancel = default)
    {
        var employees = await GetAsync<IEnumerable<Employee>>(Address, Cancel).ConfigureAwait(false);
        return employees ?? Enumerable.Empty<Employee>();
    }

    public async Task<Employee?> GetByIdAsync(int id, CancellationToken Cancel = default)
    {
        var employee = await GetAsync<Employee>($"{Address}/{id}", Cancel).ConfigureAwait(false);
        return employee;
    }

    public async Task<int> AddAsync(Employee employee, CancellationToken Cancel = default)
    {
        var response = await PostAsync(Address, employee, Cancel).ConfigureAwait(false);
        
        var added_employee = await response
            .Content
            .ReadFromJsonAsync<Employee>(cancellationToken: Cancel);
        
        if (added_employee is null)
            return -1;

        var id = added_employee.Id;
        employee.Id = id;
        return id;
    }

    public async Task<bool> EditAsync(Employee employee, CancellationToken Cancel = default)
    {
        var response = await PutAsync(Address, employee, Cancel).ConfigureAwait(false);
        
        var success = await response
            .EnsureSuccessStatusCode()
            .Content
            .ReadFromJsonAsync<bool>(cancellationToken: Cancel);
        
        return success;
    }

    public async Task<bool> DeleteAsync(int Id, CancellationToken Cancel = default)
    {
        var response = await DeleteAsync($"{Address}/{Id}", Cancel).ConfigureAwait(false);
        var success = response.IsSuccessStatusCode;
        return success;
    }
}
