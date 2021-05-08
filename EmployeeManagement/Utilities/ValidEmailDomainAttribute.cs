using System.ComponentModel.DataAnnotations;

namespace EmployeeManagement.Utilities
{
    public class ValidEmailDomainAttribute : ValidationAttribute
    {
        private readonly string email;

        public ValidEmailDomainAttribute(string email)
        {
            this.email = email;
        }

        public override bool IsValid(object value)
        {
            string emailDomain = value.ToString().Split("@")[1];
            return emailDomain.ToLower() == email.ToLower();
        }
    }
}