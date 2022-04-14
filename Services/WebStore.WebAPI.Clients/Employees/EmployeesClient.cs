using Microsoft.Extensions.Logging;
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

    public int Add(Employee employee)
    {
        throw new NotImplementedException();
    }

    public bool Delete(int id)
    {
        throw new NotImplementedException();
    }

    public bool Edit(Employee employee)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Employee> GetAll()
    {
        throw new NotImplementedException();
    }

    public Employee? GetById(int id)
    {
        throw new NotImplementedException();
    }
}
