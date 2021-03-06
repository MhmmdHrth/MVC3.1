using EmployeeManagement.Models;
using EmployeeManagement.Utilities;
using EmployeeManagement.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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

        public AdministrationController(RoleManager<IdentityRole> roleManager,
                                        UserManager<ApplicationUser> userManager)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
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
    }
}
