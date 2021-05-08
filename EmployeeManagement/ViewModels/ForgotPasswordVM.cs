using System.ComponentModel.DataAnnotations;

namespace EmployeeManagement.ViewModels
{
    public class ForgotPasswordVM
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}