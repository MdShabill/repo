using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using WebApiDemo1.DTO.InputDTO;
using WebApiDemo1.Enums;

namespace WebApiDemo1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovieController : ControllerBase
    {
        public readonly IConfiguration _Configuration;
        SqlConnection sqlConnection;

        public MovieController(IConfiguration configuration)
        {
            _Configuration = configuration;
            sqlConnection = new(_Configuration.GetConnectionString("MoviesDB").ToString());
        }

        [HttpPost]
        [Route("Add")]
        public IActionResult Add([FromBody] MovieDto movieDto)
        {
            try
            {
                movieDto.ActorName = movieDto.ActorName.Trim();
                movieDto.ActressName = movieDto.ActressName.Trim();
                movieDto.Title = movieDto.Title.Trim();

                if(ModelState.IsValid)
                {
                    if(string.IsNullOrEmpty(movieDto.ActorName)) 
                        return BadRequest("Actor Name can not be blank");

                    if(string.IsNullOrEmpty(movieDto.ActressName))
                        return BadRequest("Actress Name can not be blank");

                    if (string.IsNullOrEmpty(movieDto.Title))
                        return BadRequest("Title can not be blank");

                    if(!Enum.IsDefined(typeof(MovieTypes), movieDto.MovieType))
                        return BadRequest("Invalid Movie Type");

                    string insertQuery = $@"
                    INSERT INTO MOVIES(ActorName, ActressName, Title, MovieType, ReleaseDate)
                    VALUES('{movieDto.ActorName}','{movieDto.ActressName}', '{movieDto.Title}', 
                    {(int)movieDto.MovieType}, GetDate())";

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
        public IActionResult Update([FromBody] MovieDto movieDto)
        {
            try
            {
                movieDto.ActorName = movieDto.ActorName.Trim();
                movieDto.ActressName = movieDto.ActressName.Trim();
                movieDto.Title = movieDto.Title.Trim();

                if (ModelState.IsValid)
                {
                    if (string.IsNullOrEmpty(movieDto.ActorName))
                        return BadRequest("Actor Name can not be blank");

                    if (movieDto.ActorName.Length >= 20)
                        return BadRequest("Actor Name should be under 20 characters");

                    if (string.IsNullOrEmpty(movieDto.ActressName))
                        return BadRequest("Actress Name can not be blank");

                    if (movieDto.ActressName.Length >= 20)
                        return BadRequest("Actress Name should be under 20 characters");

                    if (string.IsNullOrEmpty(movieDto.Title))
                        return BadRequest("Title can not be blank");

                    if (!Enum.IsDefined(typeof(MovieTypes), movieDto.MovieType))
                        return BadRequest("Invalid Movie Type");

                    string updateQuery = $@"Update Movies
                    Set ActorName = '{movieDto.ActorName}', ActressName = '{movieDto.ActressName}',
                    Title = '{movieDto.Title}', MovieType = {(int)movieDto.MovieType}
                    Where Id = {movieDto.Id};";

                    var sqlCommand = new SqlCommand(updateQuery, sqlConnection);
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
    }
}
