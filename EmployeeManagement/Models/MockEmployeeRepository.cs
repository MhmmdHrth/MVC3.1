﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.Models
{
    public class MockEmployeeRepository : IEmployeeRepository
    {
        private readonly List<Employee> employees;

        public MockEmployeeRepository()
        {
            employees = new List<Employee>
            {
                new Employee {Id = 1,Name="Mary", Department="HR", Email ="mary@gmail.com"},
                new Employee {Id = 2,Name="John", Department="IT", Email ="john@gmail.com"},
                new Employee {Id = 3,Name="Sam", Department="IT", Email ="sam@gmail.com"},
            };
        }

        public Employee GetEmployee(int id)
        {
            return employees.FirstOrDefault(x => x.Id == id);
        }
    }
}