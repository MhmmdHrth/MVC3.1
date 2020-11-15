using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmployeeManagement.Models;
using EmployeeManagement.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.Controllers
{
    public class HomeController : Controller
    {
        private readonly IEmployeeRepository _employeeRepository;

        public HomeController(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
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
        public RedirectToActionResult Create(Employee employee)
        {
            Employee newEmployee = _employeeRepository.Add(employee);
            return RedirectToAction(nameof(Details), new { id = newEmployee.Id });
        }
    }
}
