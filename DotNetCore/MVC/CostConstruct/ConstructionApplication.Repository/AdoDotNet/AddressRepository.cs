using ConstructionApplication.Core.DataModels.Address;
using ConstructionApplication.Core.DataModels.ServiceProviders;
using ConstructionApplication.Repository.Interfaces;
using System.Data.SqlClient;

namespace ConstructionApplication.Repository.AdoDotNet
{
    public class AddressRepository : IAddressRepository
    {
        private readonly string _connectionString;

        public AddressRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void InsertOrUpdateAddress(Address address)
        {
            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                // Check if an address record exists for the given ServiceProviderId
                string checkQuery = "SELECT COUNT(*) FROM Addresses WHERE ServiceProviderId = @serviceProviderId";
                SqlCommand checkCommand = new SqlCommand(checkQuery, sqlConnection);
                checkCommand.Parameters.AddWithValue("@serviceProviderId", address.ServiceProviderId);

                sqlConnection.Open();
                int addressCount = (int)checkCommand.ExecuteScalar();

                if (addressCount > 0)
                {
                    string updateQuery = @"
                                UPDATE Addresses SET 
                                AddressLine1 = @addressLine1,
                                AddressTypeId = @addressTypeId,
                                CountryId = @countryId,
                                PinCode = @pinCode
                                WHERE ServiceProviderId = @serviceProviderId";

                    SqlCommand updateCommand = new SqlCommand(updateQuery, sqlConnection);
                    updateCommand.Parameters.AddWithValue("@addressLine1", address.AddressLine1 ?? (object)DBNull.Value);
                    updateCommand.Parameters.AddWithValue("@addressTypeId", address.AddressTypeId);
                    updateCommand.Parameters.AddWithValue("@countryId", address.CountryId);
                    updateCommand.Parameters.AddWithValue("@pinCode", address.PinCode);
                    updateCommand.Parameters.AddWithValue("@serviceProviderId", address.ServiceProviderId);
                    updateCommand.ExecuteNonQuery();
                }
                else
                {
                    string insertQuery = @"
                               INSERT INTO Addresses 
                                      (ServiceProviderId, AddressLine1, AddressTypeId, CountryId, PinCode)
                                VALUES 
                                      (@serviceProviderId, @addressLine1, @addressTypeId, @countryId, @pinCode)";

                    SqlCommand insertCommand = new SqlCommand(insertQuery, sqlConnection);
                    insertCommand.Parameters.AddWithValue("@serviceProviderId", address.ServiceProviderId);
                    insertCommand.Parameters.AddWithValue("@addressLine1", address.AddressLine1 ?? (object)DBNull.Value);
                    insertCommand.Parameters.AddWithValue("@addressTypeId", address.AddressTypeId);
                    insertCommand.Parameters.AddWithValue("@countryId", address.CountryId);
                    insertCommand.Parameters.AddWithValue("@pinCode", address.PinCode);
                    insertCommand.ExecuteNonQuery();
                }
                sqlConnection.Close();
            }
        }

        public void Delete(int serviceProviderId)
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                string deleteAddressesQuery = @"DELETE FROM Addresses WHERE ServiceProviderId = @serviceProviderId";
                SqlCommand deleteAddressesCommand = new(deleteAddressesQuery, sqlConnection);
                deleteAddressesCommand.Parameters.AddWithValue("@serviceProviderId", serviceProviderId);
                sqlConnection.Open();
                deleteAddressesCommand.ExecuteNonQuery();
                sqlConnection.Close();
            }
        }
    }
}
