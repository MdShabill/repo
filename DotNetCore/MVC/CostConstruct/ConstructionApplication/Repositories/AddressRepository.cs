using ConstructionApplication.DataModels.Address;
using System.Data.SqlClient;

namespace ConstructionApplication.Repositories
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
                            (ContractorId, AddressTypeId, CountryId, AddressLine1, PinCode)
                            VALUES 
                            (@contractorId, @addressTypeId, @countryId, @addressLine1, @pinCode)";

                    SqlCommand sqlCommand = new(sqlQuery, sqlConnection);
                    sqlCommand.Parameters.AddWithValue("@contractorId", address.ContractorId);
                    sqlCommand.Parameters.AddWithValue("@addressTypeId", address.AddressTypeId);
                    sqlCommand.Parameters.AddWithValue("@countryId", address.CountryId);
                    sqlCommand.Parameters.AddWithValue("@addressLine1", address.AddressLine1);
                    sqlCommand.Parameters.AddWithValue("@pinCode", address.PinCode);
                    sqlConnection.Open();
                    sqlCommand.ExecuteNonQuery();
                    sqlConnection.Close();
                }
            }
        }

        public void Delete(int contractorId)
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                string deleteAddressesQuery = @"DELETE FROM Addresses WHERE ContractorId = @contractorId";
                SqlCommand deleteAddressesCommand = new(deleteAddressesQuery, sqlConnection);
                deleteAddressesCommand.Parameters.AddWithValue("@contractorId", contractorId);
                sqlConnection.Open();
                deleteAddressesCommand.ExecuteNonQuery();
                sqlConnection.Close();
            }
        }

        public void Update(Address address)
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                {
                    string sqlQuery = @" UPDATE Addresses SET 
                                 AddressTypeId = @addressTypeId,
                                 CountryId = @countryId,
                                 AddressLine1 = @addressLine1,  
                                 PinCode = @pinCode  
                                 WHERE ContractorId = @contractorId ";
                    SqlCommand sqlCommand = new(sqlQuery, sqlConnection);
                    sqlCommand.Parameters.AddWithValue("@contractorId", address.ContractorId);
                    sqlCommand.Parameters.AddWithValue("@addressTypeId", address.AddressTypeId);
                    sqlCommand.Parameters.AddWithValue("@countryId", address.CountryId);
                    sqlCommand.Parameters.AddWithValue("@addressLine1", address.AddressLine1);
                    sqlCommand.Parameters.AddWithValue("@pinCode", address.PinCode);
                    sqlConnection.Open();
                    sqlCommand.ExecuteNonQuery();
                    sqlConnection.Close();
                }
            }
        }
    }
}
