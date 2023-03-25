using Microsoft.Data.SqlClient;
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

        public void AddAddress(CustomerDto customer)
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                {
                    string sqlQuery = @"INSERT INTO Addresses(CustomerId, AddressLine1, AddressLine2, PinCode,
                            Country, AddressType, CreatedOn, LastEditedOn)

                            VALUES (@CustomerId, @AddressLine1, @AddressLine2, @PinCode, @Country, @AddressType,
                            @CreatedOn, @LastEditedOn)
                            Select Scope_Identity()";
                    SqlCommand sqlCommand = new(sqlQuery, sqlConnection);
                    sqlCommand.Parameters.AddWithValue("@CustomerId", customer.Id);
                    sqlCommand.Parameters.AddWithValue("@AddressLine1", customer.AddressLine1);
                    sqlCommand.Parameters.AddWithValue("@AddressLine2", customer.AddressLine2);
                    sqlCommand.Parameters.AddWithValue("@PinCode", customer.PinCode);
                    sqlCommand.Parameters.AddWithValue("@Country", customer.Country);
                    sqlCommand.Parameters.AddWithValue("@AddressType", customer.AddressType);
                    sqlCommand.Parameters.AddWithValue("@CreatedOn", customer.CreatedOn);
                    sqlCommand.Parameters.AddWithValue("@LastEditedOn", customer.LastEditedOn);
                    sqlConnection.Open();
                    sqlCommand.ExecuteNonQuery();
                    sqlConnection.Close();
                }
            }
        }
    }
}
