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

        public IActionResult Index()
        {
            List<Employee> employees = _employeeRepository.GetAll();

            string successMessageForAdd = ViewBag.SuccessMessageForAdd;
            if (!string.IsNullOrEmpty(successMessageForAdd))
            {
                ViewBag.SuccessMessageForAdd = successMessageForAdd;
            }

            return View("Index", employees);
        }

        public IActionResult Add()
        {
            //ViewBag
            return View();
        }

        [HttpPost]
        public IActionResult Add(Employee employee) 
        {
            _employeeRepository.Add(employee);

            ViewBag.SuccessMessageForAdd = "Employee Add Successful";
            List<Employee> employees = _employeeRepository.GetAll();

            return View("index", employees);
        }
    }
}
