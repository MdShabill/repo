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
            sqlConnection = new(_Configuration.GetConnectionString("StudentDBConnection").ToString());
        }

        [HttpPost]
        [Route("Register")]
        public IActionResult Register([FromBody] StudentDto studentDto)
        {
            try
            {
                studentDto.FullName = studentDto.FullName.Trim();
                studentDto.Email = studentDto.Email.Trim();
                studentDto.Password = studentDto.Password.Trim();

                if (ModelState.IsValid)
                {
                    if (string.IsNullOrWhiteSpace(studentDto.FullName))
                        return BadRequest("Full Name can not be blank");

                    if (studentDto.FullName.Length < 3 || studentDto.FullName.Length > 20)
                        return BadRequest ("FullName should be between 3 and 20 characters.");

                    if (! Enum.IsDefined(typeof(GenderTypes), studentDto.Gender))
                        return BadRequest("Invalid Gender");

                    if (string.IsNullOrWhiteSpace(studentDto.Email))
                        return BadRequest("Student Email can not be blank");

                    if (string.IsNullOrWhiteSpace(studentDto.Password))
                        return BadRequest("Student Password can not be blank");

                    string insertQuery = $@"
                    INSERT INTO Students(FullName, Gender, Email, Password, RegistrationDate)
                    VALUES ('{studentDto.FullName}', {(int)studentDto.Gender}, 
                   '{studentDto.Email}', '{studentDto.Password}', GetDate())";

                    var sqlCommand = new SqlCommand(insertQuery, sqlConnection);
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
                if (ModelState.IsValid) 
                {
                    string updateQuery = $@"Update Students
                    Set FullName = '{studentDto.FullName}', Gender = {(int)studentDto.Gender}, 
                    Email = '{studentDto.Email}', Password = '{studentDto.Password}'
                    Where Id = {studentDto.Id};";
                    
                    var sqlCommand = new SqlCommand(updateQuery, sqlConnection);
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
    }
}
