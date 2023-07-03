using MyWebApp.Models;
using System.Composition.Convention;

namespace MyWebApp.Repositories
{
    public interface IMovieRepository
    {
        public List<Movie> GetAll();
        public int Add(Movie movie);
        public List<Actors> GetActorDetails();
    }
}
