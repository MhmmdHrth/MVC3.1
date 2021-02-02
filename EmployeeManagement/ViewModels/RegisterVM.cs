using EmployeeManagement.Utilities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.ViewModels
{
    public class RegisterVM
    {
        [Required]
        [EmailAddress]
        [Remote(controller:"Account",action: "IsEmailInUse")]
        [ValidEmailDomainAttribute("gmail.com", ErrorMessage = "Email domain must be gmail.com")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare(nameof(Password), ErrorMessage = "Password and confirmation password do not match!")]
        public string ConfirmPassword { get; set; }

        public string City { get; set; }
    }
}
