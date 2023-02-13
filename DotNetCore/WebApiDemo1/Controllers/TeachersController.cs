using Microsoft.AspNetCore.Mvc;
using WebApiDemo1.DTO.InputDTO;
using WebApiDemo1.Repositories;
using System.Text.RegularExpressions;
using WebApiDemo1.Enums;
using Microsoft.Data.SqlClient;

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
            List<TeacherDto> teachers = _teacherRepository.GetAllTeachersAsList();

            if (teachers.Count > 0)
                return Ok(teachers);
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
        [Route("GetTeacherDetailById/{Id}")]
        public IActionResult GetTeacherDetailById(int id)
        {
            TeacherDto teachers = _teacherRepository.GetTeacherDetailById(id);

            if (teachers is not null)
                return Ok(teachers);
            else
                return NotFound("No Record Found for given id");
        }

        [HttpGet]
        [Route("GetTeachersByDepartmentByTeacherName/{FullName}/{department?}")]
        public IActionResult GetTeachersByDepartmentByTeacherName(string fullName, string? department)
        {
            department = department.Trim();

            if (string.IsNullOrWhiteSpace(department))
                return BadRequest("Department can not be blank");

            if (department.Length < 3 || department.Length > 30)
                return BadRequest("Department should be between 3 and 30 characters.");

            List<TeacherDto> teachers = _teacherRepository.GetTeachersByDepartmentByTeacherName(fullName, department);

            if (teachers.Count > 0)
                return Ok(teachers);
            else
                return NotFound();
        }

        [HttpGet]
        [Route("GetTeachersBySalaryRange/{minimumSalary}/{maximumSalary}")]
        public IActionResult GetTeachersBySalaryRange(int minimumSalary, int maximumSalary)
        {
            if (maximumSalary < minimumSalary)
                return BadRequest("Maximum salary cannot be less than minimum salary");

            List<TeacherDto> teachers = _teacherRepository.GetTeachersBySalaryRange(minimumSalary, maximumSalary);

            if (teachers.Count > 0)
                return Ok(teachers);
            else
                return NotFound();
        }

        [HttpPost]
        [Route("Add")]
        public IActionResult Add([FromBody] TeacherDto teacher)
        {
            try
            {
                string errorMessage = validateTeacherAddOrUpdate(teacher);
                if (!string.IsNullOrEmpty(errorMessage))
                    return BadRequest(errorMessage);

                if (ModelState.IsValid)
                {
                    int id = _teacherRepository.Add(teacher);
                    return Ok(id);
                }
                return BadRequest();
            }
            catch (SqlException ex)
            {
                if (ex.Number == 2627)
                {
                    if (ex.Message.Contains("UQ_Teachers_Email"))
                        return BadRequest("Email already exist");

                    if (ex.Message.Contains("UQ_Teachers_MobileNumber"))
                        return BadRequest("Mobile Number already exist");

                    else
                        return BadRequest("Some error at database side");
                }
                else
                    return BadRequest("Some error at database side");
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
        [Route("Update")]
        public IActionResult Update([FromBody] TeacherDto teacher)
        {
            try
            {
                string errorMessage = validateTeacherAddOrUpdate(teacher, true);
                if (!string.IsNullOrEmpty(errorMessage))
                    return BadRequest(errorMessage);

                if (ModelState.IsValid)
                {
                    _teacherRepository.Update(teacher);
                    return Ok("Record updated");
                }
                return BadRequest("Record not updated");
            }
            catch (SqlException ex)
            {
                if (ex.Number == 2627)
                {
                    if (ex.Message.Contains("UQ_Teachers_Email"))
                        return BadRequest("Email already exist");

                    if (ex.Message.Contains("UQ_Teachers_MobileNumber"))
                        return BadRequest("Mobile Number already exist");

                    else
                        return BadRequest("Some error at database side");
                }
                else
                    return BadRequest("Some error at database side");
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

            else if (!Enum.IsDefined(typeof(GenderTypes), teacher.Gender))
                errorMessage = "Invalid Gender";

            if (teacher.Salary < 25000)
                errorMessage = "Invalid salary, Teacher salary should be above 25000";

            return errorMessage;
        }
    }
}
