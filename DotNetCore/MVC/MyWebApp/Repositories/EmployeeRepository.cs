using MyWebApp.Models;
using System.Data;
using Microsoft.Data.SqlClient;
using MyWebApp.Enums;

namespace MyWebApp.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly string _connectionString;

        public EmployeeRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<Employee> GetAll()
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                string sqlQuery = "Select * From Employees1";

                SqlDataAdapter sqlDataAdapter = new(sqlQuery, sqlConnection);
                DataTable dataTable = new();
                sqlDataAdapter.Fill(dataTable);

                List<Employee> employees = new();

                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    Employee employee = new()
                    {
                        Id = (int)dataTable.Rows[i]["Id"],
                        FullName = (string)dataTable.Rows[i]["FullName"],
                        FatherName = (string)dataTable.Rows[i]["FatherName"],
                        Email = (string)dataTable.Rows[i]["Email"],
                        CountryId = (int)dataTable.Rows[i]["CountryId"],
                    };
                    employees.Add(employee);
                }
                return employees;
            }
        }

        public void Add(Employee employee)
        {
            using(SqlConnection sqlConnection = new(_connectionString))
            {
                string sqlQuery = @"INSERT INTO Employees1
                    (FullName, FatherName, Email, CountryId)
                     VALUES 
                    (@FullName, @FatherName, @Email, @CountryId) ";
                SqlCommand sqlCommand = new(sqlQuery, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@FullName", employee.FullName);
                sqlCommand.Parameters.AddWithValue("@FatherName", employee.FatherName);
                sqlCommand.Parameters.AddWithValue("@Email", employee.Email);
                sqlCommand.Parameters.AddWithValue("@CountryId", employee.CountryId);
                sqlConnection.Open();
                sqlCommand.ExecuteNonQuery();
                sqlConnection.Close();
            }
        }
    }
}
