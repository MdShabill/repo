using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using WebApiDemo1.DTO.InputDTO;
using WebApiDemo1.Enums;

namespace WebApiDemo1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        public readonly IConfiguration _Configuration;
        SqlConnection sqlConnection;

        public StudentsController(IConfiguration configuration)
        {
            _Configuration = configuration;
            sqlConnection = new(_Configuration.GetConnectionString("SchoolManagementDB").ToString());
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
                    string insertQuery = @"
                    INSERT INTO Students(FullName, Gender, Email, Password, RegistrationDate)
                    VALUES (@FullNae, @Gender, @Email, @Password, @RegistrationDate)";

                    var sqlCommand = new SqlCommand(insertQuery, sqlConnection);
                    sqlCommand.Parameters.AddWithValue("@FullName", studentDto.FullName);
                    sqlCommand.Parameters.AddWithValue("@Gender", studentDto.Gender);
                    sqlCommand.Parameters.AddWithValue("@Email", studentDto.Email);
                    sqlCommand.Parameters.AddWithValue("@Password", studentDto.Password);
                    sqlCommand.Parameters.AddWithValue("@RegistrationDate", DateTime.Now);
                    sqlConnection.Open();
                    sqlCommand.ExecuteNonQuery();
                    sqlConnection.Close();

                    return Ok();
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
                    string updateQuery = @"Update Students
                    Set FullName = @FullName, Gender = @Gender, Email = @Email, Password = @password
                    Where @Id = Id;";
                    
                    var sqlCommand = new SqlCommand(updateQuery, sqlConnection);
                    sqlCommand.Parameters.AddWithValue("@Id", studentDto.Id);
                    sqlCommand.Parameters.AddWithValue("@FullName", studentDto.FullName);
                    sqlCommand.Parameters.AddWithValue("@Gender", studentDto.Gender);
                    sqlCommand.Parameters.AddWithValue("@Email", studentDto.Email);
                    sqlCommand.Parameters.AddWithValue("@Password", studentDto.Password);
                    sqlConnection.Open();
                    sqlCommand.ExecuteNonQuery();
                    sqlConnection.Close();
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
