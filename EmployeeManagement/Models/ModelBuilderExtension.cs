using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.Models
{
    public static class ModelBuilderExtension
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>()
                        .HasData(
                                    new Employee
                                    {
                                        Id = 1,
                                        Department = DepartmentType.IT,
                                        Email = "harith@gmail.com",
                                        Name = "Harith"
                                    },
                                    new Employee
                                    {
                                        Id = 2,
                                        Department = DepartmentType.HR,
                                        Email = "ahmad@gmail.com",
                                        Name = "Ahmad"
                                    }
                                );
        }
    }
}
