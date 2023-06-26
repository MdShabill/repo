using Microsoft.AspNetCore.Mvc;
using MyWebApp.Models;
using MyWebApp.Repositories;

namespace MyWebApp.Controllers
{
    public class EmployeeController : Controller
    {
        IEmployeeRepository _employeeRepository;

        public EmployeeController(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        //public IActionResult Index()
        //{
        //    return View();
        //}

        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add(Employee employee) 
        {

            if (ModelState.IsValid)
            {
                _employeeRepository.Add(employee);

                return View();

            }
            return View();
        }
    }
}
