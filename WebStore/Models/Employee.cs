using System.Text;

namespace WebStore.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Patronymic { get; set; }
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


    }
}
