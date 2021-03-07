using EmployeeManagement.Models;
using EmployeeManagement.Utilities;
using EmployeeManagement.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.Controllers
{
    [Authorize(Roles = Utility.Role_Admin)]
    public class AdministrationController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ILogger<AdministrationController> logger;

        public AdministrationController(RoleManager<IdentityRole> roleManager,
                                        UserManager<ApplicationUser> userManager,
                                        ILogger<AdministrationController> logger)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
            this.logger = logger;
        }

        [HttpGet]
        public IActionResult CreateRole()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole(CreateRoleVM model)
        {
            if (ModelState.IsValid)
            {
                IdentityRole identityRole = new IdentityRole
                {
                    Name = model.RoleName
                };

                IdentityResult result = await roleManager.CreateAsync(identityRole);
                if (result.Succeeded)
                {
                    return RedirectToAction("ListRoles", "Administration");
                }

                foreach(var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult ListRoles()
        {
            return View(roleManager.Roles);
        } 

        [HttpGet]
        public async Task<IActionResult> EditRole(string id)
        {
           var role = await roleManager.FindByIdAsync(id);

           if(role != null)
            {
                var model = new EditRoleVM
                {
                    Id = role.Id,
                    RoleName = role.Name
                };

               foreach(var user in await userManager.GetUsersInRoleAsync(role.Name))
                {
                    model.Users.Add(user.UserName);
                }

                return View(model);
            }

            ViewBag.ErrorMessage = $"Role with ID:{id} cannot be found";
            return View("NotFound");
        }

        [HttpPost]
        public async Task<IActionResult> EditRole(EditRoleVM model)
        {
            var role = await roleManager.FindByIdAsync(model.Id);

            if (role != null)
            {
                role.Name = model.RoleName;
                var result = await roleManager.UpdateAsync(role);

                if (result.Succeeded)
                {
                    return RedirectToAction("ListRoles");
                }

                foreach(var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return View(model);
            }

            ViewBag.ErrorMessage = $"Role with ID:{model.Id} cannot be found";
            return View("NotFound");
        }

        [HttpGet]
        public async Task<IActionResult> EditUsersInRole(string roleId)
        {
            ViewBag.roleId = roleId;
            var role = await roleManager.FindByIdAsync(roleId);

            if(role != null)
            {
                var model = new List<UserRoleVM>();

                foreach(var user in userManager.Users)
                {

                    var userRoleVm = new UserRoleVM
                    {
                        UserId = user.Id,
                        UserName = user.UserName
                    };

                    if (await userManager.IsInRoleAsync(user, role.Name))
                    {
                        userRoleVm.IsSelected = true;
                    }
                    else
                    {
                        userRoleVm.IsSelected = false;
                    }

                    model.Add(userRoleVm);

                }
                return View(model);

            }

            ViewBag.ErrorMessage = $"Role with Id:{roleId} cannot be found";
            return View("NotFound");
        }

        [HttpPost]
        public async Task<IActionResult> EditUsersInRole(List<UserRoleVM> model, string roleId)
        {
            var role = await roleManager.FindByIdAsync(roleId);

            if (role != null)
            {
                foreach(var item in model)
                {
                    var user = await userManager.FindByIdAsync(item.UserId);

                    if (item.IsSelected && !(await userManager.IsInRoleAsync(user,role.Name)))
                    {
                        await userManager.AddToRoleAsync(user, role.Name);
                    }
                    else
                    {
                        await userManager.RemoveFromRoleAsync(user, role.Name);
                    }
                }

                return RedirectToAction("EditRole", new { id = role.Id });
            }

            ViewBag.ErrorMessage = $"Role with Id:{roleId} cannot be found";
            return View("NotFound");
        }

        [HttpGet]
        public IActionResult ListUsers()
        {
            var users = userManager.Users;
            return View(users);
        }

        [HttpGet]
        public async Task<IActionResult> EditUser(string id)
        {
            var user = await userManager.FindByIdAsync(id);
            var userClaims = await userManager.GetClaimsAsync(user);
            var userRoles = await userManager.GetRolesAsync(user);

            if(user != null)
            {
                var model = new EditUserVM
                {
                    Id = user.Id,
                    Email = user.Email,
                    UserName = user.UserName,
                    City = user.City,
                    Claims = userClaims.Select(x => x.Value).ToList(),
                    Roles = userRoles.ToList()
                };

                return View(model);
            }

            ViewBag.ErrorMessage = $"User with Id = {id} cannot be found";
            return View("NotFound");
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(EditUserVM model)
        {
            var user = await userManager.FindByIdAsync(model.Id);

            if(user != null)
            {
                user.Email = model.Email;
                user.UserName = model.UserName;
                user.City = model.City;

                var result = await userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    return RedirectToAction("ListUsers");
                }

                foreach(var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return View(model);
            }

            ViewBag.ErrorMessage = $"User with Id = {model.Id} cannot be found";
            return View("NotFound");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await userManager.FindByIdAsync(id);

            if(user != null)
            {
                var result = await userManager.DeleteAsync(user);

                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(ListUsers));
                }

                foreach(var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return View(nameof(ListUsers));
            }

            ViewBag.ErrorMessage = $"User with Id = {id} cannot be found";
            return View("NotFound");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteRole(string id)
        {
            var role = await roleManager.FindByIdAsync(id);

            if (role != null)
            {
                try
                {
                    var result = await roleManager.DeleteAsync(role);

                    if (result.Succeeded)
                    {
                        return RedirectToAction(nameof(ListRoles));
                    }

                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }

                    return View(nameof(ListRoles));
                }
                catch(Exception ex)
                {
                    logger.LogError($"Error deleting role {ex}");

                    ViewBag.ErrorTitle = $"{role.Name} role is in use";
                    ViewBag.ErrorMessage = $"{role.Name} role cannot be deleted as there are users in this role." +
                        $" If you want to delete this role," +
                        $" please remove the users from the role and then try to delete";

                    return View("~/Views/Error/Error.cshtml");
                }
            }

            ViewBag.ErrorMessage = $"Role with Id = {id} cannot be found";
            return View("NotFound");
        }

        [HttpGet]
        public async Task<IActionResult> ManageUserRoles(string userId)
        {
            ViewBag.UserId = userId;
            var user = await userManager.FindByIdAsync(userId);

            if(user != null)
            {
                var model = new List<UserRolesVM>();

                foreach(var role in roleManager.Roles)
                {
                    var userRoles = new UserRolesVM
                    {
                        RoleId = role.Id,
                        RoleName = role.Name
                    };

                    if(await userManager.IsInRoleAsync(user, role.Name))
                    {
                        userRoles.IsSelected = true;
                    }
                    else
                    {
                        userRoles.IsSelected = false;
                    }

                    model.Add(userRoles);
                }

                return View(model);
            }

            ViewBag.ErrorMessage = $"User with Id = {userId} cannot be found";
            return View("NotFound");
        }

        [HttpPost]
        public async Task<IActionResult> ManageUserRoles(List<UserRolesVM> model, string userId)
        {
            var user = await userManager.FindByIdAsync(userId);

            if(user != null)
            {
                var roles = await userManager.GetRolesAsync(user);
                var result = await userManager.RemoveFromRolesAsync(user, roles);

                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", "Cannot remove user existing roles");
                    return View(model);
                }

                result = await userManager.AddToRolesAsync(user, model.Where(x => x.IsSelected).Select(y => y.RoleName));

                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", "Cannot add selected roles to user");
                    return View(model);
                }

                return RedirectToAction("EditUser", new { id = userId });
            }

            ViewBag.ErrorMessage = $"User with Id = {userId} cannot be found";
            return View("NotFound");
        }
    }
}
