using Microsoft.AspNetCore.Mvc;
using System.Configuration;
using System.Data;
using Newtonsoft.Json;
using WebApiDemo1.DTO.InputDTO;
using WebApiDemo1.DTO;
using Microsoft.VisualBasic;
using WebApiDemo1.Repositories;
using System.Text.RegularExpressions;
using WebApiDemo1.Enums;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeachersController : ControllerBase
    {
        ITeacherRepository _teacherRepository;

        public TeachersController(ITeacherRepository teacherRepository)
        {
            _teacherRepository = teacherRepository;
        }

        [HttpGet]
        [Route("GetAllTeachers")]
        public IActionResult GetAllTeachers()
        {
            DataTable dataTable = _teacherRepository.GetAllTeachers();

            if (dataTable.Rows.Count > 0)
                return Ok(JsonConvert.SerializeObject(dataTable));
            else
                return NotFound();
        }

        [HttpGet]
        [Route("GetTeachersCount")]
        public IActionResult GetTeachersCount()
        {
            int teacherCount = _teacherRepository.GetTeachersCount();
            return Ok(teacherCount);
        }

        [HttpGet]
        [Route("GetTeachersDetailById/{teacherId}")]
        public IActionResult GetTeachersDetailById(int teacherId)
        {
            if (teacherId < 1)
                return BadRequest("TeacherId Id should be greater than 0");

            DataTable dataTable = _teacherRepository.GetTeachersDetailById(teacherId);

            if (dataTable.Rows.Count > 0)
                return Ok(JsonConvert.SerializeObject(dataTable));
            else
                return NotFound();
        }

        [HttpGet]
        [Route("GetTeachersByDepartmentByTeacherName/{teacherName}/{department?}")]
        public IActionResult GetTeachersByDepartmentByTeacherName(string teacherName, string? department)
        {
            if (string.IsNullOrWhiteSpace(department))
                return BadRequest("Department can not be blank");
            department= department.Trim();
            if (department.Length < 3 || department.Length > 30)
                return BadRequest("Department should be between 3 and 30 characters.");

            DataTable dataTable = _teacherRepository.GetTeachersByDepartmentByTeacherName(teacherName, department);

            if (dataTable.Rows.Count > 0)
                return Ok(JsonConvert.SerializeObject(dataTable));
            else
                return NotFound();
        }

        [HttpGet]
        [Route("GetTeachersBySalaryRange/{minimumSalary}/{maximumSalary}")]
        public IActionResult GetTeachersBySalaryRange(int minimumSalary, int maximumSalary)
        {
            if (maximumSalary < minimumSalary)
                return BadRequest("Maximum salary cannot be less than minimum salary");

            DataTable dataTable = _teacherRepository.GetTeachersBySalaryRange(minimumSalary, maximumSalary);

            if (dataTable.Rows.Count > 0)
                return Ok(JsonConvert.SerializeObject(dataTable));
            else
                return NotFound();
        }

        [HttpPost]
        [Route("TeacherAdd")]
        public IActionResult TeacherAdd([FromBody] TeacherDto teacher)
        {
            try
            {
                string errorMessage = validateTeacherAddOrUpdate(teacher);
                if (!string.IsNullOrEmpty(errorMessage))
                    return BadRequest(errorMessage);

                if (ModelState.IsValid)
                {
                    int id = _teacherRepository.TeacherAdd(teacher);
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

        [HttpPost]
        [Route("TeacherUpdate")]
        public IActionResult TeacherUpdate([FromBody] TeacherDto teacher)
        {
            try
            {
                string errorMessage = validateTeacherAddOrUpdate(teacher, true);
                if (!string.IsNullOrEmpty(errorMessage))
                    return BadRequest(errorMessage);

                if (ModelState.IsValid)
                {
                    _teacherRepository.TeacherUpdate(teacher);
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

        private string validateTeacherAddOrUpdate(TeacherDto teacher, bool isUpdate = false)
        {
            string errorMessage = "";

            teacher.FullName = teacher.FullName.Trim();

            if (isUpdate == true)
            {
                if (teacher.Id < 1)
                    errorMessage = "Id can not be less than 0";
            }

            if (string.IsNullOrWhiteSpace(teacher.FullName))
                errorMessage = "Teacher fullName can not be blank";

            if (teacher.FullName.Length < 3 || teacher.FullName.Length > 20)
                errorMessage = "Teacher name should be between 3 and 20 characters";

            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            Match match = regex.Match(teacher.Email);
            if (!match.Success)
                errorMessage = "Email is invalid";

            if (teacher.Age <= 25)
                errorMessage = "Invalid age, Teacher age should be above 25";

            else if (!Enum.IsDefined(typeof(GenderType), teacher.Gender))
                errorMessage = "Invalid Gender";

            if (teacher.Salary < 25000)
                errorMessage = "Invalid salary, Teacher salary should be above 25000";

            return errorMessage;
        }
    }
}
