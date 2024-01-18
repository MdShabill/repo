using ShopEase.DataModels.Customer;
using ShopEase.Enums;
using System.Data;
using System.Data.SqlClient;

namespace ShopEase.Repositories
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
                string sqlQuery = @"Select 
                           FullName, Mobile,  
                           Gender, Email
                           From 
                           Customers";
                SqlDataAdapter sqlDataAdapter = new(sqlQuery, sqlConnection);
                DataTable dataTable = new();
                sqlDataAdapter.Fill(dataTable);

                List<Customer> customers = new();

                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    Customer customer = new()
                    {
                        FullName = (string)dataTable.Rows[i]["FullName"],
                        Mobile = (string)dataTable.Rows[i]["Mobile"],
                        Gender = (GenderType)dataTable.Rows[i]["Gender"],
                        Email = (string)dataTable.Rows[i]["Email"],
                    };
                    customers.Add(customer);
                }
                return customers;
            }
        }

        public Customer GetCustomerById(int id)
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                string sqlQuery = @"SELECT Customers.Id, Customers.FullName, 
                            Customers.Gender, Customers.Email, Customers.Mobile,
                            Addresses.AddressLine1, Addresses.AddressLine2,
                            Addresses.Pincode, Addresses.AddressTypeId, AddressTypes.AddressTypeName,
                            Countries.Id as CountryId, Countries.CountryName
                            FROM Customers
                            INNER JOIN Addresses ON Customers.Id = Addresses.CustomerId
                            INNER JOIN Countries ON Addresses.CountryId = Countries.Id
                            INNER JOIN AddressTypes ON Addresses.AddressTypeId = AddressTypes.Id
                            WHERE Customers.Id = @id ";

                SqlDataAdapter sqlDataAdapter = new(sqlQuery, sqlConnection);
                sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@id", id);
                DataTable dataTable = new();
                sqlDataAdapter.Fill(dataTable);

                if (dataTable.Rows.Count > 0)
                {
                    Customer customer = new()
                    {
                        Id = (int)dataTable.Rows[0]["Id"],
                        FullName = (string)dataTable.Rows[0]["FullName"],
                        Gender = (GenderType)dataTable.Rows[0]["Gender"],
                        Email = (string)dataTable.Rows[0]["Email"],
                        Mobile = (string)dataTable.Rows[0]["Mobile"],
                        AddressLine1 = (string)dataTable.Rows[0]["AddressLine1"],
                        AddressLine2 = (string)dataTable.Rows[0]["AddressLine2"],
                        PinCode = (int)dataTable.Rows[0]["PinCode"],
                        AddressTypeId = (int)dataTable.Rows[0]["AddressTypeId"],
                        AddressTypeName = (string)dataTable.Rows[0]["AddressTypeName"],
                        CountryId = (int)dataTable.Rows[0]["CountryId"],
                        CountryName = (string)dataTable.Rows[0]["CountryName"]
                    };
                    return customer;
                }
                return null;
            }
        }

        public Customer GetCustomerDetailByEmail(string email)
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                string sqlQuery = @"Select Id, FullName, Mobile, Gender, 
                                    Email, Password, LoginFailedCount, IsLocked
                                    From 
                                    Customers 
                                    Where Email = @email ";

                SqlDataAdapter sqlDataAdapter = new(sqlQuery, sqlConnection);
                sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@email", email);
                DataTable dataTable = new();
                sqlDataAdapter.Fill(dataTable);

                if (dataTable.Rows.Count > 0)
                {
                    Customer customers = new()
                    {
                        Id = (int)dataTable.Rows[0]["Id"],
                        FullName = (string)dataTable.Rows[0]["FullName"],
                        Mobile = (string)dataTable.Rows[0]["Mobile"],
                        Gender = (GenderType)dataTable.Rows[0]["Gender"],
                        Email = (string)dataTable.Rows[0]["Email"],
                        Password = dataTable.Rows[0]["Password"] != DBNull.Value ? (string)dataTable.Rows[0]["Password"] : null,
                        LoginFailedCount = dataTable.Rows[0]["LoginFailedCount"] != DBNull.Value ? (int)dataTable.Rows[0]["LoginFailedCount"] : null,
                        IsLocked = dataTable.Rows[0]["IsLocked"] != DBNull.Value ? (bool)dataTable.Rows[0]["IsLocked"] : false
                    };
                    return customers;
                }
                return null;
            }
        }

        public void UpdateOnLoginSuccessfull(string email)
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                string sqlQuery = @"UPDATE Customers 
                                  SET 
                                  LastSuccessFulLoginDate = getdate(), 
                                  LoginFailedCount = 0
                                  WHERE Email = @email";
                SqlCommand sqlCommand = new(sqlQuery, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@email", email);
                sqlConnection.Open();
                sqlCommand.ExecuteNonQuery();
                sqlConnection.Close();
            }
        }

        public void UpdateOnLoginFailed(string email)
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                string sqlQuery = @"UPDATE Customers
                                    SET
                                    LoginFailedCount = ISNULL(LoginFailedCount, 0) + 1,
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
                                    Set 
                                    Islocked = @isLocked
                                    Where Email = @email";
                SqlCommand sqlCommand = new(sqlQuery, sqlConnection);
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
                string sqlQuery = @"Insert Into Customers
                       (FullName, Mobile, Gender, Email, Password)
                       Values
                       (@fullName, @mobile, @gender, @email, @password)
                       Select Scope_Identity()";

                SqlCommand sqlCommand = new(sqlQuery, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@fullName", customer.FullName);
                sqlCommand.Parameters.AddWithValue("@mobile", customer.Mobile);
                sqlCommand.Parameters.AddWithValue("@gender", customer.Gender);
                sqlCommand.Parameters.AddWithValue("@email", customer.Email);
                sqlCommand.Parameters.AddWithValue("@password", customer.Password);

                sqlConnection.Open();
                customer.Id = Convert.ToInt32(sqlCommand.ExecuteScalar());
                sqlConnection.Close();

                return customer.Id;
            }
        }

        public int Update(Customer customer)
        {
            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                string sqlQuery = @"UPDATE Customers SET 
                            FullName = @fullName,
                            Gender = @gender,
                            Mobile = @mobile
                            WHERE Id = @id";

                using (SqlCommand sqlCommand = new SqlCommand(sqlQuery, sqlConnection))
                {
                    sqlCommand.Parameters.AddWithValue("@id", customer.Id);
                    sqlCommand.Parameters.AddWithValue("@fullName", customer.FullName);
                    sqlCommand.Parameters.AddWithValue("@gender", customer.Gender);
                    sqlCommand.Parameters.AddWithValue("@mobile", customer.Mobile);

                    sqlConnection.Open();
                    int affectedRowCount = sqlCommand.ExecuteNonQuery();
                    sqlConnection.Close();
                    return affectedRowCount;
                }
            }
        }
    }
}
