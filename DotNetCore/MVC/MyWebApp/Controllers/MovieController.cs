using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MyWebApp.ViewModels;
using MyWebApp.Repositories;
using MyWebApp.DataModel;
using AutoMapper;

namespace MyWebApp.Controllers
{
    public class MovieController : Controller
    {
        IMovieRepository _movieRepository;
        IMapper _imapper;

        public MovieController(IMovieRepository movieRepository)
        {
            _movieRepository = movieRepository;

            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Actors, ActorsVm>();
                cfg.CreateMap<Actress, ActressVm>();
                cfg.CreateMap<AddVm, Add>();
                cfg.CreateMap<Movie, MovieVm>();
            });

            _imapper = configuration.CreateMapper();
        }
        
        public IActionResult Index()
        {
            List<Movie> movies = _movieRepository.GetAll();

            List<MovieVm> movieResult = _imapper.Map<List<Movie>, List<MovieVm>>(movies);

            return View(movieResult);
        }

        public IActionResult View(int id)
        {
            Movie movie = _movieRepository.Get(id);

            MovieVm movieResult = _imapper.Map<Movie, MovieVm>(movie);

            return View(movieResult);
        }

        public IActionResult Edit(int id)
        {
            Movie movie = _movieRepository.Get(id);

            MovieVm movieResult = _imapper.Map<Movie, MovieVm>(movie);

            ViewBag.actors = new SelectList(GetAllActors(), "Id", "ActorName");
            return View(movieResult);
        }

        [HttpPost]
        public IActionResult Update(MovieVm movie) 
        {
            int affectedRowCount = _movieRepository.Update(movie);

            if(affectedRowCount > 0) 
            {
                TempData["SuccessMessageForUpdate"] = "Movie Update Successful";
            }
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            int deletedRow = _movieRepository.Delete(id);
            if (deletedRow > 0)
            {
                TempData["SuccessMessageForDelete"] = "Movie Record Delete Successful";
            }
            return RedirectToAction("Index");
        }

        public IActionResult Add()
        {
            List<Actors> actors = _movieRepository.GetAllActors();
            ViewBag.actors = new SelectList(actors, "Id", "ActorName");

            List<Actress> actresses = _movieRepository.GetAllActresses();
            ViewBag.actresses = new SelectList(actresses, "Id", "ActressName");

            return View();
        }

        [HttpPost]
        public IActionResult Add(AddVm addVm)
        {
            Add add = _imapper.Map<AddVm, Add>(addVm);
            int affectedRecordCount = _movieRepository.Add(add);

            if (affectedRecordCount > 0)
            {
                TempData["SuccessMessageForAdd"] = "Movie Add Successful";
            }
            return RedirectToAction("Index", addVm);
        }

        private List<ActorsVm> GetAllActors()
        {
            List<Actors> actors = _movieRepository.GetAllActors();

            List<ActorsVm> actorResult = _imapper.Map<List<Actors>, List<ActorsVm>>(actors);
            return actorResult;
        }

        private List<ActressVm> GetAllActresses()
        {
            List<Actress> actresses = _movieRepository.GetAllActresses();

            List<ActressVm> actressResult = _imapper.Map<List<Actress>, List<ActressVm>>(actresses);
            return actressResult;
        }
    }
}
