using System.Text;
using WebStore.Domain.Entities.Base;

namespace WebStore.Domain.Entities;

/// <summary>Сотрудник</summary>
public class Employee : Entity
{
    /// <summary>Фамилия</summary>
    public string LastName { get; set; } = null!;

    /// <summary>Имя</summary>
    public string FirstName { get; set; } = null!;
    
    /// <summary>Отчество</summary>
    public string? Patronymic { get; set; }
    
    /// <summary>Фамилия и инициалы</summary>
    public string ShortName
    {
        get
        {
            var result = new StringBuilder(LastName);

            if (FirstName is { Length: > 0 } first_name)
                result.Append(" ").Append(first_name[0]).Append(".");

            if (Patronymic is { Length: > 0 } patronymic)
                result.Append(" ").Append(patronymic[0]).Append(".");

            return result.ToString();
        }
    }

    private int _Age;
    
    /// <summary>Возраст</summary>
    public int Age
    {
        get => _Age;
        set
        {
            if (value <= 0)
                throw new ArgumentOutOfRangeException(nameof(value), value, "Значение возраста не может быть меньше 0");
            _Age = value;
        }
    }

    public override string ToString() => $"[{Id}]{LastName} {FirstName} {Patronymic} {Age}";
}
