using MyWebApp.Models;
using System.Data;
using Microsoft.Data.SqlClient;
using MyWebApp.Enums;

namespace MyWebApp.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly string _connectionString;

        public CustomerRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<Customer> GetAll()
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                string sql = @"Select Id, FirstName, LastName, Email, DateOfBirth, Mobile,
                                Case
	                                When Gender = 1 Then 'Male'
	                                When Gender = 2 Then 'Female'
                                End As Gender
                                from Customers ";

                SqlDataAdapter sqlDataAdapter = new(sql, sqlConnection);
                DataTable dataTable = new();
                sqlDataAdapter.Fill(dataTable);

                List<Customer> customers = new();

                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    Customer customer = new()
                    {
                        Id = (int)dataTable.Rows[i]["Id"],
                        FirstName = (string)dataTable.Rows[i]["FirstName"],
                        LastName = (string)dataTable.Rows[i]["LastName"],
                        Gender = (string)dataTable.Rows[i]["Gender"],
                        Email = (string)dataTable.Rows[i]["Email"],
                        DateOfBirth = (DateTime)dataTable.Rows[i]["DateOfBirth"],
                        Mobile = Convert.ToString(dataTable.Rows[i]["Mobile"]),
                    };
                    customers.Add(customer);
                }
                return customers;
            }
        }

        public Customer Get(int id)
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                SqlDataAdapter sqlDataAdapter = new($"SELECT * FROM Customers Where Id = @id", sqlConnection);
                sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@Id", id);
                DataTable dataTable = new();
                sqlDataAdapter.Fill(dataTable);

                Customer customer = new()
                {
                    Id = (int)dataTable.Rows[0]["Id"],
                    FirstName = (string)dataTable.Rows[0]["FirstName"],
                    LastName = (string)dataTable.Rows[0]["LastName"],
                    Gender = (string)dataTable.Rows[0]["Gender"],
                    Email = (string)dataTable.Rows[0]["Email"],
                    DateOfBirth = (DateTime)dataTable.Rows[0]["DateOfBirth"],
                    Mobile = (string)dataTable.Rows[0]["Mobile"],
                };
                return customer;
            }
        }

        public int Delete(int id)
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                string deleteQuery = "Delete From Customers Where Id = @Id";
                SqlCommand sqlCommand = new(deleteQuery, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@Id", id);
                sqlConnection.Open();
                int affectedRowCount = sqlCommand.ExecuteNonQuery();
                sqlConnection.Close();
                return affectedRowCount;
            }
        }

        public int Register(Customer customer)
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                string sqlQuery = @"INSERT INTO Customers
                   (FirstName, LastName, Gender, 
                    Email, DateOfBirth, Mobile)
                    VALUES 
                   (@FirstName, @LastName, @Gender, 
                    @Email, @DateOfBirth, @Mobile)";
                SqlCommand sqlCommand = new(sqlQuery, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@FirstName", customer.FirstName);
                sqlCommand.Parameters.AddWithValue("@LastName", customer.LastName);
                sqlCommand.Parameters.AddWithValue("@Gender", customer.Gender);
                sqlCommand.Parameters.AddWithValue("@Email", customer.Email);
                sqlCommand.Parameters.AddWithValue("@DateOfBirth", customer.DateOfBirth);
                sqlCommand.Parameters.AddWithValue("@Mobile", customer.Mobile);
                sqlConnection.Open();
                int affectedRowCount = sqlCommand.ExecuteNonQuery();
                sqlConnection.Close();
                return affectedRowCount;
            }
        }

        public int Update(Customer customer)
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                string sqlQuery = @" UPDATE Customers SET 
                   FirstName = @FirstName, 
                   LastName = @LastName, 
                   Gender = @Gender, 
                   Email = @Email, 
                   DateOfBirth = @DateOfBirth, 
                   Mobile = @Mobile
                   WHERE Id = @Id ";
                SqlCommand sqlCommand = new(sqlQuery, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@Id", customer.Id);
                sqlCommand.Parameters.AddWithValue("@FirstName", customer.FirstName);
                sqlCommand.Parameters.AddWithValue("@LastName", customer.LastName);
                sqlCommand.Parameters.AddWithValue("@Gender", customer.Gender);
                sqlCommand.Parameters.AddWithValue("@Email", customer.Email);
                sqlCommand.Parameters.AddWithValue("@DateOfBirth", customer.DateOfBirth);
                sqlCommand.Parameters.AddWithValue("@Mobile", customer.Mobile);
                sqlConnection.Open();
                int affectedRowCount = sqlCommand.ExecuteNonQuery();
                sqlConnection.Close();
                return affectedRowCount;
            }
        }
    }
}
