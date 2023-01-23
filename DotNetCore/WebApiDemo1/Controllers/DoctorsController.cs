using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System.Data;
using WebApiDemo1.DTO.InputDTO;

namespace WebApiDemo1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorsController : ControllerBase
    {
        public readonly IConfiguration _Configuration;
        SqlConnection sqlConnection;

        public DoctorsController(IConfiguration configuration)
        {
            _Configuration = configuration;
            sqlConnection = new(_Configuration.GetConnectionString("DoctorDBConnection").ToString());
        }

        [HttpGet]
        [Route("GetAllDoctors")]
        public IActionResult GetAllDoctors()
        {
            SqlDataAdapter sqlDataAdapter = new("Select * From Doctors", sqlConnection);
            DataTable dataTable = new();
            sqlDataAdapter.Fill(dataTable);
            DataTable copyDataTable = dataTable.Copy();
            sqlDataAdapter.Fill(copyDataTable);

            if (dataTable.Rows.Count > 0)
            {
                return Ok(JsonConvert.SerializeObject(copyDataTable));
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet]
        [Route("GetDoctorsCount")]
        public IActionResult GetDoctorsCount()
        {
            string sqlQuery = "Select Count(*) From Doctors";
            SqlCommand sqlCommand = new(sqlQuery, sqlConnection);
            sqlConnection.Open();
            int doctorcount = Convert.ToInt32(sqlCommand.ExecuteScalar());
            sqlConnection.Close();

            return Ok(doctorcount);
        }

        [HttpGet]
        [Route("GetDoctorDepartmentById/{doctorId}")]
        public IActionResult GetDoctorDepartmentById(int doctorId)
        {
            if (doctorId < 1)
            {
                return BadRequest("Doctor id should be greater than 0");
            }

            string sqlQuery = "Select Department From Doctors where id = @doctorId";
            SqlCommand sqlCommand = new(sqlQuery, sqlConnection);
            sqlCommand.Parameters.AddWithValue("@doctorId", doctorId);
            sqlConnection.Open();
            string doctorDepartment = Convert.ToString(sqlCommand.ExecuteScalar());
            string subStringDoctorDepartment = doctorDepartment.Substring(4);
            sqlConnection.Close();

            return Ok(subStringDoctorDepartment);
        }

        [HttpGet]
        [Route("GetDoctorsDetailByFullNameByDepartment/{fullName}/{department}")]
        public IActionResult GetDoctorsDetailByFullNameByDepartment(string fullName, string department)
        {
            if (string.IsNullOrWhiteSpace(fullName))
            {
                return BadRequest("Doctor full name can not be blank");
            }
            fullName = fullName.Trim();
            if (fullName.Length < 3 || fullName.Length > 20)
            {
                return BadRequest("Doctor full name should be between 3 and 20 characters");
            }

            if (string.IsNullOrWhiteSpace(department))
            {
                return BadRequest("Department name can not be blank");
            }
            department = department.Trim();
            if (department.Length < 3 || department.Length > 20)
            {
                return BadRequest("Department name should be between 3 and 20 characters");
            }

            SqlDataAdapter sqlDataAdapter = new(@"Select * From Doctors Where FullName = @fullName
                                                  And Department = @department", sqlConnection);
            sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@fullName", fullName);
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
        [Route("GetDoctorsDetailByDepartmentByCity/{department}/{city?}")]
        public IActionResult GetDoctorsDetailByDepartmentByCity(string department, string? city)
        {
            city = city.Trim();
            if (city.Length < 3 || city.Length > 20)
            {
                return BadRequest("City name should be between 3 and 20 characters");
            }

            if (string.IsNullOrWhiteSpace(department))
            {
                return BadRequest("Department name can not be blank");
            }
            department = department.Trim();
            if (department.Length < 3 || department.Length > 20)
            {
                return BadRequest("Department name should be between 3 and 20 characters");
            }

            string sqlQuery = "Select * From Doctors Where Department = @department ";

            if (!string.IsNullOrWhiteSpace(city))
            {
                sqlQuery += "And City = @city ";
            }

            SqlDataAdapter sqlDataAdapter = new(sqlQuery, sqlConnection);
            sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@department", department);
            sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@city", city);
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
        [Route("DoctorAdd")]
        public IActionResult DoctorAdd([FromBody] DoctorDto doctor)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (string.IsNullOrWhiteSpace(doctor.FullName))
                    {
                        return BadRequest("Doctor fullName can not be blank");
                    }
                    doctor.FullName = doctor.FullName.Trim();
                    string completeFullName = doctor.FullName.Insert(0, "Md.");
                    string completeFullName1 = completeFullName.Replace("Md.", "Janab");
                    if (completeFullName1.Length < 3 || completeFullName1.Length > 20)
                    {
                        return BadRequest("Doctor full name should be between 3 and 20 characters");
                    }

                    if (string.IsNullOrWhiteSpace(doctor.Department))
                    {
                        return BadRequest("Department name can not be blank");
                    }
                    doctor.Department = doctor.Department.Trim();
                    if (doctor.Department.Length < 3 || doctor.Department.Length > 20)
                    {
                        return BadRequest("Department name should be between 3 and 20 characters");
                    }

                    if (string.IsNullOrWhiteSpace(doctor.Gender))
                    {
                        return BadRequest("Gender can not be blank");
                    }

                    doctor.City = doctor.City.Trim();
                    if (doctor.City.Length < 3 || doctor.City.Length > 20)
                    {
                        return BadRequest("City name should be between 3 and 20 characters");
                    }

                    string sqlQuery = @"INSERT INTO Doctors(FullName, Department, Gender, City)
                                        VALUES (@FullName, @Department, @Gender, @City)
                                        Select Scope_Identity() ";

                    SqlCommand sqlCommand = new(sqlQuery, sqlConnection);
                    sqlCommand.Parameters.AddWithValue("@FullName", completeFullName1);
                    sqlCommand.Parameters.AddWithValue("@Department", doctor.Department);
                    sqlCommand.Parameters.AddWithValue("@Gender", doctor.Gender);
                    sqlCommand.Parameters.AddWithValue("@City", doctor.City);
                    sqlConnection.Open();
                    doctor.Id = Convert.ToInt32(sqlCommand.ExecuteScalar());
                    sqlConnection.Close();

                    return Ok(doctor.Id);
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
