using Microsoft.Extensions.Logging;
using System.Net.Http.Json;
using WebStore.Domain.Entities;
using WebStore.Interfaces.Services;
using WebStore.WebAPI.Clients.Base;

namespace WebStore.WebAPI.Clients.Employees;

public class EmployeesClient : BaseClient, IEmployeesData
{
    private readonly ILogger<EmployeesClient> _Logger;

    public EmployeesClient(HttpClient Client, ILogger<EmployeesClient> Logger) 
        : base(Client, "api/employees")
    {
        _Logger = Logger;
    }

    public IEnumerable<Employee> GetAll()
    {
        var employees = Get<IEnumerable<Employee>>(Address);
        return employees ?? Enumerable.Empty<Employee>();
    }

    public Employee? GetById(int id)
    {
        var employee = Get<Employee>($"{Address}/{id}");
        return employee;
    }

    public int Add(Employee employee)
    {
        var response = Post(Address, employee);
        var added_employee = response.Content.ReadFromJsonAsync<Employee>().Result;
        if (added_employee is null)
            return -1;

        var id = added_employee.Id;
        employee.Id = id;
        return id;
    }

    public bool Edit(Employee employee)
    {
        var response = Put(Address, employee);
        var success = response
            .EnsureSuccessStatusCode()
            .Content
            .ReadFromJsonAsync<bool>()
            .Result;
        return success;
    }

    public bool Delete(int Id)
    {
        var response = Delete($"{Address}/{Id}");
        var success = response.IsSuccessStatusCode;
        return success;
    }


}
