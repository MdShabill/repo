using MyWebApp.DataModel;
using MyWebApp.ViewModels;
using System.Composition.Convention;

namespace MyWebApp.Repositories
{
    public interface IMovieRepository
    {
        public List<Movie> GetAll();
        public Movie Get(int id);
        public List<Actors> GetActor();
        public List<Actors> GetActress();
        public int Delete(int id);
        public int Add(Movie movie);
        public int Update(MovieVm movie);
        public List<Actors> GetActorDetails();
    }
}
