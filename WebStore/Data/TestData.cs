using WebStore.Domain.Entities;

namespace WebStore.Data;

public class TestData
{
    public static ICollection<Employee> Employees { get; } = new List<Employee>()
    {
        new () { Id = 1, LastName = "Иванов", FirstName = "Иван", Patronymic = "Иваныч", Age = 23 },
        new () { Id = 2, LastName = "Сидорова", FirstName = "Сидорыня", Patronymic = "Сидоровна", Age = 43 },
        new () { Id = 3, LastName = "Петров", FirstName = "Петро", Patronymic = "Петрович", Age = 37 },
    };
}
