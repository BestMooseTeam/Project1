using System;
using System.Globalization;

namespace EmployeeManager
{
    /// <summary>Сотрудник компании: имя, должность и зарплата.</summary>
    public class Employee
    {
        private string _name;
        private string _position;
        private decimal _salary;

        public Employee(string name, string position, decimal salary)
        {
            Name = name;
            Position = position;
            Salary = salary;
        }

        public string Name
        {
            get { return _name; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Имя не может быть пустым.", "name");
                _name = value.Trim();
            }
        }

        public string Position
        {
            get { return _position; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Должность не может быть пустой.", "position");
                _position = value.Trim();
            }
        }

        public decimal Salary
        {
            get { return _salary; }
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException("salary", "Зарплата не может быть отрицательной.");
                _salary = value;
            }
        }

        /// <summary>Повышает зарплату на указанный процент (например, 10 — на 10%).</summary>
        public void RaiseSalary(decimal percent)
        {
            if (percent <= 0)
                throw new ArgumentOutOfRangeException("percent", "Процент повышения должен быть больше нуля.");
            Salary = Salary * (1 + percent / 100m);
        }

        /// <summary>Повышает зарплату на фиксированную сумму.</summary>
        public void RaiseSalaryByAmount(decimal amount)
        {
            if (amount <= 0)
                throw new ArgumentOutOfRangeException("amount", "Сумма повышения должна быть больше нуля.");
            Salary = Salary + amount;
        }

        /// <summary>Возвращает строку с данными сотрудника.</summary>
        public string GetInfo()
        {
            return string.Format(
                CultureInfo.GetCultureInfo("ru-RU"),
                "{0} — {1}, зарплата {2:N2} ₽",
                Name, Position, Salary);
        }

        /// <summary>Выводит информацию о сотруднике в консоль.</summary>
        public void PrintInfo()
        {
            Console.WriteLine(GetInfo());
        }

        public override string ToString()
        {
            return GetInfo();
        }
    }
}
