using ShopEase.DataModels;
using ShopEase.ViewModels;
using System.Data;
using System.Data.SqlClient;

namespace ShopEase.Repositories
{
    public class AddressRepository : IAddressRepository
    {
        private readonly string _connectionString;

        public AddressRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void Add(Address address)
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                {
                    string sqlQuery = @"INSERT INTO Addresses
                                (CustomerId, AddressLine1, AddressLine2, 
                            PinCode, CountryId, AddressTypeId, CreatedOn)
                            VALUES 
                            (@customerId, @addressLine1, @addressLine2, 
                            @pinCode, @countryId, @addressTypeId,
                            @createdOn)";

                    SqlCommand sqlCommand = new(sqlQuery, sqlConnection);
                    sqlCommand.Parameters.AddWithValue("@customerId", address.CustomerId);
                    sqlCommand.Parameters.AddWithValue("@addressLine1", address.AddressLine1);
                    sqlCommand.Parameters.AddWithValue("@addressLine2", address.AddressLine2);
                    sqlCommand.Parameters.AddWithValue("@pinCode", address.PinCode);
                    sqlCommand.Parameters.AddWithValue("@countryId", address.CountryId);
                    sqlCommand.Parameters.AddWithValue("@addressTypeId", address.AddressTypeId);
                    sqlCommand.Parameters.AddWithValue("@createdOn", DateTime.Now);
                    sqlConnection.Open();
                    sqlCommand.ExecuteNonQuery();
                    sqlConnection.Close();
                }
            }
        }

        public List<Address> GetAllAddress(int customerId)
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                string sqlQuery = @"SELECT Addresses.Id, ' - ', CONCAT(Addresses.Id, Addresses.AddressLine1, ' - ', 
                            Addresses.AddressLine2, ' - ', Addresses.PinCode, 
                            ' - ', Countries.CountryName, ' - ', AddressTypes.AddressTypeName) As AddressDetail
                            FROM Addresses
                            INNER JOIN Countries ON Addresses.CountryId = Countries.Id
                            INNER JOIN AddressTypes ON Addresses.AddressTypeID = AddressTypes.Id
                            Where CustomerId = @customerId";
                SqlDataAdapter sqlDataAdapter = new(sqlQuery, sqlConnection);
                sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@customerId", customerId);
                DataTable dataTable = new();
                sqlDataAdapter.Fill(dataTable);

                List<Address> addresses = new();
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    Address address = new()
                    {
                        Id = (int)dataTable.Rows[0]["Id"],
                        AddressDetail = dataTable.Rows[0]["AddressDetail"].ToString()
                    };
                    addresses.Add(address);
                }
                return addresses;
            }
        }

        public void Update(Address address)
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                {
                    string sqlQuery = @" UPDATE Addresses SET 
                                 AddressLine1 = @addressLine1, 
                                 AddressLine2 = @addressLine2, 
                                 PinCode = @pinCode, 
                                 CountryId = @countryId, 
                                 AddressTypeId = @addressTypeId 
                                 WHERE CustomerId = @CustomerId ";
                    SqlCommand sqlCommand = new(sqlQuery, sqlConnection);
                    sqlCommand.Parameters.AddWithValue("@CustomerId", address.CustomerId);
                    sqlCommand.Parameters.AddWithValue("@AddressLine1", address.AddressLine1);
                    sqlCommand.Parameters.AddWithValue("@AddressLine2", address.AddressLine2);
                    sqlCommand.Parameters.AddWithValue("@PinCode", address.PinCode);
                    sqlCommand.Parameters.AddWithValue("@countryId", address.CountryId);
                    sqlCommand.Parameters.AddWithValue("@addressTypeId", address.AddressTypeId);
                    sqlConnection.Open();
                    sqlCommand.ExecuteNonQuery();
                    sqlConnection.Close();
                }
            }
        }
    }
}
