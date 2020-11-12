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

        public string Index()
        {
            return _employeeRepository.GetEmployee(1).Department;
        }

        public ViewResult Details()
        {
            HomeDetailsVM homeDetailsVM = new HomeDetailsVM
            {
                Employee = _employeeRepository.GetEmployee(1),
                PageTitle = "Details Page Title"
            };

            return View(homeDetailsVM);
        }
    }
}
