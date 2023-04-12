using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using WebApiDemo1.DataModel;
using WebApiDemo1.DTO.InputDTO;

namespace WebApiDemo1.Repositories
{
    public class MovieRepository : IMovieRepository
    {
        private readonly string _connectionString;

        public MovieRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public DataTable GetAllMovies()
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                SqlDataAdapter sqlDataAdapter = new("SELECT * FROM Movies", sqlConnection);
                DataTable dataTable = new();
                sqlDataAdapter.Fill(dataTable);
                return dataTable;
            }
        }

        public DataTable GetMovieCount()
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                SqlDataAdapter sqlDataAdapter = new("SELECT * FROM Movies", sqlConnection);
                DataTable dataTable = new();
                sqlDataAdapter.Fill(dataTable);
                return dataTable;
            }
        }

        public MovieDto DeleteRecord(int id)
        {
            using (SqlConnection sqlConnection =new(_connectionString)) 
            {
                string deleteQuery = @"Delete From Movies Where Id = @id";
                SqlCommand sqlCommand = new(deleteQuery, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@id", id);
                sqlConnection.Open();
                sqlCommand.ExecuteNonQuery();
                sqlConnection.Close();
                return new MovieDto();
            }   
        }

        public int Add(MovieDto movieDto)
        {
            using (SqlConnection sqlConnection = new(_connectionString)) 
            {
                string insertQuery = @"
                    INSERT INTO MOVIES(ActorName, ActressName, Title, MovieType, ReleaseDate)
                    VALUES(@ActorName, @ActressName, @Title, @MovieType, @ReleaseDate)";

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
