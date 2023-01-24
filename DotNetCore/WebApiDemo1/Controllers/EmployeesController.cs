using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Collections.Specialized;
using System.Data;
using System.Diagnostics.Metrics;
using System.Reflection;
using System.Text.RegularExpressions;
using WebApiDemo1.DTO.InputDTO;
using WebApiDemo1.Repositories;
using WebApplication1.DTO.InputDTO;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        public readonly IConfiguration _Configuration;

        public EmployeesController(IConfiguration configuration)
        {
            _Configuration = configuration;
        }

        [HttpGet]
        [Route("GetAllEmployees")]
        public IActionResult GetAllEmployees()
        {
            EmployeeRepository employeeRepository = new(_Configuration);
            DataTable dataTable = employeeRepository.GetAllEmployees();

            if (dataTable.Rows.Count > 0)
            {
                return Ok(JsonConvert.SerializeObject(dataTable));
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet]
        [Route("GetEmployeesCount")]
        public IActionResult GetEmployeesCount()
        {
            EmployeeRepository employeeRepository = new(_Configuration);
            int employeeCount = employeeRepository.GetEmployeesCount();
            return Ok(employeeCount);
        }

        [HttpGet]
        [Route("GetEmployeesFullNameById/{EmployeeId}")]
        public IActionResult GetEmployeesFullNameById(int employeeId)
        {
            if (employeeId < 1)
            {
                return BadRequest("Employee id should be greater than 0");
            }

            EmployeeRepository employeeRepository = new(_Configuration);
            string employeeFullName = employeeRepository.GetEmployeesFullNameById(employeeId);
            return Ok(employeeFullName);
        }

        [HttpGet]
        [Route("GetEmployeesDetailByGenderBySalary/{gender}/{salary}")]
        public IActionResult GetEmployeesDetailByGenderBySalary(string gender, int salary)
        {
            if (salary < 10000)
            {
                return BadRequest("Please Enter salary above 10000");
            }

            EmployeeRepository employeeRepository = new(_Configuration);
            DataTable dataTable = employeeRepository.GetEmployeesDetailByGenderBySalary(gender, salary);

            if (dataTable.Rows.Count > 0)
            {
                return Ok(JsonConvert.SerializeObject(dataTable));
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet]
        [Route("GetEmployeesBySalaryRange/{minimumSalary}/{maximumSalary}")]
        public IActionResult GetEmployeesBySalaryRange(int minimumSalary, int maximumSalary)
        {
            if (maximumSalary < minimumSalary)
            {
                return BadRequest("Maximum salary cannot be less than minimum salary");
            }

            EmployeeRepository employeeRepository = new(_Configuration);
            DataTable dataTable = employeeRepository.GetEmployeesBySalaryRange(minimumSalary, maximumSalary);

            if (dataTable.Rows.Count > 0)
            {
                return Ok(JsonConvert.SerializeObject(dataTable));
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost]
        [Route("EmployeeRegister")]
        public IActionResult EmployeeRegister([FromBody] EmployeeDto employee)
        {
            try
            {
                string errorMessage = ValidateEmployeeRegisterOrUpdate(employee);
                if (!string.IsNullOrEmpty(errorMessage))
                {
                    return BadRequest(errorMessage);
                }

                if (ModelState.IsValid)

                    if (ModelState.IsValid)
                    {
                        EmployeeRepository employeeRepository = new(_Configuration);
                        int id = employeeRepository.Add(employee);
                        return Ok(id);
                    }
                return BadRequest();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", @"Unable to save changes. 
                    Try again, and if the problem persists 
                    see your system administrator.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        private string ValidateEmployeeRegisterOrUpdate(EmployeeDto employee)
        {
            string errorMessage = "";

            employee.FullName = employee.FullName.Trim();
            employee.Gender = employee.Gender.Trim();

            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            Match match = regex.Match(employee.Email);
            if (!match.Success)
            {
                errorMessage = "Email is invalid";
            }
            if (string.IsNullOrWhiteSpace(employee.FullName))
            {
                errorMessage = "Name can not be blank";
            }
            if (employee.FullName.Length < 3 || employee.FullName.Length > 30)
            {
                errorMessage = "FullName should be between 3 and 30 characters.";
            }
            if (employee.Salary < 8000)
            {
                errorMessage = "Invalid salary, employee salary should be above 8000";
            }
            return errorMessage;
        }

        [HttpPost]
        [Route("EmployeeUpdate")]
        public IActionResult EmployeeUpdate([FromBody] EmployeeDto employee)
        {
            try
            {
                string errorMessage = ValidateEmployeeRegisterOrUpdate(employee);
                if (!string.IsNullOrEmpty(errorMessage))
                {
                    return BadRequest(errorMessage);
                }

                if (ModelState.IsValid)
                {
                    EmployeeRepository employeeRepository = new(_Configuration);
                    employeeRepository.Update(employee);
                    return Ok("Record updated");
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", @"Unable to save changes. 
                    Try again, and if the problem persists 
                    see your system administrator.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}


