using System.ComponentModel.DataAnnotations;

namespace EmployeeManagement.ViewModels
{
    public class CreateRoleVM
    {
        [Required]
        public string RoleName { get; set; }
    }
}