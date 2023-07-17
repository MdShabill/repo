using MyWebApp.ViewModels;
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
                string sqlQuery = @"Select 
                                    Employees1.Id, Employees1.FullName, Employees1.FatherName, 
                                    Employees1.Email, Employees1.CountryId, Employees1.QualificationId,
                                    Qualifications.QualificationName
                                    From Employees1
                                    Inner Join Qualifications On Employees1.QualificationId = Qualifications.Id";

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
                        QualificationId = (int)dataTable.Rows[i]["QualificationId"],
                        QualificationName = (string)dataTable.Rows[i]["QualificationName"],
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
                    (FullName, FatherName, Email, CountryId, QualificationId)
                     VALUES 
                    (@FullName, @FatherName, @Email, @CountryId, @QualificationId) ";
                SqlCommand sqlCommand = new(sqlQuery, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@FullName", employee.FullName);
                sqlCommand.Parameters.AddWithValue("@FatherName", employee.FatherName);
                sqlCommand.Parameters.AddWithValue("@Email", employee.Email);
                sqlCommand.Parameters.AddWithValue("@CountryId", employee.CountryId);
                sqlCommand.Parameters.AddWithValue("@QualificationId", employee.QualificationId);
                sqlConnection.Open();
                sqlCommand.ExecuteNonQuery();
                sqlConnection.Close();
            }
        }

        public List<Qualification> GetQualification()
        {
            using(SqlConnection sqlConnection =new(_connectionString)) 
            {
                string sqlQuery = "Select Id, QualificationName From Qualifications Order By Id, QualificationName DESC";

                SqlDataAdapter sqlDataAdapter = new(sqlQuery, sqlConnection);
                DataTable dataTable = new();
                sqlDataAdapter.Fill(dataTable);

                List<Qualification> qualifications = new();

                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    Qualification qualification = new()
                    {
                        Id = (int)dataTable.Rows[i]["Id"],
                        QualificationName = (string)dataTable.Rows[i]["QualificationName"]
                    };
                    qualifications.Add(qualification);
                }
                return qualifications;
            }
        }
    }
}
