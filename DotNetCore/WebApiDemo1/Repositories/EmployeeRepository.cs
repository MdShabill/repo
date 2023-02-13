using Microsoft.Data.SqlClient;
using System.Data;
using System.Reflection;
using WebApiDemo1.DTO.InputDTO;
using WebApiDemo1.Enums;

namespace WebApiDemo1.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly string _connectionString;

        public EmployeeRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<EmployeeDto> GetAllEmployeesAsList()
        {
            List<EmployeeDto> employees = new();

            using (SqlConnection sqlConnection = new(_connectionString))
            {
                SqlDataAdapter sqlDataAdapter = new("SELECT * FROM Employees", sqlConnection);
                DataTable dataTable = new();
                sqlDataAdapter.Fill(dataTable);

                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    EmployeeDto employeeDto = new();
                    employeeDto.Id = (int)dataTable.Rows[i]["Id"];
                    employeeDto.FullName = (string)dataTable.Rows[i]["FullName"];
                    employeeDto.Gender = (GenderTypes)dataTable.Rows[i]["Gender"];
                    employeeDto.Email = (string)dataTable.Rows[i]["Email"];
                    employeeDto.MobileNumber = (string)dataTable.Rows[i]["MobileNumber"];
                    employeeDto.Salary = (int)dataTable.Rows[i]["Salary"];
                    employees.Add(employeeDto);
                }
                return employees;
            }
        }

        public EmployeeDto GetAllEmployeeById(int id)
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                SqlDataAdapter sqlDataAdapter = new($"SELECT * FROM Employees Where Id = @id", sqlConnection);
                sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@Id", id);
                DataTable dataTable = new();
                sqlDataAdapter.Fill(dataTable);
                if (dataTable.Rows.Count > 0)
                {
                    EmployeeDto employeeDto = new();
                    employeeDto.Id = (int)dataTable.Rows[0]["Id"];
                    employeeDto.FullName = (string)dataTable.Rows[0]["FullName"];
                    employeeDto.Gender = (GenderTypes)dataTable.Rows[0]["Gender"];
                    employeeDto.Email = (string)dataTable.Rows[0]["Email"];
                    employeeDto.MobileNumber = (string)dataTable.Rows[0]["MobileNumber"];
                    employeeDto.Salary = (int)dataTable.Rows[0]["Salary"];
                    return employeeDto;
                }
                else  
                    return null;   
            }
        }

        public int GetEmployeesCount()
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                string sqlQuery = "SELECT COUNT(*) FROM Employees ";
                SqlCommand sqlCommand = new(sqlQuery, sqlConnection);
                sqlConnection.Open();
                int employeeCount = Convert.ToInt32(sqlCommand.ExecuteScalar());
                sqlConnection.Close();
                return employeeCount;
            }
        }

        public List<EmployeeDto> GetEmployeesDetailByGenderBySalary(int gender, int salary)
        {
            List<EmployeeDto> employees = new();

            using (SqlConnection sqlConnection = new(_connectionString))
            {
                SqlDataAdapter sqlDataAdapter = new(@"SELECT * FROM Employees WHERE Gender = @gender 
                                                  AND Salary > @salary", sqlConnection);
                sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@gender", gender);
                sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@salary", salary);
                DataTable dataTable = new();
                sqlDataAdapter.Fill(dataTable);

                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    EmployeeDto employeeDto = new();
                    employeeDto.Id = (int)dataTable.Rows[i]["Id"];
                    employeeDto.FullName = (string)dataTable.Rows[i]["FullName"];
                    employeeDto.Gender = (GenderTypes)dataTable.Rows[i]["Gender"];
                    employeeDto.Email = (string)dataTable.Rows[i]["Email"];
                    employeeDto.MobileNumber = (string)dataTable.Rows[i]["MobileNumber"];
                    employeeDto.Salary = (int)dataTable.Rows[i]["Salary"];
                    employees.Add(employeeDto);
                }
                return employees;
            }
        }

        public List<EmployeeDto> GetEmployeesBySalaryRange(int minimumSalary, int maximumSalary)
        {
            List<EmployeeDto> employees = new();

            using (SqlConnection sqlConnection = new(_connectionString))
            {
                SqlDataAdapter sqlDataAdapter = new(@" SELECT * FROM Employees 
                               WHERE Salary BETWEEN @minimumSalary AND @maximumSalary
                               ORDER BY Salary", sqlConnection);
                sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@minimumSalary", minimumSalary);
                sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@maximumSalary", maximumSalary);
                DataTable dataTable = new();
                sqlDataAdapter.Fill(dataTable);

                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    EmployeeDto employeeDto = new();
                    employeeDto.Id = (int)dataTable.Rows[i]["Id"];
                    employeeDto.FullName = (string)dataTable.Rows[i]["FullName"];
                    employeeDto.Gender = (GenderTypes)dataTable.Rows[i]["Gender"];
                    employeeDto.Email = (string)dataTable.Rows[i]["Email"];
                    employeeDto.MobileNumber = (string)dataTable.Rows[i]["MobileNumber"];
                    employeeDto.Salary = (int)dataTable.Rows[i]["Salary"];
                    employees.Add(employeeDto);
                }
                return employees;
            }
        }

        public int Add(EmployeeDto employee)
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                string sqlQuery = @"INSERT INTO Employees(FullName, Gender, Email, MobileNumber, Salary)
                           VALUES (@FullName, @Gender, @Email, @MobileNumber, @Salary)
                           Select Scope_Identity() ";
                SqlCommand sqlCommand = new(sqlQuery, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@FullName", employee.FullName);
                sqlCommand.Parameters.AddWithValue("@Gender", employee.Gender);
                sqlCommand.Parameters.AddWithValue("@Email", employee.Email);
                sqlCommand.Parameters.AddWithValue("@MobileNumber", employee.MobileNumber);
                sqlCommand.Parameters.AddWithValue("@Salary", employee.Salary);
                sqlConnection.Open();
                employee.Id = Convert.ToInt32(sqlCommand.ExecuteScalar());
                sqlConnection.Close();
                return employee.Id;
            }   
        }

        public void Update(EmployeeDto employee)
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                string sqlQuery = @"UPDATE Employees SET FullName = @FullName, Gender = @Gender,
                           Email = @Email, MobileNumber = @MobileNumber, Salary = @Salary
                           WHERE Id = @Id ";
                SqlCommand sqlCommand = new(sqlQuery, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@Id", employee.Id);
                sqlCommand.Parameters.AddWithValue("@FullName", employee.FullName);
                sqlCommand.Parameters.AddWithValue("@Gender", employee.Gender);
                sqlCommand.Parameters.AddWithValue("@Email", employee.Email);
                sqlCommand.Parameters.AddWithValue("@MobileNumber", employee.MobileNumber);
                sqlCommand.Parameters.AddWithValue("@Salary", employee.Salary);
                sqlConnection.Open();
                sqlCommand.ExecuteNonQuery();
                sqlConnection.Close();
            }
        }
    }
}
