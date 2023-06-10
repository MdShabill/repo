using WebApiDemo1.DataModel;
using WebApiDemo1.DTO.InputDTO;

namespace WebApiDemo1.Repositories
{
    public interface IMovieRepository
    {
        public List<Movie> GetAllMovies();
        public int GetMoviesCount();
        public Movie GetMovieById(int id);
        public Movie GetMovieById_Automapper(int id);
        public List<MovieDto> GetMoviesByArtistName(string artistName);
        public List<MovieDto> GetMovieDetails(string? searchKeyWord, string? sortColumnName, string? sortOrder, int pageSize);
        public int Add(Movie movie);
        public void Update(Movie movie);
    }
}
