using System;
using System.Globalization;
using System.Text;

namespace EmployeeManager
{
    public static class Program
    {
        public static void Main()
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.InputEncoding = Encoding.UTF8;

            Company company = new Company();
            SeedDemo(company);

            Console.WriteLine("=== Управление сотрудниками ===");

            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("1 — список сотрудников");
                Console.WriteLine("2 — добавить сотрудника");
                Console.WriteLine("3 — повысить зарплату (%)");
                Console.WriteLine("4 — повысить зарплату (сумма)");
                Console.WriteLine("5 — сведения о сотруднике");
                Console.WriteLine("6 — удалить сотрудника");
                Console.WriteLine("0 — выход");
                Console.Write("Выбор: ");

                switch (ReadLineOrExit())
                {
                    case "1":
                        company.PrintAll();
                        break;
                    case "2":
                        AddEmployee(company);
                        break;
                    case "3":
                        RaisePercent(company);
                        break;
                    case "4":
                        RaiseAmount(company);
                        break;
                    case "5":
                        ShowEmployee(company);
                        break;
                    case "6":
                        RemoveEmployee(company);
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Неизвестная команда.");
                        break;
                }
            }
        }

        private static void SeedDemo(Company company)
        {
            company.Add(new Employee("Анна Иванова", "Разработчик", 120000m));
            company.Add(new Employee("Пётр Сидоров", "Тестировщик", 90000m));
            company.Add(new Employee("Мария Козлова", "Менеджер", 150000m));
        }

        private static void AddEmployee(Company company)
        {
            string name = AskNonEmpty("Имя: ");
            string position = AskNonEmpty("Должность: ");
            decimal salary = AskDecimal("Зарплата: ", 0m, 100000000m);

            try
            {
                company.Add(new Employee(name, position, salary));
                Console.WriteLine("Сотрудник добавлен.");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine("Ошибка: {0}", ex.Message);
            }
        }

        private static void RaisePercent(Company company)
        {
            Employee employee = AskEmployee(company);
            if (employee == null)
                return;

            decimal percent = AskDecimal("Процент повышения: ", 0.01m, 1000m);
            decimal before = employee.Salary;
            employee.RaiseSalary(percent);
            Console.WriteLine("{0}: {1:N2} ₽ → {2:N2} ₽", employee.Name, before, employee.Salary);
        }

        private static void RaiseAmount(Company company)
        {
            Employee employee = AskEmployee(company);
            if (employee == null)
                return;

            decimal amount = AskDecimal("Сумма повышения: ", 0.01m, 100000000m);
            decimal before = employee.Salary;
            employee.RaiseSalaryByAmount(amount);
            Console.WriteLine("{0}: {1:N2} ₽ → {2:N2} ₽", employee.Name, before, employee.Salary);
        }

        private static void ShowEmployee(Company company)
        {
            Employee employee = AskEmployee(company);
            if (employee == null)
                return;
            employee.PrintInfo();
        }

        private static void RemoveEmployee(Company company)
        {
            Employee employee = AskEmployee(company);
            if (employee == null)
                return;

            int index = -1;
            for (int i = 0; i < company.Count; i++)
            {
                if (object.ReferenceEquals(company.GetByIndex(i), employee))
                {
                    index = i;
                    break;
                }
            }

            if (company.RemoveByIndex(index))
                Console.WriteLine("Сотрудник {0} удалён.", employee.Name);
        }

        private static Employee AskEmployee(Company company)
        {
            if (company.Count == 0)
            {
                Console.WriteLine("Сотрудников пока нет.");
                return null;
            }

            company.PrintAll();
            while (true)
            {
                Console.Write("Номер сотрудника: ");
                int number;
                if (int.TryParse(ReadLineOrExit(), out number) && number >= 1 && number <= company.Count)
                    return company.GetByIndex(number - 1);
                Console.WriteLine("Введите номер от 1 до {0}.", company.Count);
            }
        }

        private static string AskNonEmpty(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string value = ReadLineOrExit();
                if (!string.IsNullOrWhiteSpace(value))
                    return value.Trim();
                Console.WriteLine("Поле не может быть пустым.");
            }
        }

        private static decimal AskDecimal(string prompt, decimal minExclusive, decimal maxInclusive)
        {
            while (true)
            {
                Console.Write(prompt);
                decimal value;
                string raw = ReadLineOrExit().Replace(',', '.');
                if (decimal.TryParse(raw, NumberStyles.Number, CultureInfo.InvariantCulture, out value)
                    && value > minExclusive && value <= maxInclusive)
                    return value;
                Console.WriteLine("Введите число больше {0} и не больше {1}.", minExclusive, maxInclusive);
            }
        }

        private static string ReadLineOrExit()
        {
            string line = Console.ReadLine();
            if (line == null)
                Environment.Exit(0);
            char bom = (char)0xFEFF;
            return line.Trim().Trim(bom).Trim();
        }
    }
}
