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
            List<Actors> actors = _movieRepository.GetActorDetails();

            ViewBag.actors = new SelectList(actors, "Id", "ActorName");
            return View();
        }

        [HttpPost]
        public IActionResult Add(Movie movie)
        {
            _movieRepository.Add(movie);

            //ViewBag.SuccessMessageForAdd = "Movie Add Successful";
            TempData["SuccessMessageForAdd"] = "Movie Add Successful";
            
            return RedirectToAction("Index");
        }
    }
}
