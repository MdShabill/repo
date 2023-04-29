using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
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

        public MovieDto GetById(int id)
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                SqlDataAdapter sqlDataAdapter = new("Select * From Movies Where Id = @id", sqlConnection);
                sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@id", id);
                DataTable dataTable = new();
                sqlDataAdapter.Fill(dataTable);

                ////apporach #1
                if (dataTable.Rows.Count > 0)
                {
                    ////apporach #1
                    MovieDto movieDto = new();
                    movieDto.Id = (int)dataTable.Rows[0]["Id"];
                    movieDto.ActorName = (string)dataTable.Rows[0]["ActorName"];
                    movieDto.ActressName = (string)dataTable.Rows[0]["ActressName"];
                    movieDto.Title = (string)dataTable.Rows[0]["Title"];
                    movieDto.MovieType = (MovieTypes)dataTable.Rows[0]["MovieType"];
                    movieDto.ReleaseDate = (DateTime)dataTable.Rows[0]["ReleaseDate"];

                    ////apporach #2 - Using Object Initializer
                    //MovieDto movieDto = new()
                    //{
                    //    Id = (int)dataTable.Rows[0]["Id"],
                    //    ActorName = (string)dataTable.Rows[0]["ActorName"],
                    //    ActressName = (string)dataTable.Rows[0]["ActressName"],
                    //    Title = (string)dataTable.Rows[0]["Title"],
                    //    MovieType = (MovieTypes)dataTable.Rows[0]["MovieType"],
                    //    ReleaseDate = (DateTime)dataTable.Rows[0]["ReleaseDate"]
                    //};

                    return movieDto;
                }
                else
                {
                    return null;
                }
            }
        }

        public List<MovieDto> GetAllMovies()
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                SqlDataAdapter sqlDataAdapter = new("SELECT * FROM Movies", sqlConnection);
                DataTable dataTable = new();
                sqlDataAdapter.Fill(dataTable);

                List<MovieDto> celebrity = new();

                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    MovieDto movieDto = new()
                    {
                        Id = (int)dataTable.Rows[i]["Id"],
                        ActorName = (string)dataTable.Rows[i]["ActorName"],
                        ActressName = (string)dataTable.Rows[i]["ActressName"],
                        Title = (string)dataTable.Rows[i]["Title"],
                        MovieType = (MovieTypes)dataTable.Rows[i]["MovieType"],
                        ReleaseDate = (DateTime)dataTable.Rows[i]["ReleaseDate"]
                    };
                    celebrity.Add(movieDto);
                }
                return celebrity;
            }
        }

        //TODO: Make Use Count(*) Function
        public int GetMovieCount()
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                string sqlQuery = "SELECT Count(*) FROM Movies";
                SqlCommand sqlCommand= new(sqlQuery, sqlConnection);
                sqlConnection.Open();
                int movieCount = Convert.ToInt32(sqlCommand.ExecuteScalar());
                sqlConnection.Close();
                return movieCount;
            }
        }

        public List<MovieDto> GetMoviesByArtistName(string artistName)
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                SqlDataAdapter sqlDataAdapter = new(@"Select * From Movies Where ActorName LIKE '%' + @artistName + '%'
                                Or ActressName LIKE '%' + @artistName + '%' ", sqlConnection);
                sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@artistName", artistName);
                DataTable dataTable= new();
                sqlDataAdapter.Fill(dataTable);

                List<MovieDto> movie = new();

                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    MovieDto movieDto = new()
                    {
                        Id = (int)dataTable.Rows[i]["Id"],
                        ActorName = (string)dataTable.Rows[i]["ActorName"],
                        ActressName = (string)dataTable.Rows[i]["ActressName"],
                        Title = (string)dataTable.Rows[i]["Title"],
                        MovieType = (MovieTypes)dataTable.Rows[i]["MovieType"],
                        ReleaseDate = (DateTime)dataTable.Rows[i]["ReleaseDate"]
                    };
                    movie.Add(movieDto);
                }
                return movie;
            }
        }

        public List<MovieDto> GetMoviesDetail(string? searchKeyWord, string? sortColumnName, string? sortOrder, int pageSize)
        {
            using SqlConnection sqlConnection= new(_connectionString);
            {
                string sqlBasicQuery = $"Select Top {pageSize} * From Movies ";

                if (!string.IsNullOrWhiteSpace(searchKeyWord))
                    sqlBasicQuery += "Where ActorName LIKE '%' + @search + '%' Or ActressName LIKE '%' + @search + '%' Or Title Like '%' + @search + '%' ";
                                
                sqlBasicQuery += $"Order By {sortColumnName} {sortOrder}";

                SqlDataAdapter sqlDataAdapter = new(sqlBasicQuery, sqlConnection);

                if(!string.IsNullOrWhiteSpace(searchKeyWord))
                    sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@search", searchKeyWord);

                DataTable dataTable = new();
                sqlDataAdapter.Fill(dataTable);

                List<MovieDto> movieDetail = new();

                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    MovieDto movieDto = new()
                    {
                        Id = (int)dataTable.Rows[i]["Id"],
                        ActorName = (string)dataTable.Rows[i]["ActorName"],
                        ActressName = (string)dataTable.Rows[i]["ActressName"],
                        Title = (string)dataTable.Rows[i]["Title"],
                        MovieType = (MovieTypes)dataTable.Rows[i]["MovieType"],
                        ReleaseDate = (DateTime)dataTable.Rows[i]["ReleaseDate"]
                    };
                    movieDetail.Add(movieDto);
                }
                return movieDetail;
            }
        }

        public void Delete(int id)
        {
            using (SqlConnection sqlConnection =new(_connectionString)) 
            {
                string deleteQuery = @"Delete From Movies Where Id = @id";
                SqlCommand sqlCommand = new(deleteQuery, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@id", id);
                sqlConnection.Open();
                sqlCommand.ExecuteNonQuery();
                sqlConnection.Close();
            }   
        }

        public int Add(MovieDto movieDto)
        {
            using (SqlConnection sqlConnection = new(_connectionString)) 
            {
                string insertQuery = @"
                    INSERT INTO MOVIES(ActorName, ActressName, Title, MovieType, ReleaseDate)
                    VALUES(@ActorName, @ActressName, @Title, @MovieType, @ReleaseDate)
                    Select Scope_Identity()";    
                var sqlCommand = new SqlCommand(insertQuery, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@ActorName", movieDto.ActorName);
                sqlCommand.Parameters.AddWithValue("@ActressName", movieDto.ActressName);
                sqlCommand.Parameters.AddWithValue("@Title", movieDto.Title);
                sqlCommand.Parameters.AddWithValue("@MovieType", movieDto.MovieType);
                sqlCommand.Parameters.AddWithValue("@ReleaseDate", DateTime.Now);
                sqlConnection.Open();
                movieDto.Id = Convert.ToInt32(sqlCommand.ExecuteScalar());
                sqlConnection.Close();
                return movieDto.Id;
            }
        }

        public void Update(MovieDto movieDto)
        {
            using (SqlConnection sqlConnection=new(_connectionString)) 
            {
                string updateQuery = @"Update Movies
                    Set ActorName = @ActorName, ActressName = @ActressName, Title = @Title, MovieType = @MovieType
                    Where Id = @Id;";
                var sqlCommand = new SqlCommand(updateQuery, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@Id", movieDto.Id);
                sqlCommand.Parameters.AddWithValue("@ActorName", movieDto.ActorName);
                sqlCommand.Parameters.AddWithValue("@ActressName", movieDto.ActressName);
                sqlCommand.Parameters.AddWithValue("@Title", movieDto.Title);
                sqlCommand.Parameters.AddWithValue("@MovieType", movieDto.MovieType);
                sqlConnection.Open();
                sqlCommand.ExecuteNonQuery();
                sqlConnection.Close();
            }
        }
    }
}
