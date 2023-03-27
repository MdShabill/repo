using Microsoft.Data.SqlClient;
using System.Data;
using System.Diagnostics.Metrics;
using System.Text;
using System.Security.Cryptography;
using WebApiDemo1.DTO.InputDTO;
using WebApiDemo1.Enums;
using WebApiDemo1.DTO.InputDTO;
using System.Collections;
using WebApiDemo1.Helpers;
using WebApiDemo1.DataModel;

namespace WebApiDemo1.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly string _connectionString;

        public CustomerRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<CustomerDto> GetAllCustomersAsList()
        {
            List<CustomerDto> customers = new();

            using (SqlConnection sqlConnection = new(_connectionString))
            {
                SqlDataAdapter sqlDataAdapter = new("SELECT * FROM Customers", sqlConnection);
                DataTable dataTable = new();
                sqlDataAdapter.Fill(dataTable);

                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    //Approch 1
                    //CustomerDto customerDto = new();
                    //customerDto.Id = (int)dataTable.Rows[i]["Id"];
                    //customerDto.FullName = (string)dataTable.Rows[i]["Name"];
                    //customerDto.Gender = (GenderTypes)dataTable.Rows[i]["Gender"];
                    //customerDto.Age = (int)dataTable.Rows[i]["Age"];
                    //customerDto.Email = (string)dataTable.Rows[i]["Email"];
                    //customerDto.Password = (string)dataTable.Rows[i]["Password"];
                    //customerDto.MobileNumber = (string)dataTable.Rows[i]["MobileNumber"];
                    //customerDto.Country = (string)dataTable.Rows[i]["Country"];
                    //customers.Add(customerDto);

                    ////Approch 2
                    //CustomerDto customerDto = new()
                    //{
                    //     Id = (int)dataTable.Rows[i]["Id"],
                    //     FullName = (string)dataTable.Rows[i]["Name"],
                    //     Gender = (GenderTypes)dataTable.Rows[i]["Gender"],
                    //     Age = (int)dataTable.Rows[i]["Age"],
                    //     Email = (string)dataTable.Rows[i]["Email"],
                    //     Password = (string)dataTable.Rows[i]["Password"],
                    //     MobileNumber = (string)dataTable.Rows[i]["MobileNumber"],
                    //};

                    //Approch 3 - Using Constructor
                    CustomerDto customerDto = new(
                        (int)dataTable.Rows[i]["Id"], (string)dataTable.Rows[i]["Name"], (GenderTypes)dataTable.Rows[i]["Gender"],
                        (int)dataTable.Rows[i]["Age"], (string)dataTable.Rows[i]["Email"], (string)dataTable.Rows[i]["Password"],
                        dataTable.Rows[i]["Mobile"] != DBNull.Value ? (string)dataTable.Rows[i]["Mobile"] : null,
                        dataTable.Rows[i]["Country"] != DBNull.Value ? (string)dataTable.Rows[i]["Country"] : null);

                    customers.Add(customerDto);
                }
                return customers;
            }
        }

        public int GetCustomersCount()
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                string sqlQuery = "SELECT COUNT(*) FROM Customers ";
                SqlCommand sqlCommand = new(sqlQuery, sqlConnection);
                sqlConnection.Open();
                int customerCount = Convert.ToInt32(sqlCommand.ExecuteScalar());
                sqlConnection.Close();
                return customerCount;
            }
        }

        public CustomerDto GetAllCustomerById(int id)
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                SqlDataAdapter sqlDataAdapter = new($"SELECT * FROM Customers Where Id = @id", sqlConnection);
                sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@Id", id);
                DataTable dataTable = new();
                sqlDataAdapter.Fill(dataTable);

                if (dataTable.Rows.Count > 0)
                {
                    //Approch 1
                    //CustomerDto customerDto = new();
                    //customerDto.Id = (int)dataTable.Rows[0]["Id"];
                    //customerDto.FullName = (string)dataTable.Rows[0]["Name"];
                    //customerDto.Gender = (GenderTypes)dataTable.Rows[0]["Gender"];
                    //customerDto.Age = (int)dataTable.Rows[0]["Age"];
                    //customerDto.Email = (string)dataTable.Rows[0]["Email"];
                    //customerDto.Password = (string)dataTable.Rows[0]["Password"];
                    //customerDto.MobileNumber = (string)dataTable.Rows[0]["MobileNumber"];
                    //customerDto.Country = (string)dataTable.Rows[0]["Country"];
                    //return customerDto;

                    //Approch 2
                    CustomerDto customerDto = new()
                    {
                        Id = (int)dataTable.Rows[0]["Id"],
                        FullName = (string)dataTable.Rows[0]["Name"],
                        Gender = (GenderTypes)dataTable.Rows[0]["Gender"],
                        Age = (int)dataTable.Rows[0]["Age"],
                        Email = (string)dataTable.Rows[0]["Email"],
                        Password = (string)dataTable.Rows[0]["Password"],
                        MobileNumber = (string)dataTable.Rows[0]["MobileNumber"],
                    };
                    return customerDto;
                }
                else
                    return null;
            }
        }

        public string GetCustomerFullNameById(int customerId)
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                string sqlQuery = "SELECT Name FROM Customers WHERE Id = @customerId";
                SqlCommand sqlCommand = new(sqlQuery, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@customerId", customerId);
                sqlConnection.Open();
                string customerFullName = Convert.ToString(sqlCommand.ExecuteScalar());
                sqlConnection.Close();
                return customerFullName;
            }
        }

        public List<CustomerDto> GetCustomersDetailByGenderByCountry(int gender, string country)
        {
            List<CustomerDto> customers = new();

            using (SqlConnection sqlConnection = new(_connectionString))
            {
                SqlDataAdapter sqlDataAdapter = new(@"SELECT * FROM Customers WHERE Gender = @gender
                            AND Country = @country ", sqlConnection);
                sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@gender", gender);
                sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@country", country);
                DataTable dataTable = new();
                sqlDataAdapter.Fill(dataTable);

                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    //Approch 1
                    //CustomerDto customerDto = new();
                    //customerDto.Id = (int)dataTable.Rows[i]["Id"];
                    //customerDto.FullName = (string)dataTable.Rows[i]["Name"];
                    //customerDto.Gender = (GenderTypes)dataTable.Rows[i]["Gender"];
                    //customerDto.Age = (int)dataTable.Rows[i]["Age"];
                    //customerDto.Email = (string)dataTable.Rows[i]["Email"];
                    //customerDto.Password = (string)dataTable.Rows[i]["Password"];
                    //customerDto.MobileNumber = (string)dataTable.Rows[i]["MobileNumber"];
                    //customerDto.Country = (string)dataTable.Rows[i]["Country"];
                    //customers.Add(customerDto);

                    //Approch 2
                    CustomerDto customerDto = new()
                    {
                        Id = (int)dataTable.Rows[i]["Id"],
                        FullName = (string)dataTable.Rows[i]["Name"],
                        Gender = (GenderTypes)dataTable.Rows[i]["Gender"],
                        Age = (int)dataTable.Rows[i]["Age"],
                        Email = (string)dataTable.Rows[i]["Email"],
                        Password = (string)dataTable.Rows[i]["Password"],
                        MobileNumber = (string)dataTable.Rows[i]["MobileNumber"],
                    };
                    customers.Add(customerDto);
                }
                return customers;
            }
        }

        public CustomerDto GetCustomerDetailsByEmailAndPassword(string email, byte[] password)
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                SqlDataAdapter sqlDataAdapter = new(@"SELECT * FROM Customers
                        WHERE Email = @email AND Password = @password ", sqlConnection);
                sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@email", email);
                sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@password", password);
                DataTable dataTable = new();
                sqlDataAdapter.Fill(dataTable);

                if (dataTable.Rows.Count > 0)
                {
                    CustomerDto customerDto = new()
                    {
                        Id = (int)dataTable.Rows[0]["Id"],
                        FullName = (string)dataTable.Rows[0]["Name"],
                        Gender = (GenderTypes)dataTable.Rows[0]["Gender"],
                        Age = (int)dataTable.Rows[0]["Age"],
                        Email = (string)dataTable.Rows[0]["Email"],
                        Password = (string)dataTable.Rows[0]["Password"],
                        MobileNumber = (string)dataTable.Rows[0]["MobileNumber"],
                        LastSuccessfulLoginDate = dataTable.Rows[0]["LastSuccessfulLoginDate"] != DBNull.Value ? (DateTime)dataTable.Rows[0]["LastSuccessfulLoginDate"] : null,
                        LastFailedLoginDate = dataTable.Rows[0]["LastFailedLoginDate"] != DBNull.Value ? (DateTime)dataTable.Rows[0]["LastFailedLoginDate"] : null,
                        LoginFailedCount = dataTable.Rows[0]["LoginFailedCount"] != DBNull.Value ? (int)dataTable.Rows[0]["LoginFailedCount"] : null,
                        IsLocked = dataTable.Rows[0]["IsLocked"] != DBNull.Value ? (bool)dataTable.Rows[0]["IsLocked"] : false
                        //if (dataTable.Rows[0]["IsLocked"] != DBNull.Value)
                        //    customerDto.IsLocked = (bool)dataTable.Rows[0]["IsLocked"];
                        //else
                        //    customerDto.IsLocked = false
                    };
                    return customerDto;
                }
                return null;
            }
        }

        public void UpdateOnLoginFailed(string email)
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                string sqlQuery = $@"UPDATE Customers SET LoginFailedCount = IsNull(LoginFailedCount, 0) + 1,
                                    LastFailedLoginDate = getdate()
                                    WHERE Email = @email";
                SqlCommand sqlCommand = new(sqlQuery, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@email", email);
                sqlConnection.Open();
                sqlCommand.ExecuteNonQuery();
                sqlConnection.Close();
            }
        }

        public void UpdateOnLoginSuccessfull(string email)
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                DateTime lastSucccessfulLoginDate = DateTime.Now;
                string sqlQuery = @"UPDATE Customers SET LastSuccessfulLoginDate = getdate(), LoginFailedCount = 0
                                    WHERE Email = @email";
                SqlCommand sqlCommand = new(sqlQuery, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@email", email);
                sqlConnection.Open();
                sqlCommand.ExecuteNonQuery();
                sqlConnection.Close();
            }
        }

        public int GetLoginFailedCount(string email)
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                string sqlQuery = "SELECT IsNull(LoginFailedCount, 0) FROM Customers WHERE Email = @Email";
                SqlCommand sqlCommand = new(sqlQuery, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@email", email);
                sqlConnection.Open();
                int loginFailedCount = Convert.ToInt32(sqlCommand.ExecuteScalar());
                sqlConnection.Close();
                return loginFailedCount;
            }
        }

        public void UpdateIsLocked(string email, bool isLocked)
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                string sqlQuery = @"UPDATE Customers SET IsLocked = @isLocked
                                    WHERE Email = @email";
                SqlCommand sqlCommand = new(sqlQuery, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@email", email);
                sqlCommand.Parameters.AddWithValue("@isLocked", isLocked);
                sqlConnection.Open();
                sqlCommand.ExecuteNonQuery();
                sqlConnection.Close();
            }
        }

        public void UpdateNewPassword(string email, byte[] password)
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                string sqlQuery = @"UPDATE Customers SET Password = @newPassword
                                    WHERE Email = @email";
                SqlCommand sqlCommand = new(sqlQuery, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@email", email);
                sqlCommand.Parameters.AddWithValue("@newPassword", password);
                sqlConnection.Open();
                sqlCommand.ExecuteNonQuery();
                sqlConnection.Close();
            }
        }

        public int Add(Customer customer)
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                string sqlQuery = @"INSERT INTO Customers(Name, Gender, Age, Email, Password, MobileNumber)
                            VALUES (@FullName, @Gender, @Age, @Email, @Password, @Mobilenumber)
                            Select Scope_Identity()";
                SqlCommand sqlCommand = new(sqlQuery, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@FullName", customer.FullName);
                sqlCommand.Parameters.AddWithValue("@Gender", customer.Gender);
                sqlCommand.Parameters.AddWithValue("@Age", customer.Age);
                sqlCommand.Parameters.AddWithValue("@Email", customer.Email);
                sqlCommand.Parameters.AddWithValue("@Password", customer.HashValuePassword);
                sqlCommand.Parameters.AddWithValue("@MobileNumber", customer.MobileNumber);
                sqlConnection.Open();
                customer.Id = Convert.ToInt32(sqlCommand.ExecuteScalar());
                sqlConnection.Close();
                return customer.Id;
            }
        }

        public void Update(Customer customer)
        {
            //Approach #1  - Recommended
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                string sqlQuery = @" UPDATE Customers SET Name = @FullName, Gender = @Gender, Age = @Age,
                            Email = @Email, Password = @Password, MobileNumber = @MobileNumber
                            WHERE Id = @Id ";
                SqlCommand sqlCommand = new(sqlQuery, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@Id", customer.Id);
                sqlCommand.Parameters.AddWithValue("@FullName", customer.FullName);
                sqlCommand.Parameters.AddWithValue("@Gender", customer.Gender);
                sqlCommand.Parameters.AddWithValue("@Age", customer.Age);
                sqlCommand.Parameters.AddWithValue("@Email", customer.Email);
                sqlCommand.Parameters.AddWithValue("@Password", customer.HashValuePassword);
                sqlCommand.Parameters.AddWithValue("@MobileNumber", customer.MobileNumber);
                sqlConnection.Open();
                sqlCommand.ExecuteNonQuery();
                sqlConnection.Close();
            }
            ////Approach #2
            //SqlConnection sqlConnection = new(_connectionString);
            //string sqlQuery = @" UPDATE Customers SET Name = @FullName, Gender = @Gender,
            //                         Age = @Age
            //                         WHERE Id = @Id ";
            //SqlCommand sqlCommand = new(sqlQuery, sqlConnection);
            //sqlCommand.Parameters.AddWithValue("@Id", customer.Id);
            //sqlCommand.Parameters.AddWithValue("@FullName", customer.FullName);
            //sqlCommand.Parameters.AddWithValue("@Gender", customer.Gender);
            //sqlCommand.Parameters.AddWithValue("@Age", customer.Age);
            //sqlConnection.Open();
            //sqlCommand.ExecuteNonQuery();
            //sqlConnection.Close();
        }
    }
}
