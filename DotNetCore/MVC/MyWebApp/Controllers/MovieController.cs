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
                cfg.CreateMap<MovieAddVm, Movie>();
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

        //public IActionResult Edit(int id)
        //{
        //    Movie movie = _movieRepository.Get(id);

        //    MovieVm movieResult = _imapper.Map<Movie, MovieVm>(movie);

        //    ViewBag.actors = new SelectList(GetAllActors(), "Id", "ActorName");
        //    return View(movieResult);
        //}

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
            List<Actors> actors = _movieRepository.GetActor();
            ViewBag.Actors = new SelectList(actors, "Id", "ActorName");

            List<Actors> actresses = _movieRepository.GetActress();
            ViewBag.Actresses = new SelectList(actresses, "Id", "ActorName");

            return View();
        }

        [HttpPost]
        public IActionResult Add(MovieAddVm movieAddVm)
        {
            Movie add = _imapper.Map<MovieAddVm, Movie>(movieAddVm);
            int affectedRecordCount = _movieRepository.Add(add);

            if (affectedRecordCount > 0)
            {
                TempData["SuccessMessageForAdd"] = "Movie Add Successful";
            }
            return RedirectToAction("Index", movieAddVm);
        }

        private List<ActorsVm> GetMaleActor()
        {
            List<Actors> maleActors = _movieRepository.GetActor();
            List<ActorsVm> maleActorsVm = _imapper.Map<List<Actors>, List<ActorsVm>>(maleActors);
            return maleActorsVm;
        }

        private List<ActorsVm> GetFemaleActress()
        {
            List<Actors> femaleActresses = _movieRepository.GetActress();
            List<ActorsVm> femaleActressesVm = _imapper.Map<List<Actors>, List<ActorsVm>>(femaleActresses);
            return femaleActressesVm;
        }
    }
}
