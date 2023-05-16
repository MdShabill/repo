using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApiDemo1.DTO.InputDTO;
using WebApiDemo1.Enums;
using WebApiDemo1.Repositories;

namespace WebApiDemo1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        IMovieRepository _movieRepository;

        public MoviesController(IMovieRepository movieRepository)
        {
            _movieRepository = movieRepository;
        }

        [HttpGet]
        [Route("GetAllMovies")]
        public IActionResult GetAllMovies()
        {
            List<MovieDto> movies = _movieRepository.GetAllMovies();

            if (movies.Count > 0)
                return Ok(movies);
            else
                return NotFound();
        }

        [HttpGet]
        [Route("GetMoviesCount")]
        public IActionResult GetMoviesCount() 
        {
            int movieCount = _movieRepository.GetMoviesCount();
            return Ok(movieCount);
        }

        [HttpGet]
        [Route("GetMovieById/{Id}")]
        public IActionResult GetMovieById(int Id)
        {
            MovieDto movieDto = _movieRepository.GetMovieById(Id);
            
            if (movieDto != null)
                return Ok(movieDto);
            else 
                return NotFound();
        }

        [HttpGet]
        [Route("GetMoviesByArtistName/{ArtistName}")]
        public IActionResult GetMoviesByArtistName(string artistName) 
        {
            if (string.IsNullOrEmpty(artistName))
                return BadRequest("Artist Name Should Not Be Blnk");

            List<MovieDto> movies = _movieRepository.GetMoviesByArtistName(artistName);

            if(movies.Count > 0)
                return Ok(movies);
            else
                return NotFound();
        }

        [HttpGet]
        [Route("GetMovieDetails/{SearchKeyWord}/{SortColumnName}/{SortOrder}/{PageSize}")]
        public IActionResult GetMovieDetails(string? searchKeyWord, string? sortColumn, string? sortOrder, int pageSize)
        {
            List<MovieDto> movieDetails = _movieRepository.GetMovieDetails(searchKeyWord, sortColumn, sortOrder, pageSize);
            if(movieDetails.Count > 0)
                return Ok(movieDetails);
            else
                return NotFound();
        }


        [HttpPost]
        [Route("Add")]
        public IActionResult Add([FromBody] MovieDto movieDto)
        {
            try
            {
                string errormessage = validateMovieAddOrUpdate(movieDto);
                if (!string.IsNullOrEmpty(errormessage))
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
                }
                return Ok("Record Update");
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

            if (IsUpdate == true)
            {
                if (movieDto.Id < 1)
                    errorMessage = "Movie Id Can Not Be Less Then Zero";
            }

            movieDto.ActorName = movieDto.ActorName.Trim();
            movieDto.ActressName = movieDto.ActressName.Trim();
            movieDto.Title = movieDto.Title.Trim();

            if (string.IsNullOrEmpty(movieDto.ActorName))
                errorMessage = "Actor Name Can Not Be Blank";

            else if (movieDto.ActorName.Length >= 20)
                errorMessage = "Actor Name Should Be Under 20 Characters";

            else if (string.IsNullOrEmpty(movieDto.ActressName))
                errorMessage = "Actress Name Can Not Be Blank";

            else if (movieDto.ActressName.Length >= 20)
                errorMessage = "Actress Name Should Be Under 20 Characters";

            else if (string.IsNullOrEmpty(movieDto.Title))
                errorMessage = "Title Can Not Be Blank";

            else if (!Enum.IsDefined(typeof(MovieTypes), movieDto.MovieType))
                errorMessage = "Invalid Movie Type";

            return errorMessage;
        }
    }
}
