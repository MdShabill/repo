using Microsoft.Data.SqlClient;
using MyWebApp.DataModel;
using MyWebApp.ViewModels;
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
                string sqlQuery = @"
                          Select Movies.Id,
                          Movies.MovieName, Movies.DirectorName,
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
                        ActorName = (string)dataTable.Rows[i]["ActorName"],
                    };
                    movies.Add(movie);
                }
                return movies;
            }
        }

        //TODO: Refactor this query and make Join dynamic
        public Movie Get(int id)
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                string sqlQuery = @"
                            Select Movies.Id,
                            Movies.MovieName, Movies.DirectorName,
                            Movies.ActorId, Actors.ActorName
                            From Movies
                            Inner Join Actors On Movies.ActorId = Actors.Id
                            Where 
                            Movies.Id = @Id";
                SqlDataAdapter sqlDataAdapter = new(sqlQuery, sqlConnection);
                sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@id", id);
                DataTable dataTable = new();
                sqlDataAdapter.Fill(dataTable);

                Movie movie = new()
                {
                    Id = (int)dataTable.Rows[0]["Id"],
                    MovieName = (string)dataTable.Rows[0]["MovieName"],
                    DirectorName = (string)dataTable.Rows[0]["DirectorName"],
                    ActorId = (int)dataTable.Rows[0]["ActorId"],
                    ActorName = (string)dataTable.Rows[0]["ActorName"],
                };
                return movie;
            }
        }

        public List<Actors> GetAllActors()
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                string sqlQuery = "SELECT Id, ActorName FROM Actors";
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

        public int Delete(int id)
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                string deleteQuery = "Delete From Movies Where Id = @id";
                SqlCommand sqlCommand = new(deleteQuery, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@id", id);
                sqlConnection.Open();
                int affectedRowCount = sqlCommand.ExecuteNonQuery();
                sqlConnection.Close();
                return affectedRowCount;
            }
        }

        public int Add(MovieVm movie)
        {
            using(SqlConnection sqlConnection = new(_connectionString))
            {
                string sqlQuery = @"
                       INSERT INTO Movies
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

        public int Update(MovieVm movie)
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                string sqlQuery = @" 
                          UPDATE Movies Set 
                          MovieName = @MovieName, 
                          DirectorName = @DirectorName,
                          ActorId = @ActorId
                          WHERE Id = @Id ";
                SqlCommand sqlCommand = new(sqlQuery, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@Id", movie.Id);
                sqlCommand.Parameters.AddWithValue("@MovieName", movie.MovieName);
                sqlCommand.Parameters.AddWithValue("@DirectorName", movie.DirectorName);
                sqlCommand.Parameters.AddWithValue("@ActorId", movie.ActorId);
                sqlConnection.Open();
                int affectedRowCount = sqlCommand.ExecuteNonQuery();
                sqlConnection.Close();
                return affectedRowCount;
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
