using Microsoft.Data.SqlClient;
using System.Data;
using WebApiDemo1.DTO.InputDTO;
using WebApiDemo1.Enums;

namespace WebApiDemo1.Repositories
{
    public class StudentRepository : IStudentRepository 
    {
        private readonly string _connectionString;

        public StudentRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public StudentDto GetStudentById(int id)
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                SqlDataAdapter sqlDataAdapter = new("Select * From Students Where Id = @id", sqlConnection);
                sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@id", id);
                DataTable dataTable = new();
                sqlDataAdapter.Fill(dataTable);

                if (dataTable.Rows.Count > 0)
                {
                    //Apporach #1
                    StudentDto studentDto = new();
                    studentDto.Id = (int)dataTable.Rows[0]["Id"];
                    studentDto.FullName = (string)dataTable.Rows[0]["FullName"];
                    studentDto.Gender = (GenderTypes)dataTable.Rows[0]["Gender"];
                    studentDto.Email = (string)dataTable.Rows[0]["Email"];
                    studentDto.Password = (string)dataTable.Rows[0]["Password"];

                    //Apporach #2
                    //StudentDto studentDto = new()
                    //{
                    //    Id = (int)dataTable.Rows[i]["Id"],
                    //    FullName = (string)dataTable.Rows[i]["FullName"],
                    //    Gender = (GenderTypes)dataTable.Rows[i]["Gender"],
                    //    Email = (string)dataTable.Rows[i]["Email"],
                    //    Password = (string)dataTable.Rows[i]["Password"]
                    //};

                    return studentDto;
                }
                else
                {
                    return null;
                }
            }
        }

        public List<StudentDto> GetAllStudents()
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                SqlDataAdapter sqlDataAdapter = new("SELECT * FROM Students", sqlConnection);
                DataTable dataTable = new();
                sqlDataAdapter.Fill(dataTable);

                List<StudentDto> student = new();

                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    StudentDto studentDto = new()
                    {
                        Id = (int)dataTable.Rows[i]["Id"],
                        FullName = (string)dataTable.Rows[i]["FullName"],
                        Gender = (GenderTypes)dataTable.Rows[i]["Gender"],
                        Email = (string)dataTable.Rows[i]["Email"],
                        Password = (string)dataTable.Rows[i]["Password"]
                    };
                    student.Add(studentDto);
                }
                return student;
            }
        }

        public int GetStudentCount()
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                string sqlQuery = "SELECT COUNT(*) FROM Students ";
                SqlCommand sqlCommand = new(sqlQuery, sqlConnection);
                sqlConnection.Open();
                int studentCount = Convert.ToInt32(sqlCommand.ExecuteScalar());
                sqlConnection.Close();
                return studentCount;
            }
        }

        public string GetStudentFullNameById(int id)
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                string sqlQuery = "Select FullName From Students Where Id = @Id";
                SqlCommand sqlCommand = new(sqlQuery, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@Id", id);
                sqlConnection.Open();
                string studentFullName = Convert.ToString(sqlCommand.ExecuteScalar());
                sqlConnection.Close();
                return studentFullName;
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
