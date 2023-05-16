using WebApiDemo1.DTO.InputDTO;

namespace WebApiDemo1.Repositories
{
    public interface IMovieRepository
    {
        public List<MovieDto> GetAllMovies();
        public int GetMoviesCount();
        public MovieDto GetMovieById(int id);
        public List<MovieDto> GetMoviesByArtistName(string artistName);
        public List<MovieDto> GetMovieDetails(string? searchKeyWord, string? sortColumnName, string? sortOrder, int pageSize);
        public int Add(MovieDto movieDto);
        public void Update(MovieDto movieDto);
    }
}
