using System.Collections.Generic;

namespace EmployeeManagement.ViewModels
{
    public class UserClaimsVM
    {
        public UserClaimsVM()
        {
            Claims = new List<UserClaim>();
        }

        public string UserId { get; set; }

        public List<UserClaim> Claims { get; set; }
    }
}