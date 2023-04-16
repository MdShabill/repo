using System.Data;
using WebApiDemo1.DataModel;
using WebApiDemo1.DTO.InputDTO;

namespace WebApiDemo1.Repositories
{
    public interface IMovieRepository
    {
        public MovieDto GetMovieById(int id);
        public List<MovieDto> GetAllMovies();

        public DataTable GetMovieCount();
        public void DeleteMovie(int id);
        public int Add(MovieDto movieDto);
        public void Update(MovieDto movieDto);
    }
}
