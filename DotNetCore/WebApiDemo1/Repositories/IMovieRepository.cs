using System.Data;
using WebApiDemo1.DataModel;
using WebApiDemo1.DTO.InputDTO;

namespace WebApiDemo1.Repositories
{
    public interface IMovieRepository
    {
        public MovieDto GetById(int id);
        public List<MovieDto> GetAllMovies();
        public int GetMovieCount();
        public List<MovieDto> GetMoviesByArtistName(string artistName);
        public List<MovieDto> GetMoviesDetail(string? search, string? sort);
        public void Delete(int id);
        public int Add(MovieDto movieDto);
        public void Update(MovieDto movieDto);
    }
}
