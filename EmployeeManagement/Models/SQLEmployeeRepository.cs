using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.Models
{
    public class SQLEmployeeRepository : IEmployeeRepository
    {
        private readonly ApplicationDbContext dbcontext;

        public SQLEmployeeRepository(ApplicationDbContext dbcontext)
        {
            this.dbcontext = dbcontext;
        }

        public Employee Add(Employee employee)
        {
            if(employee != null)
            {
                dbcontext.Employees.Add(employee);
                dbcontext.SaveChanges();
            }

            return employee;
        }

        public Employee Delete(int id)
        {
            var obj = dbcontext.Employees.Find(id);
            if(obj != null)
            {
                dbcontext.Employees.Remove(obj);
                dbcontext.SaveChanges();
            }

            return obj;
        }

        public IEnumerable<Employee> GetAllEmployee()
        {
            var obj = dbcontext.Employees.ToList();
            return obj;
        }

        public Employee GetEmployee(int id)
        {
            var obj = dbcontext.Employees.FirstOrDefault(x => x.Id == id);
            return obj;
        }

        public Employee Update(Employee employee)
        {
            dbcontext.Entry(employee).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            dbcontext.SaveChanges();
            return employee;
        }
    }
}
