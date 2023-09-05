using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MyWebApp.ViewModels;
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
            int employeesCount = _employeeRepository.GetAllEmployeesCount();
            if (employeesCount > 0)
            {
                ViewBag.EmployeeCount = employeesCount;
            }

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
            List<Qualification> qualifications = _employeeRepository.GetQualification();

            ViewBag.Qualification = new SelectList(qualifications, "Id", "QualificationName");
           
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
