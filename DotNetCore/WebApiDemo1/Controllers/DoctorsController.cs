using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System.Data;
using System.Diagnostics.Metrics;
using System.Reflection;
using WebApiDemo1.DTO.InputDTO;
using WebApiDemo1.Repositories;
using WebApplication1.DTO.InputDTO;

namespace WebApiDemo1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorsController : ControllerBase
    {
        IDoctorRepository _doctorRepository;

        public DoctorsController(IDoctorRepository doctorRepository)
        {
            _doctorRepository = doctorRepository;
        }

        [HttpGet]
        [Route("GetAllDoctors")]
        public IActionResult GetAllDoctors()
        {
            DataTable dataTable = _doctorRepository.GetAllDoctors();
            if (dataTable.Rows.Count > 0)
                return Ok(JsonConvert.SerializeObject(dataTable));
            else
                return NotFound();
        }

        [HttpGet]
        [Route("GetDoctorsCount")]
        public IActionResult GetDoctorsCount()
        {
            int doctorcount = _doctorRepository.GetDoctorsCount();
            return Ok(doctorcount);
        }

        [HttpGet]
        [Route("GetDoctorDepartmentById/{doctorId}")]
        public IActionResult GetDoctorDepartmentById(int doctorId)
        {
            if (doctorId < 1)
                return BadRequest("Doctor id should be greater than 0");

            string subStringDoctorDepartment = _doctorRepository.GetDoctorDepartmentById(doctorId);
            return Ok(subStringDoctorDepartment);
        }

        [HttpGet]
        [Route("GetDoctorsDetailByFullNameByDepartment/{fullName}/{department}")]
        public IActionResult GetDoctorsDetailByFullNameByDepartment(string fullName, string department)
        {
            fullName = fullName.Trim();
            department = department.Trim();

            if (string.IsNullOrWhiteSpace(fullName))
                return BadRequest("Doctor full name can not be blank");

            if (fullName.Length < 3 || fullName.Length > 20)
                return BadRequest("Doctor full name should be between 3 and 20 characters");

            if (string.IsNullOrWhiteSpace(department))
                return BadRequest("Department name can not be blank");

            if (department.Length < 3 || department.Length > 20)
                return BadRequest("Department name should be between 3 and 20 characters");

            DataTable dataTable = _doctorRepository.GetDoctorsDetailByFullNameByDepartment(fullName, department);

            if (dataTable.Rows.Count > 0)
                return Ok(JsonConvert.SerializeObject(dataTable));
            else
                return NotFound();
        }

        [HttpGet]
        [Route("GetDoctorsDetailByDepartmentByCity/{department}/{city?}")]
        public IActionResult GetDoctorsDetailByDepartmentByCity(string department, string? city)
        {
            city = city.Trim();
            department = department.Trim();

            if (city.Length < 3 || city.Length > 20)
                return BadRequest("City name should be between 3 and 20 characters");

            if (string.IsNullOrWhiteSpace(department))
                return BadRequest("Department name can not be blank");

            if (department.Length < 3 || department.Length > 20)
                return BadRequest("Department name should be between 3 and 20 characters");

            DataTable dataTable = _doctorRepository.GetDoctorsDetailByDepartmentByCity(department, city);

            if (dataTable.Rows.Count > 0)
                return Ok(JsonConvert.SerializeObject(dataTable));
            else
                return NotFound();
        }

        [HttpPost]
        [Route("DoctorAdd")]
        public IActionResult DoctorAdd([FromBody] DoctorDto doctor)
        {
            try
            {
                string errorMessage = validateDoctorAddOrUpdate(doctor);
                if (!string.IsNullOrEmpty(errorMessage))
                    return BadRequest(errorMessage);

                if (ModelState.IsValid)
                {
                    int id = _doctorRepository.Add(doctor);

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

        private string validateDoctorAddOrUpdate(DoctorDto doctor, bool isUpdate = false)
        {
            string errorMessage = "";

            doctor.FullName = doctor.FullName.Trim();
            doctor.Department = doctor.Department.Trim();
            doctor.City = doctor.City.Trim();

            if (isUpdate == true)
            {
                if (doctor.Id < 1)
                {
                    errorMessage = "Id can not be less than 0";
                }
            }

            if (string.IsNullOrWhiteSpace(doctor.FullName))
                errorMessage = "Doctor fullName can not be blank";

            else if (doctor.FullName.Length < 3 || doctor.FullName.Length > 20)
                errorMessage = "Doctor full name should be between 3 and 20 characters";

            else if (string.IsNullOrWhiteSpace(doctor.Department))
                errorMessage = "Department name can not be blank";

            else if (doctor.Department.Length < 3 || doctor.Department.Length > 20)
                errorMessage = "Department name should be between 3 and 20 characters";

            else if (string.IsNullOrWhiteSpace(doctor.Gender))
                errorMessage = "Gender can not be blank";

            else if (doctor.City.Length < 3 || doctor.City.Length > 20)
                errorMessage = "City name should be between 3 and 20 characters";

            return errorMessage;
        }

        [HttpPost]
        [Route("DoctorUpdate")]
        public IActionResult DoctorUpdate([FromBody] DoctorDto doctor)
        {
            try
            {
                string errorMessage = validateDoctorAddOrUpdate(doctor, true);
                if (!string.IsNullOrEmpty(errorMessage))
                    return BadRequest(errorMessage);

                if (ModelState.IsValid)
                {
                    _doctorRepository.Update(doctor);
                    return Ok("Record updated");
                }
                return BadRequest("Record not updated");
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
