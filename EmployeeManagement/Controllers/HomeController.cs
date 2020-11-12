using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmployeeManagement.Models;
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
            var obj = _employeeRepository.GetEmployee(1);

            ViewData["Employee"] = obj;
            ViewData["PageTitle"] = "Employee Details";

            return View();
        }
    }
}
