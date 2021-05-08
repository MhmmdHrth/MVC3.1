using System.Collections.Generic;
using System.Linq;

namespace EmployeeManagement.Models
{
    public class MockEmployeeRepository : IEmployeeRepository
    {
        private readonly List<Employee> employees;

        public MockEmployeeRepository()
        {
            employees = new List<Employee>
            {
                new Employee {Id = 1,Name="Mary", Department=DepartmentType.HR, Email ="mary@gmail.com"},
                new Employee {Id = 2,Name="John", Department=DepartmentType.IT, Email ="john@gmail.com"},
                new Employee {Id = 3,Name="Sam", Department=DepartmentType.Payroll, Email ="sam@gmail.com"},
            };
        }

        public Employee Add(Employee employee)
        {
            employee.Id = employees.Max(x => x.Id) + 1;
            employees.Add(employee);
            return employee;
        }

        public Employee Delete(int id)
        {
            var employee = employees.FirstOrDefault(x => x.Id == id);

            if (employee != null)
            {
                employees.Remove(employee);
            }

            return employee;
        }

        public IEnumerable<Employee> GetAllEmployee()
        {
            return employees;
        }

        public Employee GetEmployee(int id)
        {
            return employees.FirstOrDefault(x => x.Id == id);
        }

        public Employee Update(Employee employee)
        {
            var obj = employees.FirstOrDefault(x => x.Id == employee.Id);

            if (obj != null)
            {
                obj.Name = employee.Name;
                obj.Department = employee.Department;
                obj.Email = employee.Email;
            }

            return employee;
        }
    }
}