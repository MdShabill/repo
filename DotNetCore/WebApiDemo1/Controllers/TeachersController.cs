using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using System.Configuration;
using System.Data;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
//using WebApiDemo2.DTO;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using WebApiDemo1.DTO.InputDTO;
using WebApiDemo1.DTO;
using Microsoft.VisualBasic;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeachersController : ControllerBase
    {
        public readonly IConfiguration _Configuration;
        SqlConnection sqlConnection;

        public TeachersController(IConfiguration configuration)
        {
            _Configuration = configuration;
            sqlConnection = new SqlConnection(_Configuration.GetConnectionString("TeacherDBConnection").ToString());
        }

        [HttpGet]
        [Route("GetTeachers")]
        public IActionResult GetTeachers()
        {
            SqlDataAdapter sqlDataAdapter = new("SELECT * FROM Teachers", sqlConnection);
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
        [Route("GetTeachersCount")]
        public IActionResult GetTeachersCount()
        {
            string sqlQuery = "SELECT COUNT(*) FROM Teachers";

            var sqlCommand = new SqlCommand(sqlQuery, sqlConnection);

            sqlConnection.Open();
            int customerCount = Convert.ToInt32(sqlCommand.ExecuteScalar());
            sqlConnection.Close();

            return Ok(customerCount);
        }

        [HttpGet]
        [Route("GetTeachersDetail/{teacherId}")]
        public IActionResult GetTeachersDetailById(int teacherId)
        {
            if (teacherId < 1)
            {
                return BadRequest("TeacherId Id should be greater than 0");
            }
            SqlDataAdapter sqlDataAdapter = new("SELECT * FROM Teachers WHERE Id = @teacherId", sqlConnection);

            sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@teacherId", teacherId);

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
        [Route("GetTeachersByDepartmentByTeacherName/{teacherName}/{department?}")]
        public IActionResult GetTeachersByDepartmentByTeacherName(string teacherName, string? department)
        {
            if (string.IsNullOrWhiteSpace(department))
            {
                return BadRequest("Department can not be blank");
            }
            department= department.Trim();
            if (department.Length < 3 || department.Length > 30)
            {
                return BadRequest("Department should be between 3 and 30 characters.");
            }

            string sqlQuery = "SELECT * FROM Teachers Where FullName = @teacherName";
            if(!string.IsNullOrWhiteSpace(department))
            {
                sqlQuery += "And Department = @department";
            }

            SqlDataAdapter sqlDataAdapter = new(sqlQuery, sqlConnection);
            sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@teacherName", teacherName);
            sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@department", department);

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
        [Route("GetTeachersBySalaryRange/{minimumSalary}/{maximumSalary}")]
        public IActionResult GetTeachersBySalaryRange(int minimumSalary, int maximumSalary)
        {
            if (maximumSalary < minimumSalary)
            {
                return BadRequest("Maximum salary cannot be smaller than minimum salary");
            }
            SqlDataAdapter sqlDataAdapter = new(@" SELECT * FROM Teachers 
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
        [Route("TeacherAdd")]
        public IActionResult TeacherAdd([FromBody] TeacherDto teacher)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(teacher.FullName))
                {
                    return BadRequest("Teacher fullName can not be blank");
                }
                teacher.FullName = teacher.FullName.Trim();
                if (teacher.FullName.Length < 3 || teacher.FullName.Length > 20 )
                {
                    return BadRequest("Teacher name should be between 3 and 20 characters");
                }

                if (teacher.Age <= 25)
                {
                    return BadRequest("Invalid age, Teacher age should be above 25");
                }

                if (string.IsNullOrWhiteSpace(teacher.Gender))
                {
                    return BadRequest("Teacher gender can not be blank");
                }

                if (teacher.Salary < 25000)
                {
                    return BadRequest("Invalid salary, Teacher salary should be above 25000");
                }

                if (ModelState.IsValid)
                {
                    string sqlQuery = @"INSERT INTO Teachers(FullName, Age, Gender, SchoolName, Department, Salary)
                                        VALUES (@FullName, @Age, @Gender, @SchoolName, @Department, @Salary)
                                        Select Scope_Identity() ";

                    var sqlCommand = new SqlCommand(sqlQuery, sqlConnection);
                    sqlCommand.Parameters.AddWithValue("@FullName", teacher.FullName);
                    sqlCommand.Parameters.AddWithValue("@Age", teacher.Age);
                    sqlCommand.Parameters.AddWithValue("@Gender", teacher.Gender);
                    sqlCommand.Parameters.AddWithValue("@SchoolName", teacher.SchoolName);
                    sqlCommand.Parameters.AddWithValue("@Department", teacher.Department);
                    sqlCommand.Parameters.AddWithValue("@Salary", teacher.Salary);
                   
                    sqlConnection.Open();
                    teacher.Id = Convert.ToInt32(sqlCommand.ExecuteScalar());
                    sqlConnection.Close();

                    return Ok(teacher.Id);
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
