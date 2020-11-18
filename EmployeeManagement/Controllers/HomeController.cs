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
        private readonly IWebHostEnvironment hostingEnvironment;

        public HomeController(IEmployeeRepository employeeRepository, IWebHostEnvironment hostingEnvironment)
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
            Employee employee = _employeeRepository.GetEmployee(id.GetValueOrDefault(1));

            if(employee != null)
            {

                HomeDetailsVM homeDetailsVM = new HomeDetailsVM
                {
                    Employee = employee,
                    PageTitle = "Details Page Title"
                };

                return View(homeDetailsVM);
            }

            HttpContext.Response.StatusCode = 404;
            return View("EmployeeNotFound", id);
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
                string uniqueFileName = ProcessUploadedFile(model);

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

        public IActionResult Edit(int id)
        {
            var objFromDb = _employeeRepository.GetEmployee(id);

            var editObject = new EmployeeEditVM
            {
                Department = objFromDb.Department,
                Email = objFromDb.Email,
                Id = objFromDb.Id,
                Name = objFromDb.Name,
                ExistingPhotoPath = objFromDb.PhotoPath
            };

            return View(editObject);
        }

        [HttpPost]
        public IActionResult Edit(EmployeeEditVM model)
        {
            if (ModelState.IsValid)
            {
                var objFromDb = _employeeRepository.GetEmployee(model.Id);
                objFromDb.Department = model.Department;
                objFromDb.Email = model.Email;
                objFromDb.Name = model.Name;

                if(model.Photos != null)
                {
                    if(model.ExistingPhotoPath != null)
                    {
                        string filePath = Path.Combine
                            (String.Format($"{hostingEnvironment.WebRootPath}/Images/{model.ExistingPhotoPath}"));

                        System.IO.File.Delete(filePath);
                    }
                    objFromDb.PhotoPath = ProcessUploadedFile(model);
                }

                _employeeRepository.Update(objFromDb);
                return RedirectToAction(nameof(Details), new { id = objFromDb.Id });
            }

            return View();
        }

        private string ProcessUploadedFile(EmployeeCreateViewModel model)
        {
            string uniqueFileName = null;
            if (model.Photos != null && model.Photos.Count > 0)
            {
                foreach (var photo in model.Photos)
                {
                    var uploadFolderFile = Path.Combine(hostingEnvironment.WebRootPath, "Images");
                    uniqueFileName = String.Format($"{Guid.NewGuid().ToString()}_{photo.FileName}");

                    string filePath = Path.Combine(uploadFolderFile, uniqueFileName);
                    using(var filestream = new FileStream(filePath, FileMode.Create))
                    {
                        photo.CopyTo(filestream);
                    }
                }
            }

            return uniqueFileName;
        }
    }
}
