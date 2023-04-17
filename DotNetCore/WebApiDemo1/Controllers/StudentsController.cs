using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System.Data;
using WebApiDemo1.DTO.InputDTO;
using WebApiDemo1.Enums;
using WebApiDemo1.Repositories;

namespace WebApiDemo1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        IStudentRepository _studentRepository;

        public StudentsController(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }

        [HttpGet]
        [Route("GetStudentById/{Id}")]
        public IActionResult GetStudentById(int id)
        {
            StudentDto studentDto = _studentRepository.GetStudentById(id);

            if (studentDto != null)
                return Ok(studentDto);
            else
                return NotFound();
        }

        [HttpGet]
        [Route("GetAllStudents")]
        public IActionResult GetAllStudents()
        {
            List<StudentDto> student = _studentRepository.GetAllStudents();

            if (student.Count > 0)
                return Ok(student);
            else
                return NotFound();
        }

        [HttpGet]
        [Route("GetStudentCount")]
        public IActionResult GetStudentCount()
        {
            int studentCount = _studentRepository.GetStudentCount();
            return Ok(studentCount);
        }

        [HttpGet]
        [Route("GetStudentFullNameById/{Id}")]
        public IActionResult GetStudentFullNameById(int id) 
        {
            if(id < 1)
                return BadRequest("Student Id Should be greater Than 0");

            string studentFullName = _studentRepository.GetStudentFullNameById(id);
            return Ok(studentFullName);
        }

        [HttpGet]
        [Route("DeleteStudent/{Id}")]
        public IActionResult DeleteStudent(int id)
        {
            _studentRepository.DeleteStudent(id);
            return Ok("Record Deleted");
        }

        [HttpPost]
        [Route("Register")]
        public IActionResult Register([FromBody] StudentDto studentDto)
        {
            try
            {
                string errorMessage = ValidateStudentRegisterOrUpdate(studentDto);
                    if(!string.IsNullOrEmpty(errorMessage))
                        return BadRequest(errorMessage);

                if (ModelState.IsValid)
                {
                    studentDto.Id = _studentRepository.Add(studentDto);

                    return Ok(studentDto.Id);
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
        [Route("Update")]
        public IActionResult Update([FromBody] StudentDto studentDto)
        {
            try
            {
                string errorMessage = ValidateStudentRegisterOrUpdate(studentDto, true);
                if (!string.IsNullOrEmpty(errorMessage))
                    return BadRequest(errorMessage);

                if (ModelState.IsValid) 
                {
                    _studentRepository.Update(studentDto);
                }
                return Ok("Record Updated");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", @"Unable to save changes. 
                    Try again, and if the problem persists 
                    see your system administrator.");

                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        private string ValidateStudentRegisterOrUpdate(StudentDto studentDto, bool IsUpdate = false)
        {
            string errorMessage = "";

            if(IsUpdate == true)
            {
                if (studentDto.Id < 1)
                    errorMessage = "Student Id can Not Be Less Then Zero";
            }

            studentDto.FullName = studentDto.FullName.Trim();
            studentDto.Email = studentDto.Email.Trim();
            studentDto.Password = studentDto.Password.Trim();

            if (string.IsNullOrWhiteSpace(studentDto.FullName))
                errorMessage = "Full Name can not be blank";

            else if (studentDto.FullName.Length >= 20)
                errorMessage = "FullName should be under 20 characters";

            else if (!Enum.IsDefined(typeof(GenderTypes), studentDto.Gender))
                errorMessage = "Invalid Gender";

            else if (string.IsNullOrWhiteSpace(studentDto.Email))
                errorMessage = "Student Email can not be blank";

            else if (string.IsNullOrWhiteSpace(studentDto.Password))
                errorMessage = "Student Password can not be blank";

            else if (studentDto.Password.Length >= 30)
                errorMessage = "Password should be under 30 characters";

            return errorMessage;
        }
    }
}
