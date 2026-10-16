using System;
using System.Collections.Generic;
using System.Linq;

namespace EmployeeManager
{
    /// <summary>Список сотрудников компании.</summary>
    public class Company
    {
        private readonly List<Employee> _employees = new List<Employee>();

        public IList<Employee> Employees
        {
            get { return _employees.AsReadOnly(); }
        }

        public int Count
        {
            get { return _employees.Count; }
        }

        public void Add(Employee employee)
        {
            if (employee == null)
                throw new ArgumentNullException("employee");
            _employees.Add(employee);
        }

        public Employee GetByIndex(int index)
        {
            if (index < 0 || index >= _employees.Count)
                throw new ArgumentOutOfRangeException("index");
            return _employees[index];
        }

        public bool RemoveByIndex(int index)
        {
            if (index < 0 || index >= _employees.Count)
                return false;
            _employees.RemoveAt(index);
            return true;
        }

        public decimal TotalPayroll
        {
            get { return _employees.Sum(e => e.Salary); }
        }

        public void PrintAll()
        {
            if (_employees.Count == 0)
            {
                Console.WriteLine("Сотрудников пока нет.");
                return;
            }

            Console.WriteLine("№  Сотрудник");
            for (int i = 0; i < _employees.Count; i++)
            {
                Console.WriteLine("{0,2}. {1}", i + 1, _employees[i].GetInfo());
            }
            Console.WriteLine("Фонд оплаты труда: {0:N2} ₽", TotalPayroll);
        }
    }
}
