using MyWebApp.Models;
using System.Composition.Convention;

namespace MyWebApp.Repositories
{
    public interface IMovieRepository
    {
        public List<Movie> GetAll();
        public Movie Get(int id);
        public List<Actors> GetAllActors();
        public int Delete(int id);
        public int Add(Movie movie);
        public List<Actors> GetActorDetails();
    }
}
