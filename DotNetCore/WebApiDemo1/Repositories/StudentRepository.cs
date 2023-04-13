using Microsoft.Data.SqlClient;
using System.Data;
using WebApiDemo1.DTO.InputDTO;

namespace WebApiDemo1.Repositories
{
    public class StudentRepository : IStudentRepository 
    {
        private readonly string _connectionString;

        public StudentRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public DataTable GetAllStudents()
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                SqlDataAdapter sqlDataAdapter = new("SELECT * FROM Students", sqlConnection);
                DataTable dataTable = new();
                sqlDataAdapter.Fill(dataTable);
                return dataTable;
            }
        }

        public DataTable GetStudentCount()
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                SqlDataAdapter sqlDataAdapter = new("SELECT * FROM Students", sqlConnection);
                DataTable dataTable = new();
                sqlDataAdapter.Fill(dataTable);
                return dataTable;
            }
        }

        public void DeleteStudent(int id)
        {
            using (SqlConnection sqlConnection = new(_connectionString)) 
            {
                string deleteQuery = "Delete From Students Where Id = @id";
                SqlCommand sqlCommand = new(deleteQuery, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@id", id);
                sqlConnection.Open();
                sqlCommand.ExecuteNonQuery();
                sqlConnection.Close();
            }
        }

        public int Add(StudentDto studentDto)
        {
            using (SqlConnection sqlConnection = new(_connectionString)) 
            {
                string insertQuery = @"
                    INSERT INTO Students(FullName, Gender, Email, Password, RegistrationDate)
                    VALUES (@FullName, @Gender, @Email, @Password, @RegistrationDate)
                    Select Scope_Identity()";
                var sqlCommand = new SqlCommand(insertQuery, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@FullName", studentDto.FullName);
                sqlCommand.Parameters.AddWithValue("@Gender", studentDto.Gender);
                sqlCommand.Parameters.AddWithValue("@Email", studentDto.Email);
                sqlCommand.Parameters.AddWithValue("@Password", studentDto.Password);
                sqlCommand.Parameters.AddWithValue("@RegistrationDate", DateTime.Now);
                sqlConnection.Open();
                studentDto.Id = Convert.ToInt32(sqlCommand.ExecuteScalar());
                sqlConnection.Close();
                return studentDto.Id;
            }
        }

        public void Update(StudentDto studentDto)
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                string updateQuery = @"Update Students
                    Set FullName = @FullName, Gender = @Gender, Email = @Email, Password = @password
                    Where @Id = Id;";
                var sqlCommand = new SqlCommand(updateQuery, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@Id", studentDto.Id);
                sqlCommand.Parameters.AddWithValue("@FullName", studentDto.FullName);
                sqlCommand.Parameters.AddWithValue("@Gender", studentDto.Gender);
                sqlCommand.Parameters.AddWithValue("@Email", studentDto.Email);
                sqlCommand.Parameters.AddWithValue("@Password", studentDto.Password);
                sqlConnection.Open();
                sqlCommand.ExecuteNonQuery();
                sqlConnection.Close();
            }
        }
    }
}
