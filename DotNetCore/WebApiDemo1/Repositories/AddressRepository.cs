using Microsoft.Data.SqlClient;
using WebApiDemo1.DataModel;
using WebApiDemo1.DTO.InputDTO;

namespace WebApiDemo1.Repositories
{
    public class AddressRepository : IAddressRepository
    {
        private readonly string _connectionString;

        public AddressRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void AddAddress(Address address)
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                {
                    string sqlQuery = @"INSERT INTO Addresses(CustomerId, AddressLine1, AddressLine2, PinCode,
                            Country, AddressType, CreatedOn)
                            VALUES (@CustomerId, @AddressLine1, @AddressLine2, @PinCode, @Country, @AddressType,
                            @CreatedOn)
                            Select Scope_Identity()";
                    SqlCommand sqlCommand = new(sqlQuery, sqlConnection);
                    sqlCommand.Parameters.AddWithValue("@CustomerId", address.CustomerId);
                    sqlCommand.Parameters.AddWithValue("@AddressLine1", address.AddressLine1);
                    sqlCommand.Parameters.AddWithValue("@AddressLine2", address.AddressLine2);
                    sqlCommand.Parameters.AddWithValue("@PinCode", address.PinCode);
                    sqlCommand.Parameters.AddWithValue("@Country", address.Country);
                    sqlCommand.Parameters.AddWithValue("@AddressType", address.AddressType);
                    sqlCommand.Parameters.AddWithValue("@CreatedOn", DateTime.Now);
                    sqlConnection.Open();
                    sqlCommand.ExecuteNonQuery();
                    sqlConnection.Close();
                }
            }
        }

        public void UpdateAddress(Address address)
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                {
                    string sqlQuery = @" UPDATE Addresses SET AddressLine1 = @AddressLine1, AddressLine2 = @AddressLine2, 
                            PinCode = @PinCode, Country = @Country, AddressType = @AddressType, LastEditedOn = @LastEditedOn
                            WHERE CustomerId = @CustomerId ";
                    SqlCommand sqlCommand = new(sqlQuery, sqlConnection);
                    sqlCommand.Parameters.AddWithValue("@CustomerId", address.CustomerId);
                    sqlCommand.Parameters.AddWithValue("@AddressLine1", address.AddressLine1);
                    sqlCommand.Parameters.AddWithValue("@AddressLine2", address.AddressLine2);
                    sqlCommand.Parameters.AddWithValue("@PinCode", address.PinCode);
                    sqlCommand.Parameters.AddWithValue("@Country", address.Country);
                    sqlCommand.Parameters.AddWithValue("@AddressType", address.AddressType);
                    sqlCommand.Parameters.AddWithValue("@LastEditedOn", DateTime.Now);
                    sqlConnection.Open();
                    sqlCommand.ExecuteNonQuery();
                    sqlConnection.Close();
                }
            }
        }
    }
}
