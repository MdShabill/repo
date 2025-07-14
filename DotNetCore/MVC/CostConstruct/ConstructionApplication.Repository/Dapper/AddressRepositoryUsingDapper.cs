using ConstructionApplication.Core.DataModels.Address;
using ConstructionApplication.Core.DataModels.ServiceProviders;
using ConstructionApplication.Repository.Interfaces;
using Dapper;
using System.Data;
using System.Data.SqlClient;

namespace ConstructionApplication.Repository.Dapper
{
    public class AddressRepositoryUsingDapper : IAddressRepository
    {
        private readonly string _connectionString;

        public AddressRepositoryUsingDapper(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void InsertOrUpdateAddress(Address address)
        {
            using (IDbConnection connection = new SqlConnection(_connectionString))
            {
                // Check if an address record exists for the given ServiceProviderId
                string checkQuery = "SELECT COUNT(*) FROM Addresses WHERE ServiceProviderId = @ServiceProviderId";
                int addressCount = connection.ExecuteScalar<int>(checkQuery, new { address.ServiceProviderId });

                if (addressCount > 0)
                {
                    // Update existing address
                    string updateQuery = @"
                           UPDATE Addresses SET 
                                  AddressLine1 = @AddressLine1,
                                  AddressTypeId = @AddressTypeId,
                                  CountryId = @CountryId,
                                  PinCode = @PinCode
                           WHERE ServiceProviderId = @ServiceProviderId";

                    connection.Execute(updateQuery, address);
                }
                else
                {
                    // Insert new address
                    string insertQuery = @"
                           INSERT INTO Addresses 
                               (ServiceProviderId, AddressLine1, AddressTypeId, CountryId, PinCode)
                           VALUES 
                               (@ServiceProviderId, @AddressLine1, @AddressTypeId, @CountryId, @PinCode)";

                    connection.Execute(insertQuery, address);
                }
            }
        }

        public void Delete(int serviceProviderId)
        {
            using (IDbConnection connection = new SqlConnection(_connectionString))
            {
                string deleteQuery = "DELETE FROM Addresses WHERE ServiceProviderId = @ServiceProviderId";
                connection.Execute(deleteQuery, new { ServiceProviderId = serviceProviderId });
            }
        }
    }
}
