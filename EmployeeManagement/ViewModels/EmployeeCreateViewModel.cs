using EmployeeManagement.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EmployeeManagement.ViewModels
{
    public class EmployeeCreateViewModel
    {
        [Required, MaxLength(50, ErrorMessage = "Name cannot exceed 50 characters")]
        public string Name { get; set; }

        [Display(Name = "Office Email")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        [Required]
        public string Email { get; set; }

        public DepartmentType? Department { get; set; }

        public List<IFormFile> Photos { get; set; }
    }
}