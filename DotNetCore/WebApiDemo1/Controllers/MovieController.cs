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
                string errormessage = validateMovieAdd(movieDto);
                if(!string.IsNullOrEmpty(errormessage)) 
                    return BadRequest(errormessage);

                if (ModelState.IsValid)
                {
                    string insertQuery = @"
                    INSERT INTO MOVIES(ActorName, ActressName, Title, MovieType, ReleaseDate)
                    VALUES(@ActorName, @ActressName, @Title, @MovieType, @ReleaseDate)";

                    var sqlCommand = new SqlCommand(insertQuery, sqlConnection);
                    sqlCommand.Parameters.AddWithValue("@ActorName", movieDto.ActorName);
                    sqlCommand.Parameters.AddWithValue("@ActressName", movieDto.ActressName);
                    sqlCommand.Parameters.AddWithValue("@Title", movieDto.Title);
                    sqlCommand.Parameters.AddWithValue("@MovieType", movieDto.MovieType);
                    sqlCommand.Parameters.AddWithValue("@ReleaseDate", DateTime.Now);
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

                    string updateQuery = @"Update Movies
                    Set ActorName = @ActorName, ActressName = @ActressName, Title = @Title, MovieType = @MovieType
                    Where Id = @Id;";

                    var sqlCommand = new SqlCommand(updateQuery, sqlConnection);
                    sqlCommand.Parameters.AddWithValue("@Id", movieDto.Id);
                    sqlCommand.Parameters.AddWithValue("@ActorName", movieDto.ActorName);
                    sqlCommand.Parameters.AddWithValue("@ActressName", movieDto.ActressName);
                    sqlCommand.Parameters.AddWithValue("@Title", movieDto.Title);
                    sqlCommand.Parameters.AddWithValue("@MovieType", movieDto.MovieType);
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

        private string validateMovieAdd(MovieDto movieDto)
        {
            string errorMessage = "";

            movieDto.ActorName = movieDto.ActorName.Trim();
            movieDto.ActressName = movieDto.ActressName.Trim();
            movieDto.Title = movieDto.Title.Trim();

            if (string.IsNullOrEmpty(movieDto.ActorName))
                errorMessage = "Actor Name can not be blank";

            else if (string.IsNullOrEmpty(movieDto.ActressName))
                errorMessage = "Actress Name can not be blank";

            else if (string.IsNullOrEmpty(movieDto.Title))
                errorMessage = "Title can not be blank";

            else if (!Enum.IsDefined(typeof(MovieTypes), movieDto.MovieType))
                errorMessage = "Invalid Movie Type";

            return errorMessage;
        }
    }
}
