using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Collections.Specialized;
using System.Data;
using System.Text.RegularExpressions;
using WebApiDemo1.DTO.InputDTO;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        public readonly IConfiguration _Configuration;
        SqlConnection sqlConnection;

        public EmployeesController(IConfiguration configuration)
        {
            _Configuration = configuration;
            sqlConnection = new SqlConnection(_Configuration.GetConnectionString("EmployeeDBConnection").ToString());
        }

        [HttpGet]
        [Route("GetAllEmployees")]
        public IActionResult GetAllEmployees()
        {
            SqlDataAdapter sqlDataAdapter = new("SELECT * FROM Employees", sqlConnection);
            DataTable dataTable = new();
            sqlDataAdapter.Fill(dataTable);

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
            string sqlQuery = "SELECT COUNT(*) FROM Employees ";

            SqlCommand sqlCommand = new(sqlQuery, sqlConnection);

            sqlConnection.Open();
            int employeeCount = Convert.ToInt32(sqlCommand.ExecuteScalar());
            sqlConnection.Close();

            return Ok(employeeCount);
        }

        [HttpGet]
        [Route("GetEmployeeDetailById/{employeeId}")]
        public IActionResult GetEmployeeDetailById(int employeeId)
        {
            if (employeeId < 1)
            {
                return BadRequest("EmployeeId should be greater than 0");
            }
            SqlDataAdapter sqlDataAdapter = new("SELECT * FROM Employees WHERE Id = @employeeId", sqlConnection);

            sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@employeeId", employeeId);

            DataTable dataTable = new();
            sqlDataAdapter.Fill(dataTable);

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
        [Route("GetEmployeesFullNameById/{EmployeeId}")]
        public IActionResult GetEmployeesFullNameById(int employeeId)
        {
            string sqlQuery = "SELECT FullName FROM Employees WHERE Id = @employeeId";

            SqlCommand sqlCommand = new(sqlQuery, sqlConnection);
            sqlCommand.Parameters.AddWithValue("@employeeId", employeeId);

            sqlConnection.Open();
            string employeeFullName = Convert.ToString(sqlCommand.ExecuteScalar());
            sqlConnection.Close();

            return Ok(employeeFullName);
        }

        [HttpGet]
        [Route("GetEmployeesDetail/{gender}/{salary}")]
        public IActionResult GetEmployeesDetailByGenderBySalary(string gender, int salary)
        {
            if (salary < 10000)
            {
                return BadRequest("Please Enter salary above 10000");
            }
            SqlDataAdapter sqlDataAdapter = new(@"SELECT * FROM Employees WHERE Gender = @gender 
                                                   AND Salary > @salary", sqlConnection);

            sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@gender", gender);
            sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@salary", salary);

            DataTable dataTable = new();
            sqlDataAdapter.Fill(dataTable);

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
                return BadRequest("Maximum salary cannot be smaller than minimum salary");
            }
            SqlDataAdapter sqlDataAdapter = new(@"SELECT * FROM Employees 
                                                    WHERE Salary BETWEEN @minimumSalary AND @maximumSalary
                                                    ORDER BY Salary", sqlConnection);

            sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@minimumSalary", minimumSalary);
            sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@maximumSalary", maximumSalary);

            DataTable dataTable = new();
            sqlDataAdapter.Fill(dataTable);

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
                Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
                Match match = regex.Match(employee.Email);

                //# Approach 1
                if (!match.Success)
                {
                    return BadRequest("Email is invalid");
                }

                    ////# Approach 2
                    //if (match.Success == false)
                    //{
                    //return BadRequest("Email is invalid");
                    //}

                    //// Approach 3
                    //if (match.Success != true)
                    //{
                    //return BadRequest("Email is invalid");
                    //}

                    ////# Approach 4
                    //if (match.Success)
                    //{

                    //}
                    //else
                    //{
                    //return BadRequest("Email is invalid");
                    //}

                    ////# Approach 5
                    //if (match.Success == true)
                    //{

                    //}
                    //else
                    //{
                    //return BadRequest("Email is invalid");
                    //}

                if (string.IsNullOrWhiteSpace(employee.FullName))
                {
                    return BadRequest("Name can not be blank");
                }
                employee.FullName = employee.FullName.Trim();
                if (employee.FullName.Length < 3 || employee.FullName.Length > 30)
                {
                    return BadRequest("Name should be between 3 and 30 characters.");
                }

                if (employee.Salary < 8000)
                {
                    return BadRequest("Invalid salary, Employee salary should be above 8000");
                }

                if (ModelState.IsValid)

                    if (ModelState.IsValid)
                    {
                        string sqlQuery = @"INSERT INTO Employees(FullName, Email, Gender, Salary)
                                             VALUES (@FullName, @Email, @Gender, @Salary)
                                             Select Scope_Identity() ";

                        SqlCommand sqlCommand = new(sqlQuery, sqlConnection);
                        sqlCommand.Parameters.AddWithValue("@FullName", employee.FullName);
                        sqlCommand.Parameters.AddWithValue("@Email", employee.Email);
                        sqlCommand.Parameters.AddWithValue("@Gender", employee.Gender);
                        sqlCommand.Parameters.AddWithValue("@Salary", employee.Salary);

                        sqlConnection.Open();
                        employee.Id = Convert.ToInt32(sqlCommand.ExecuteScalar());
                        sqlConnection.Close();


                        return Ok(employee.Id);
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

