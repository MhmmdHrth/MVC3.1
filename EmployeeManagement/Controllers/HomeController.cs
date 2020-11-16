using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using EmployeeManagement.Models;
using EmployeeManagement.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.Controllers
{
    public class HomeController : Controller
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IHostingEnvironment hostingEnvironment;

        public HomeController(IEmployeeRepository employeeRepository, IHostingEnvironment hostingEnvironment)
        {
            _employeeRepository = employeeRepository;
            this.hostingEnvironment = hostingEnvironment;
        }

        public ViewResult Index()
        {
            var model = _employeeRepository.GetAllEmployee();
            return View(model);
        }

        public ViewResult Details(int? id)
        {
            HomeDetailsVM homeDetailsVM = new HomeDetailsVM
            {
                Employee = _employeeRepository.GetEmployee(id??1),
                PageTitle = "Details Page Title"
            };

            return View(homeDetailsVM);
        }

        [HttpGet]
        public ViewResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(EmployeeCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = null;
                if (model.PhotoPath != null)
                {
                    var uploadFolderFile = Path.Combine(hostingEnvironment.WebRootPath, "Images");
                    uniqueFileName = String.Format($"{Guid.NewGuid().ToString()}_{model.PhotoPath.FileName}");

                    string filePath = Path.Combine(uploadFolderFile, uniqueFileName);
                    model.PhotoPath.CopyTo(new FileStream(filePath, FileMode.Create));
                }

                Employee newEmployee = new Employee
                {
                    Department = model.Department,
                    Email = model.Email,
                    Name = model.Name,
                    PhotoPath = uniqueFileName
                };

                _employeeRepository.Add(newEmployee);
                return RedirectToAction(nameof(Details), new { id = newEmployee.Id });
            }

            return View();
        }
    }
}
