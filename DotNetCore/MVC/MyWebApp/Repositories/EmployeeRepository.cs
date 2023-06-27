using MyWebApp.Models;
using System.Data;
using Microsoft.Data.SqlClient;

namespace MyWebApp.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly string _connectionString;

        public EmployeeRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void Add(Employee employee)
        {
            using(SqlConnection sqlConnection = new(_connectionString))
            {
                string sqlQuery = @"INSERT INTO Employees1
                    (FullName, FatherName, Email)
                     VALUES 
                    (@FullName, @FatherName, @Email) ";
                SqlCommand sqlCommand = new(sqlQuery, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@FullName", employee.FullName);
                sqlCommand.Parameters.AddWithValue("@FatherName", employee.FatherName);
                sqlCommand.Parameters.AddWithValue("@Email", employee.Email);
                sqlConnection.Open();
                sqlCommand.ExecuteNonQuery();
                sqlConnection.Close();
            }
        }
    }
}
