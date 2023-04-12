using System.Data;
using WebApiDemo1.DataModel;
using WebApiDemo1.DTO.InputDTO;

namespace WebApiDemo1.Repositories
{
    public interface IMovieRepository
    {
        public DataTable GetAllMovies();
        public DataTable GetMovieCount();
        public MovieDto DeleteRecord(int id);
        public int Add(MovieDto movieDto);
        public void Update(MovieDto movieDto);
    }
}
