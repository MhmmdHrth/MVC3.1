using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EmployeeManagement.Security
{
    public class CanEditOnlyOtherAdminRolesAndClaimsHandler : AuthorizationHandler<ManageAdminRolesAndClaimsRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ManageAdminRolesAndClaimsRequirement requirement)
        {
            var authFilterContext = context.Resource as AuthorizationFilterContext;

            if (authFilterContext != null)
            {
                string loggedAdminId = context.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;
                string adminIdBeingEdited = authFilterContext.HttpContext.Request.Query["userId"];

                if (context.User.IsInRole("Admin") && context.User.HasClaim(claim => claim.Type == "Edit Role" && claim.Value == "true")
                    && (adminIdBeingEdited.ToLower() != loggedAdminId.ToLower()))
                {
                    context.Succeed(requirement);
                }

                return Task.CompletedTask;
            }

            return Task.CompletedTask;
        }
    }
}