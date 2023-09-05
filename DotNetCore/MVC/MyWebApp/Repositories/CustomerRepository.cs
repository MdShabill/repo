using MyWebApp.ViewModels;
using System.Data;
using Microsoft.Data.SqlClient;
using MyWebApp.Enums;
using MyWebApp.DataModel;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;

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
                string sqlQuery = "Select * from Customers ";

                SqlDataAdapter sqlDataAdapter = new(sqlQuery, sqlConnection);
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
                        Gender = (GenderType)dataTable.Rows[i]["Gender"],
                        Email = (string)dataTable.Rows[i]["Email"],
                        Password = (string)dataTable.Rows[i]["Password"],
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
                string sqlQuery = $"SELECT * FROM Customers Where Id = @id ";

                SqlDataAdapter sqlDataAdapter = new(sqlQuery, sqlConnection);
                sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@Id", id);
                DataTable dataTable = new();
                sqlDataAdapter.Fill(dataTable);

                Customer customer = new()
                {
                    Id = (int)dataTable.Rows[0]["Id"],
                    FirstName = (string)dataTable.Rows[0]["FirstName"],
                    LastName = (string)dataTable.Rows[0]["LastName"],
                    Gender = (GenderType)dataTable.Rows[0]["Gender"],
                    Email = (string)dataTable.Rows[0]["Email"],
                    Password = (string)dataTable.Rows[0]["Password"],
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

        public List<Customer> GetCustomers(CustomerSearch searchFilter)
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                string sqlQuery = @"Select * From Customers
                                    Where FirstName Like '%' + @firstName + '%' And
                                    LastName Like '%' + @lastName + '%' And
                                    Gender = @gender ";

                SqlDataAdapter sqlDataAdapter = new(sqlQuery, sqlConnection);
                sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@firstName", searchFilter.FirstName);
                sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@lastName", searchFilter.LastName);
                sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@gender", searchFilter.Gender);
                
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
                        Gender = (GenderType)dataTable.Rows[i]["Gender"],
                        Email = (string)dataTable.Rows[i]["Email"],
                        Password = (string)dataTable.Rows[i]["Password"],
                        DateOfBirth = (DateTime)dataTable.Rows[i]["DateOfBirth"],
                        Mobile = Convert.ToString(dataTable.Rows[i]["Mobile"]),
                    };
                    customers.Add(customer);
                }
                return customers;
            }
        }

        public List<Customer> GetCustomersOptional(CustomerSearchOptional optionalFilter)
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                string sqlQuery = "Select * From Customers Where 1=1 ";


                if (!string.IsNullOrEmpty(optionalFilter.FirstName))
                    sqlQuery += " And FirstName Like '%' + @firstName + '%' ";

                if (!string.IsNullOrEmpty(optionalFilter.LastName))
                    sqlQuery += " And LastName Like '%' + @lastName + '%'";

                if (optionalFilter.Gender != 0)
                    sqlQuery += " And Gender = @gender ";
                    

                SqlDataAdapter sqlDataAdapter = new(sqlQuery, sqlConnection);

                if(!string.IsNullOrEmpty(optionalFilter.FirstName))
                    sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@firstName", optionalFilter.FirstName);

                if (!string.IsNullOrEmpty(optionalFilter.LastName))
                    sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@lastName", optionalFilter.LastName);

                if (optionalFilter.Gender != 0)
                    sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@gender", optionalFilter.Gender);

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
                        Gender = (GenderType)dataTable.Rows[i]["Gender"],
                        Email = (string)dataTable.Rows[i]["Email"],
                        Password = (string)dataTable.Rows[i]["Password"],
                        DateOfBirth = (DateTime)dataTable.Rows[i]["DateOfBirth"],
                        Mobile = Convert.ToString(dataTable.Rows[i]["Mobile"]),
                    };
                    customers.Add(customer);
                }
                return customers;
            }
        }

        public Customer GetCustomerDetailsByEmail(string email)
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                string sqlQuery = @"
                        SELECT * FROM Customers
                        WHERE 
                        Email = @email ";

                SqlDataAdapter sqlDataAdapter = new(sqlQuery, sqlConnection);
                sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@email", email);
                DataTable dataTable = new();
                sqlDataAdapter.Fill(dataTable);

                if (dataTable.Rows.Count > 0)
                {
                    Customer customers = new()
                    {
                        Id = (int)dataTable.Rows[0]["Id"],
                        FirstName = (string)dataTable.Rows[0]["FirstName"],
                        LastName = (string)dataTable.Rows[0]["LastName"],
                        Gender = (GenderType)dataTable.Rows[0]["Gender"],
                        Email = (string)dataTable.Rows[0]["Email"],
                        Password = dataTable.Rows[0]["Password"] != DBNull.Value ? (string)dataTable.Rows[0]["Password"] : null,
                        DateOfBirth = (DateTime)dataTable.Rows[0]["DateOfBirth"],
                        Mobile = Convert.ToString(dataTable.Rows[0]["Mobile"]),
                        LoginFailedCount = dataTable.Rows[0]["LoginFailedCount"] != DBNull.Value ? (int)dataTable.Rows[0]["LoginFailedCount"] : null,
                        IsLocked = dataTable.Rows[0]["IsLocked"] != DBNull.Value ? (bool)dataTable.Rows[0]["IsLocked"]:false
                    };
                    return customers;
                }
                return null;
            }
        }

        public void UpdateOnLoginSuccessful(string email)
        {
            using (SqlConnection sqlconnection = new(_connectionString))
            {
                string sqlQuery = @"Update Customers 
                                  Set LastSuccessfulLoginDate = getDate()
                                  Where Email = @email ";
                SqlCommand sqlCommand = new(sqlQuery, sqlconnection);
                sqlCommand.Parameters.AddWithValue("@email", email);
                sqlconnection.Open();
                sqlCommand.ExecuteNonQuery();
                sqlconnection.Close();  
            }
        }

        public void UpdateOnLoginFailed(string email)
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                string sqlQuery = @"UPDATE Customers
                                    SET LoginFailedCount = ISNULL(LoginFailedCount, 0) + 1,
                                    LastFailedLoginDate = GETDATE()
                                    WHERE Email = @email ";
                SqlCommand sqlCommand = new(sqlQuery, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@Email", email);
                sqlConnection.Open();
                sqlCommand.ExecuteNonQuery();
                sqlConnection.Close();
            }
        }

        public void UpdateIsLocked(string email, bool isLocked)
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                string sqlQuery = @"Update Customers 
                                    Set Islocked = @isLocked
                                    Where Email = @email";
                SqlCommand sqlCommand = new (sqlQuery, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@email", email);
                sqlCommand.Parameters.AddWithValue("@isLocked", isLocked);

                sqlConnection.Open();
                sqlCommand.ExecuteNonQuery();
                sqlConnection.Close();
            }
        }

        public int Register(Customer customer)
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                string sqlQuery = @"INSERT INTO Customers
                   (FirstName, LastName, Gender, 
                    Email, Password, DateOfBirth, Mobile)
                    VALUES 
                   (@FirstName, @LastName, @Gender, 
                    @Email, @Password, @DateOfBirth, @Mobile)";
                SqlCommand sqlCommand = new(sqlQuery, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@FirstName", customer.FirstName);
                sqlCommand.Parameters.AddWithValue("@LastName", customer.LastName);
                sqlCommand.Parameters.AddWithValue("@Gender", customer.Gender);
                sqlCommand.Parameters.AddWithValue("@Email", customer.Email);
                sqlCommand.Parameters.AddWithValue("@Password", customer.Password);
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
                   Password = @Password, 
                   DateOfBirth = @DateOfBirth, 
                   Mobile = @Mobile
                   WHERE Id = @Id ";
                SqlCommand sqlCommand = new(sqlQuery, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@Id", customer.Id);
                sqlCommand.Parameters.AddWithValue("@FirstName", customer.FirstName);
                sqlCommand.Parameters.AddWithValue("@LastName", customer.LastName);
                sqlCommand.Parameters.AddWithValue("@Gender", customer.Gender);
                sqlCommand.Parameters.AddWithValue("@Email", customer.Email);
                sqlCommand.Parameters.AddWithValue("@Password", customer.Password);
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
