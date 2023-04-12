using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using WebApiDemo1.DataModel;
using WebApiDemo1.DTO.InputDTO;
using WebApiDemo1.Enums;
using Newtonsoft.Json;
using WebApiDemo1.Repositories;

namespace WebApiDemo1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovieController : ControllerBase
    {
        IMovieRepository _movieRepository;

        public MovieController(IMovieRepository movieRepository)
        {
            _movieRepository = movieRepository;
        }

        [HttpGet]
        [Route("GetAllMovies")]
        public IActionResult GetAllMovies()
        {
            DataTable dataTable = _movieRepository.GetAllMovies();
            if (dataTable.Rows.Count > 0)
                return Ok(JsonConvert.SerializeObject(dataTable));
            else
                return NotFound();
        }

        [HttpGet]
        [Route("GetMovieCount")]
        public IActionResult GetMovieCount()
        {
            DataTable dataTable = _movieRepository.GetMovieCount();

            int numberOfRecords = dataTable.Rows.Count;
            return Ok (numberOfRecords);
        }
        
        [HttpGet]
        [Route("DeleteRecord/{Id}")]
        public IActionResult DeleteRecord(int id) 
        {
            MovieDto movieDto = _movieRepository.DeleteRecord(id);
            return Ok(movieDto);
        }
        
        [HttpPost]
        [Route("Add")]
        public IActionResult Add([FromBody] MovieDto movieDto)
        {
            try
            {
                string errormessage = validateMovieAddOrUpdate(movieDto);
                if(!string.IsNullOrEmpty(errormessage)) 
                    return BadRequest(errormessage);
        
                if (ModelState.IsValid)
                {
                    movieDto.Id = _movieRepository.Add(movieDto);

                    return Ok(movieDto.Id);
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
                string errormessage = validateMovieAddOrUpdate(movieDto, true);
                if (!string.IsNullOrEmpty(errormessage))
                    return BadRequest(errormessage);
        
                if (ModelState.IsValid)
                {
                    _movieRepository.Update(movieDto);
                    
                    return Ok("Record Update");
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

        private string validateMovieAddOrUpdate(MovieDto movieDto, bool IsUpdate = false)
        {
            string errorMessage = "";

            if(IsUpdate == true) 
            {
                if (movieDto.Id < 1)
                    errorMessage = "Movie Id Can Not Be Less Then Zero";
            }

            movieDto.ActorName = movieDto.ActorName.Trim();
            movieDto.ActressName = movieDto.ActressName.Trim();
            movieDto.Title = movieDto.Title.Trim();

            if (string.IsNullOrEmpty(movieDto.ActorName))
                errorMessage = "Actor Name can not be blank";

            else if (movieDto.ActorName.Length >= 20)
                errorMessage = "Actor Name should be under 20 characters";

            else if (string.IsNullOrEmpty(movieDto.ActressName))
                errorMessage = "Actress Name can not be blank";

            else if (movieDto.ActressName.Length >= 20)
                errorMessage = "Actress Name should be under 20 characters";

            else if (string.IsNullOrEmpty(movieDto.Title))
                errorMessage = "Title can not be blank";

            else if (!Enum.IsDefined(typeof(MovieTypes), movieDto.MovieType))
                errorMessage = "Invalid Movie Type";

            return errorMessage;
        }
    }
}
