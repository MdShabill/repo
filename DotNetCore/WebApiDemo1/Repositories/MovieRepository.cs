using Microsoft.Data.SqlClient;
using System.Data;
using WebApiDemo1.DataModel;
using WebApiDemo1.DTO.InputDTO;
using WebApiDemo1.Enums;

namespace WebApiDemo1.Repositories
{
    public class MovieRepository : IMovieRepository
    {
        private readonly string _connectionString;

        public MovieRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<Movie> GetAllMovies()
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                SqlDataAdapter sqlDataAdapter = new("SELECT * FROM Movies", sqlConnection);
                DataTable dataTable = new();
                sqlDataAdapter.Fill(dataTable);

                List<Movie> movies = new();

                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    Movie movie = new()
                    {
                        Id = (int)dataTable.Rows[i]["Id"],
                        ActorName = (string)dataTable.Rows[i]["ActorName"],
                        ActressName = (string)dataTable.Rows[i]["ActressName"],
                        Title = (string)dataTable.Rows[i]["Title"],
                        MovieType = (MovieTypes)dataTable.Rows[i]["MovieType"],
                        ReleaseDate = (DateTime)dataTable.Rows[i]["ReleaseDate"]
                    };
                    movies.Add(movie);
                }
                return movies;
            }
        }

        public int GetMoviesCount()
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                string sqlQuery = "SELECT Count(*) FROM Movies";
                SqlCommand sqlCommand = new(sqlQuery, sqlConnection);
                sqlConnection.Open();
                int movieCount = Convert.ToInt32(sqlCommand.ExecuteScalar());
                sqlConnection.Close();
                return movieCount;
            }
        }

        public Movie GetMovieById(int id)
        {
            using (SqlConnection sqlConnection =new(_connectionString)) 
            {
                SqlDataAdapter sqlDataAdapter = new("Select * From Movies Where Id = @id", sqlConnection);
                sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@id", id);
                DataTable dataTable= new();
                sqlDataAdapter.Fill(dataTable);

                if (dataTable.Rows.Count > 0)
                {
                    Movie movies = new()
                    {
                        Id = (int)dataTable.Rows[0]["id"],
                        ActorName = (string)dataTable.Rows[0]["ActorName"],
                        ActressName = (string)dataTable.Rows[0]["ActressName"],
                        Title = (string)dataTable.Rows[0]["Title"],
                        MovieType = (MovieTypes)dataTable.Rows[0]["MovieType"],
                        ReleaseDate = (DateTime)dataTable.Rows[0]["ReleaseDate"]
                    };
                    return movies;
                }
                else
                {
                    return null;
                }
            }
        }

        public Movie GetMovieById_Automapper(int id)
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                SqlDataAdapter sqlDataAdapter = new("Select * From Movies Where Id = @id", sqlConnection);
                sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@id", id);
                DataTable dataTable = new();
                sqlDataAdapter.Fill(dataTable);

                if (dataTable.Rows.Count > 0)
                {
                    Movie movies = new()
                    {
                        Id = (int)dataTable.Rows[0]["id"],
                        ActorName = (string)dataTable.Rows[0]["ActorName"],
                        ActressName = (string)dataTable.Rows[0]["ActressName"],
                        Title = (string)dataTable.Rows[0]["Title"],
                        MovieType = (MovieTypes)dataTable.Rows[0]["MovieType"],
                        ReleaseDate = (DateTime)dataTable.Rows[0]["ReleaseDate"]
                    };
                    return movies;
                }
                else
                {
                    return null;
                }
            }
        }

        public List<MovieDto> GetMoviesByArtistName(string artistName)
        {
            using SqlConnection sqlConnection=new(_connectionString);
            {
                SqlDataAdapter sqlDataAdapter = new(@"Select * From Movies Where ActorName LIKE '%' + @artistName + '%'
                        Or ActressName LIKE '%' + @artistName + '%' ", sqlConnection);
                sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@artistName", artistName);
                DataTable dataTable= new();
                sqlDataAdapter.Fill(dataTable);

                List<MovieDto> movies = new();

                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    MovieDto movieDto = new()
                    {
                        Id = (int)dataTable.Rows[i]["id"],
                        Hero = (string)dataTable.Rows[i]["ActorName"],
                        Heroine = (string)dataTable.Rows[i]["ActressName"],
                        Title = (string)dataTable.Rows[i]["Title"],
                        MovieType = (MovieTypes)dataTable.Rows[i]["MovieType"],
                        ReleaseDate = (DateTime)dataTable.Rows[i]["ReleaseDate"]
                    };
                    movies.Add(movieDto);
                }
                return movies;
            }
        }

        public List<MovieDto> GetMovieDetails(string? searchKeyWord, string? sortColumnName, string? sortOrder, int pageSize)
        {
            using(SqlConnection sqlConnection = new(_connectionString)) 
            {
                if (string.IsNullOrEmpty(sortColumnName))
                    sortColumnName = "Title";

                if (string.IsNullOrEmpty(sortOrder))
                    sortOrder = "ASC";

                if(pageSize == 0)
                    pageSize = 10;

                string sqlBasicQuery = $"Select Top {pageSize} * From Movies ";

                if (!string.IsNullOrWhiteSpace(searchKeyWord))
                    sqlBasicQuery += @"Where ActorName LIKE '%' + @search + '%' Or ActressName LIKE '%' + @search + '%'
                           Or Title Like '%' + @search + '%' ";

                SqlDataAdapter sqlDataAdapter = new(sqlBasicQuery, sqlConnection);

                if (!string.IsNullOrWhiteSpace(sortColumnName) || !string.IsNullOrWhiteSpace(sortOrder))
                    sqlBasicQuery += $"Order By {sortColumnName} {sortOrder}";

                if (!string.IsNullOrWhiteSpace(searchKeyWord))
                    sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@search", searchKeyWord);

                DataTable dataTable = new();
                sqlDataAdapter.Fill(dataTable);
  
                List<MovieDto> movieDetails = new();

                for(int i = 0; i < dataTable.Rows.Count; i++)
                {
                    MovieDto movieDto = new()
                    {
                        Id = (int)dataTable.Rows[i]["Id"],
                        Hero = (string)dataTable.Rows[i]["ActorName"],
                        Heroine = (string)dataTable.Rows[i]["ActressName"],
                        Title = (string)dataTable.Rows[i]["Title"],
                        MovieType = (MovieTypes)dataTable.Rows[i]["MovieType"],
                        ReleaseDate = (DateTime)dataTable.Rows[i]["ReleaseDate"]
                    };
                    movieDetails.Add(movieDto);
                }
                return movieDetails;
            }
        }

        public int Add(Movie movie)
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                string insertQuery = @"INSERT INTO MOVIES(ActorName, ActressName, Title, MovieType, ReleaseDate)
                         VALUES(@ActorName, @ActressName, @Title, @MovieType, @ReleaseDate)
                         Select Scope_Identity()";
                var sqlCommand = new SqlCommand(insertQuery, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@ActorName", movie.ActorName);
                sqlCommand.Parameters.AddWithValue("@ActressName", movie.ActressName);
                sqlCommand.Parameters.AddWithValue("@Title", movie.Title);
                sqlCommand.Parameters.AddWithValue("@MovieType", movie.MovieType);
                sqlCommand.Parameters.AddWithValue("@ReleaseDate", DateTime.Now);
                sqlConnection.Open();
                int id = Convert.ToInt32(sqlCommand.ExecuteScalar());
                sqlConnection.Close();
                return id;
            }
        }

        public void Update(Movie movie)
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                string updateQuery = @"Update Movies Set ActorName = @ActorName, ActressName = @ActressName, 
                        Title = @Title, MovieType = @MovieType
                        Where Id = @Id;";
                var sqlCommand = new SqlCommand(updateQuery, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@Id", movie.Id);
                sqlCommand.Parameters.AddWithValue("@ActorName", movie.ActorName);
                sqlCommand.Parameters.AddWithValue("@ActressName", movie.ActressName);
                sqlCommand.Parameters.AddWithValue("@Title", movie.Title);
                sqlCommand.Parameters.AddWithValue("@MovieType", movie.MovieType);
                sqlConnection.Open();
                sqlCommand.ExecuteNonQuery();
                sqlConnection.Close();
            }
        }
    }
}
