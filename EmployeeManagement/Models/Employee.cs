using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.Models
{
    public class Employee
    {
        public int Id { get; set; }
        
        [Required, MaxLength(50, ErrorMessage = "Name cannot exceed 50 characters")]
        public string Name { get; set; }

        [Display(Name = "Office Email")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        [Required]
        public string Email { get; set; }

        public DepartmentType? Department { get; set; }

        public string PhotoPath { get; set; }
    }
}
