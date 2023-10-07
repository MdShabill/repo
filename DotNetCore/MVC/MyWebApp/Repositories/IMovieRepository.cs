using MyWebApp.DataModel;
using MyWebApp.ViewModels;
using System.Composition.Convention;

namespace MyWebApp.Repositories
{
    public interface IMovieRepository
    {
        public List<Movie> GetAll();
        public Movie Get(int id);
        public List<Actors> GetAllActors();
        public List<Actress> GetAllActresses();
        public int Delete(int id);
        public int Add(Add add);
        public int Update(MovieVm movie);
        public List<Actors> GetActorDetails();
    }
}
