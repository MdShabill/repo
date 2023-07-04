using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MyWebApp.Models;
using MyWebApp.Repositories;

namespace MyWebApp.Controllers
{
    public class MovieController : Controller
    {
        IMovieRepository _movieRepository;

        public MovieController(IMovieRepository movieRepository)
        {
            _movieRepository = movieRepository;
        }
        
        public IActionResult Index()
        {
            List<Movie> movies = _movieRepository.GetAll();

            return View("Index", movies);
        }

        public IActionResult Add() 
        {
            ViewBag.actors = new SelectList(GetAllActors(), "Id", "ActorName");
            return View();
        }

        [HttpPost]
        public IActionResult Add(Movie movie)
        {
            int affectedRecordCount =_movieRepository.Add(movie);

            if(affectedRecordCount > 0) 
            {
                TempData["SuccessMessageForAdd"] = "Movie Add Successful";
            }
            return RedirectToAction("Index");
        }

        public IActionResult View(int id)
        {
            Movie movie = _movieRepository.Get(id);
            return View(movie);
        }

        public IActionResult Edit(int id)
        {
            Movie movie = _movieRepository.Get(id);

            ViewBag.actors = new SelectList(GetAllActors(), "Id", "ActorName");
            return View(movie);
        }

        [HttpPost]
        public IActionResult Update(Movie movie) 
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

        private List<Actors> GetAllActors()
        {
            List<Actors> actors = _movieRepository.GetAllActors();
            return actors;
        }
    }
}
