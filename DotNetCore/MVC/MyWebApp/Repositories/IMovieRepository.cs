using MyWebApp.Models;
using System.Composition.Convention;

namespace MyWebApp.Repositories
{
    public interface IMovieRepository
    {
        public List<Movie> GetAll();
        public void Add(Movie movie);
        public List<Actors> GetActorDetails();
    }
}
