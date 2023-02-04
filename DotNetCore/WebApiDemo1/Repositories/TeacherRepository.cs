using Microsoft.Data.SqlClient;
using System.Data;
using WebApiDemo1.DTO;

namespace WebApiDemo1.Repositories
{
    public class TeacherRepository : ITeacherRepository
    {
        private readonly string _connectionString;

        public TeacherRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public DataTable GetAllTeachers()
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                SqlDataAdapter sqlDataAdapter = new("SELECT * FROM Teachers", sqlConnection);
                DataTable dataTable = new();
                sqlDataAdapter.Fill(dataTable);
                return dataTable;
            }
        }

        public int GetTeachersCount()
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                string sqlQuery = "SELECT COUNT(*) FROM Teachers";
                SqlCommand sqlCommand = new(sqlQuery, sqlConnection);
                sqlConnection.Open();
                int teacherCount = Convert.ToInt32(sqlCommand.ExecuteScalar());
                sqlConnection.Close();
                return teacherCount;
            }

        }

        public DataTable GetTeachersDetailById(int teacherId)
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                SqlDataAdapter sqlDataAdapter = new("SELECT * FROM Teachers WHERE Id = @teacherId", sqlConnection);
                sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@teacherId", teacherId);
                DataTable dataTable = new();
                sqlDataAdapter.Fill(dataTable);
                return dataTable;
            }
        }

        public DataTable GetTeachersByDepartmentByTeacherName(string teacherName, string? department)
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                string sqlQuery = "SELECT * FROM Teachers Where FullName = @teacherName ";
                if (!string.IsNullOrWhiteSpace(department))
                    sqlQuery += "And Department = @department";
                SqlDataAdapter sqlDataAdapter = new(sqlQuery, sqlConnection);
                sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@teacherName", teacherName);
                sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@department", department);
                DataTable dataTable = new();
                sqlDataAdapter.Fill(dataTable);
                return dataTable;
            }
        }

        public DataTable GetTeachersBySalaryRange(int minimumSalary, int maximumSalary)
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                SqlDataAdapter sqlDataAdapter = new(@"SELECT * FROM Teachers 
                        WHERE Salary BETWEEN @minimumSalary AND @maximumSalary
                        ORDER BY Salary", sqlConnection);
                sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@minimumSalary", minimumSalary);
                sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@maximumSalary", maximumSalary);
                DataTable dataTable = new();
                sqlDataAdapter.Fill(dataTable);
                return dataTable;
            }
        }

        public int TeacherAdd(TeacherDto teacher)
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                string sqlQuery = @"INSERT INTO Teachers(FullName, Email, Age, Gender, SchoolName, Department, Salary)
                            VALUES (@FullName, @Email, @Age, @Gender, @SchoolName, @Department, @Salary)
                            Select Scope_Identity() ";
                SqlCommand sqlCommand = new(sqlQuery, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@FullName", teacher.FullName);
                sqlCommand.Parameters.AddWithValue("@Email", teacher.Email);
                sqlCommand.Parameters.AddWithValue("@Age", teacher.Age);
                sqlCommand.Parameters.AddWithValue("@Gender", teacher.Gender);
                sqlCommand.Parameters.AddWithValue("@SchoolName", teacher.SchoolName);
                sqlCommand.Parameters.AddWithValue("@Department", teacher.Department);
                sqlCommand.Parameters.AddWithValue("@Salary", teacher.Salary);
                sqlConnection.Open();
                teacher.Id = Convert.ToInt32(sqlCommand.ExecuteScalar());
                sqlConnection.Close();
                return teacher.Id;
            }
        }

        public void TeacherUpdate(TeacherDto teacher)
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                string sqlQuery = @" UPDATE Teachers Set FullName = @FullName, Email = @Email, Age = @Age, 
                            Gender = @Gender, SchoolName = @SchoolName, Salary = @Salary
                            WHERE Id = @Id ";
                SqlCommand sqlCommand = new(sqlQuery, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@Id", teacher.Id);
                sqlCommand.Parameters.AddWithValue("@FullName", teacher.FullName);
                sqlCommand.Parameters.AddWithValue("@Email", teacher.Email);
                sqlCommand.Parameters.AddWithValue("@Age", teacher.Age);
                sqlCommand.Parameters.AddWithValue("@Gender", teacher.Gender);
                sqlCommand.Parameters.AddWithValue("@SchoolName", teacher.SchoolName);
                sqlCommand.Parameters.AddWithValue("@Department", teacher.Department);
                sqlCommand.Parameters.AddWithValue("@Salary", teacher.Salary);
                sqlConnection.Open();
                teacher.Id = Convert.ToInt32(sqlCommand.ExecuteScalar());
                sqlConnection.Close();
            }
        }
    }
}
