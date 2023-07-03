using Microsoft.Data.SqlClient;
using MyWebApp.Models;
using System.Data;

namespace MyWebApp.Repositories
{
    public class MovieRepository : IMovieRepository
    {
        private readonly string _connectionString;

        public MovieRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<Movie> GetAll()
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                string sqlQuery = @"Select 
                                   Movies.Id, Movies.MovieName, Movies.DirectorName,
                                   Movies.ActorId, Actors.ActorName
                                   From Movies
                                   Inner Join Actors On Movies.ActorId = Actors.Id ";
                SqlDataAdapter sqlDataAdapter = new(sqlQuery, sqlConnection);
                DataTable dataTable = new();
                sqlDataAdapter.Fill(dataTable);

                List<Movie> movies = new();

                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    Movie movie = new()
                    {
                        Id = (int)dataTable.Rows[i]["Id"],
                        MovieName = (string)dataTable.Rows[i]["MovieName"],
                        DirectorName = (string)dataTable.Rows[i]["DirectorName"],
                        ActorId = (int)dataTable.Rows[i]["ActorId"],
                        ActorName = (string)dataTable.Rows[i]["ActorName"],
                    };
                    movies.Add(movie);
                }
                return movies;
            }
        }

        public int Add(Movie movie)
        {
            using(SqlConnection sqlConnection = new(_connectionString))
            {
                string sqlQuery = @"INSERT INTO Movies
                    (MovieName, DirectorName, ActorId)
                     VALUES 
                    (@MovieName, @DirectorName, @ActorId) ";
                SqlCommand sqlCommand = new(sqlQuery, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@MovieName", movie.MovieName);
                sqlCommand.Parameters.AddWithValue("@DirectorName", movie.DirectorName);
                sqlCommand.Parameters.AddWithValue("@ActorId", movie.ActorId);
                sqlConnection.Open();
                int affectedRecordCount = sqlCommand.ExecuteNonQuery();
                sqlConnection.Close();
                return affectedRecordCount;
            }
        }

        public List<Actors> GetActorDetails()
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                string sqlQuery = "Select Id, ActorName From Actors Order By Id, ActorName DESC";

                SqlDataAdapter sqlDataAdapter = new(sqlQuery, sqlConnection);
                DataTable dataTable = new();
                sqlDataAdapter.Fill(dataTable);

                List<Actors> actors = new();

                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    Actors actor = new()
                    {
                        Id = (int)dataTable.Rows[i]["Id"],
                        ActorName = (string)dataTable.Rows[i]["ActorName"]
                    };
                    actors.Add(actor);
                }
                return actors;
            }
        }
    }
}
