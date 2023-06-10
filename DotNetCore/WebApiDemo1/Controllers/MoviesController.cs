using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using WebApiDemo1.DataModel;
using System.Security.Cryptography;
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
        IMapper _imapper;

        public MoviesController(IMovieRepository movieRepository)
        {
            _movieRepository = movieRepository;

            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<MovieDto, Movie>();
                cfg.CreateMap<Movie, MovieDto>();
            });

            _imapper = configuration.CreateMapper();
        }

        [HttpGet]
        [Route("GetAllMovies")]
        public IActionResult GetAllMovies()
        {
            List<Movie> movies = _movieRepository.GetAllMovies();

            List<MovieDto> movieDto = _imapper.Map<List<Movie>, List<MovieDto>>(movies);

            for (int i = 0; i < movies.Count; i++)
            {
                movieDto[i].Hero = movies[i].ActorName;
                movieDto[i].Heroine = movies[i].ActressName;
            }
            
            if (movieDto.Count > 0)
                return Ok(movieDto);
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
            Movie movie = _movieRepository.GetMovieById(Id);

            //Approach 1 Without AutoMapper
            MovieDto movieDto = new()
            {
                Id = movie.Id,
                Hero = movie.ActorName,
                Heroine = movie.ActressName,
                Title = movie.Title,
                MovieType = movie.MovieType,
                ReleaseDate = movie.ReleaseDate,
            };
            return Ok(movieDto);
        }

        [HttpGet]
        [Route("GetMovieById_AutoMapper/{Id}")]
        public IActionResult GetMovieById_Automapper(int Id)
        {
            Movie movie = _movieRepository.GetMovieById_Automapper(Id);

            //Approach 2 With AutoMapper
            MovieDto movieDto = _imapper.Map<Movie, MovieDto>(movie);

            movieDto.Hero = movie.ActorName;
            movieDto.Heroine = movie.ActressName;

            return Ok(movieDto);
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
            //Approach : 1
            //Movie movie = new()
            //{
            //   Id = movieDto.Id,
            //   ActorName = movieDto.ActorName,
            //   ActressName = movieDto.ActressName,
            //   Title = movieDto.Title,
            //   MovieType = movieDto.MovieType,
            //   ReleaseDate = movieDto.ReleaseDate,
            //};

            //Approach : 2
            //var configuration = new MapperConfiguration(cfg =>
            //{
            //    cfg.CreateMap<MovieDto, Movie>();
            //});
            
            //_imapper = configuration.CreateMapper();

            try
            {
                string errormessage = validateMovieAddOrUpdate(movieDto);
                if (!string.IsNullOrEmpty(errormessage))
                    return BadRequest(errormessage);

                if (ModelState.IsValid)
                {
                    Movie movie = _imapper.Map<MovieDto, Movie>(movieDto);
                    movieDto.Id = _movieRepository.Add(movie);

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
            //Movie movie = new()
            //{
            //    Id = movieDto.Id,
            //    ActorName = movieDto.ActorName,
            //    ActressName = movieDto.ActressName,
            //    Title = movieDto.Title,
            //    MovieType = movieDto.MovieType,
            //    ReleaseDate = movieDto.ReleaseDate,
            //};
            try
            {
                string errormessage = validateMovieAddOrUpdate(movieDto, true);
                if (!string.IsNullOrEmpty(errormessage))
                    return BadRequest(errormessage);

                if (ModelState.IsValid)
                {
                    Movie movie = _imapper.Map<MovieDto, Movie>(movieDto);
                    _movieRepository.Update(movie);
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

            movieDto.Hero = movieDto.Hero.Trim();
            movieDto.Heroine = movieDto.Heroine.Trim();
            movieDto.Title = movieDto.Title.Trim();

            if (string.IsNullOrEmpty(movieDto.Hero))
                errorMessage = "Actor Name Can Not Be Blank";

            else if (movieDto.Hero.Length >= 20)
                errorMessage = "Actor Name Should Be Under 20 Characters";

            else if (string.IsNullOrEmpty(movieDto.Heroine))
                errorMessage = "Actress Name Can Not Be Blank";

            else if (movieDto.Heroine.Length >= 20)
                errorMessage = "Actress Name Should Be Under 20 Characters";

            else if (string.IsNullOrEmpty(movieDto.Title))
                errorMessage = "Title Can Not Be Blank";

            else if (!Enum.IsDefined(typeof(MovieTypes), movieDto.MovieType))
                errorMessage = "Invalid Movie Type";

            return errorMessage;
        }
    }
}
