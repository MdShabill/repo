using Microsoft.Data.SqlClient;
using System.Data;
using WebApiDemo1.DTO;
using WebApiDemo1.DTO.InputDTO;
using WebApiDemo1.Enums;
using WebApplication1.DTO.InputDTO;

namespace WebApiDemo1.Repositories
{
    public class TeacherRepository : ITeacherRepository
    {
        private readonly string _connectionString;

        public TeacherRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<TeacherDto> GetAllTeachersAsList()
        {
            List<TeacherDto> teachers = new();

            using (SqlConnection sqlConnection = new(_connectionString))
            {
                SqlDataAdapter sqlDataAdapter = new("SELECT * FROM Teachers", sqlConnection);
                DataTable dataTable = new();
                sqlDataAdapter.Fill(dataTable);

                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    TeacherDto teacherDto = new();
                    teacherDto.Id = (int)dataTable.Rows[i]["Id"];
                    teacherDto.FullName = (string)dataTable.Rows[i]["FullName"];
                    teacherDto.Age = (int)dataTable.Rows[i]["Age"];
                    teacherDto.Gender = (GenderTypes)dataTable.Rows[i]["Gender"];
                    teacherDto.Email = (string)dataTable.Rows[i]["Email"];
                    teacherDto.MobileNumber = (string)dataTable.Rows[i]["MobileNumber"];
                    teacherDto.SchoolName = (string)dataTable.Rows[i]["SchoolName"];
                    teacherDto.Department = (string)dataTable.Rows[i]["Department"];
                    teacherDto.Salary = (int)dataTable.Rows[i]["Salary"];
                    teachers.Add(teacherDto);
                }
                return teachers;
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

        public TeacherDto GetTeacherDetailById(int id)
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                SqlDataAdapter sqlDataAdapter = new("SELECT * FROM Teachers Where Id = @id", sqlConnection);
                sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@Id", id);
                DataTable dataTable = new();
                sqlDataAdapter.Fill(dataTable);

                if (dataTable.Rows.Count > 0)
                {
                    TeacherDto teacherDto = new();
                    teacherDto.Id = (int)dataTable.Rows[0]["Id"];
                    teacherDto.FullName = (string)dataTable.Rows[0]["FullName"];
                    teacherDto.Age = (int)dataTable.Rows[0]["Age"];
                    teacherDto.Gender = (GenderTypes)dataTable.Rows[0]["Gender"];
                    teacherDto.Email = (string)dataTable.Rows[0]["Email"];
                    teacherDto.MobileNumber = (string)dataTable.Rows[0]["MobileNumber"];
                    teacherDto.SchoolName = (string)dataTable.Rows[0]["SchoolName"];
                    teacherDto.Department = (string)dataTable.Rows[0]["Department"];
                    teacherDto.Salary = (int)dataTable.Rows[0]["Salary"];
                    return teacherDto;
                }
                else
                    return null;
            }
        }

        public List<TeacherDto> GetTeachersByDepartmentByTeacherName(string fullName, string? department)
        {
            List<TeacherDto> teachers = new();

            using (SqlConnection sqlConnection = new(_connectionString))
            {
                string sqlQuery = "SELECT * FROM Teachers Where FullName = @fullName ";
                if (!string.IsNullOrWhiteSpace(department))
                    sqlQuery += "And Department = @department";
                SqlDataAdapter sqlDataAdapter = new(sqlQuery, sqlConnection);
                sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@fullName", fullName);
                sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@department", department);
                DataTable dataTable = new();
                sqlDataAdapter.Fill(dataTable);

                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    TeacherDto teacherDto = new();
                    teacherDto.Id = (int)dataTable.Rows[i]["Id"];
                    teacherDto.FullName = (string)dataTable.Rows[i]["FullName"];
                    teacherDto.Age = (int)dataTable.Rows[i]["Age"];
                    teacherDto.Gender = (GenderTypes)dataTable.Rows[i]["Gender"];
                    teacherDto.Email = (string)dataTable.Rows[i]["Email"];
                    teacherDto.MobileNumber = (string)dataTable.Rows[i]["MobileNumber"];
                    teacherDto.SchoolName = (string)dataTable.Rows[i]["SchoolName"];
                    teacherDto.Department = (string)dataTable.Rows[i]["Department"];
                    teacherDto.Salary = (int)dataTable.Rows[i]["Salary"];
                    teachers.Add(teacherDto);
                }
                return teachers;
            }
        }

        public List<TeacherDto> GetTeachersBySalaryRange(int minimumSalary, int maximumSalary)
        {
            List<TeacherDto> teachers = new();

            using (SqlConnection sqlConnection = new(_connectionString))
            {
                SqlDataAdapter sqlDataAdapter = new(@"SELECT * FROM Teachers 
                        WHERE Salary BETWEEN @minimumSalary AND @maximumSalary
                        ORDER BY Salary", sqlConnection);
                sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@minimumSalary", minimumSalary);
                sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@maximumSalary", maximumSalary);
                DataTable dataTable = new();
                sqlDataAdapter.Fill(dataTable);

                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    TeacherDto teacherDto = new();
                    teacherDto.Id = (int)dataTable.Rows[i]["Id"];
                    teacherDto.FullName = (string)dataTable.Rows[i]["FullName"];
                    teacherDto.Age = (int)dataTable.Rows[i]["Age"];
                    teacherDto.Gender = (GenderTypes)dataTable.Rows[i]["Gender"];
                    teacherDto.Email = (string)dataTable.Rows[i]["Email"];
                    teacherDto.MobileNumber = (string)dataTable.Rows[i]["MobileNumber"];
                    teacherDto.SchoolName = (string)dataTable.Rows[i]["SchoolName"];
                    teacherDto.Department = (string)dataTable.Rows[i]["Department"];
                    teacherDto.Salary = (int)dataTable.Rows[i]["Salary"];
                    teachers.Add(teacherDto);
                }
                return teachers;
            }
        }

        public int Add(TeacherDto teacher)
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                string sqlQuery = @"INSERT INTO Teachers(FullName, Age, Gender, 
                            Email, MobileNumber, SchoolName, Department, Salary)
                            VALUES (@FullName, @Age, @Gender, @Email, @MobileNumber, 
                            @SchoolName, @Department, @Salary)       
                            Select Scope_Identity() ";
                SqlCommand sqlCommand = new(sqlQuery, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@FullName", teacher.FullName);
                sqlCommand.Parameters.AddWithValue("@Age", teacher.Age);
                sqlCommand.Parameters.AddWithValue("@Gender", teacher.Gender);
                sqlCommand.Parameters.AddWithValue("@Email", teacher.Email);
                sqlCommand.Parameters.AddWithValue("@MobileNumber", teacher.MobileNumber);
                sqlCommand.Parameters.AddWithValue("@SchoolName", teacher.SchoolName);
                sqlCommand.Parameters.AddWithValue("@Department", teacher.Department);
                sqlCommand.Parameters.AddWithValue("@Salary", teacher.Salary);
                sqlConnection.Open();
                teacher.Id = Convert.ToInt32(sqlCommand.ExecuteScalar());
                sqlConnection.Close();
                return teacher.Id;
            }
        }

        public void Update(TeacherDto teacher)
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                string sqlQuery = @" UPDATE Teachers Set FullName = @FullName, Age = @Age, 
                        Gender = @Gender, Email = @Email, MobileNumber = @MobileNumber, SchoolName = @SchoolName, 
                        Salary = @Salary, Department = @Department
                        WHERE Id = @Id ";
                SqlCommand sqlCommand = new(sqlQuery, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@Id", teacher.Id);
                sqlCommand.Parameters.AddWithValue("@FullName", teacher.FullName);
                sqlCommand.Parameters.AddWithValue("@Age", teacher.Age);
                sqlCommand.Parameters.AddWithValue("@Gender", teacher.Gender);
                sqlCommand.Parameters.AddWithValue("@Email", teacher.Email);
                sqlCommand.Parameters.AddWithValue("@MobileNumber", teacher.MobileNumber);
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
