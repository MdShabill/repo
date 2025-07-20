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
                sqlConnection.Open();
                bool isServiceProvider = address.ServiceProviderId > 0;
                bool isSite = address.SiteId > 0;

                // Check if an address record exists for the given ServiceProviderId or SiteId
                string checkQuery = @"SELECT COUNT(*) 
                                    FROM 
                                        Addresses 
                                    WHERE " + (isServiceProvider ? "ServiceProviderId = @id" : "SiteId = @id");

                SqlCommand checkCommand = new SqlCommand(checkQuery, sqlConnection);
                checkCommand.Parameters.AddWithValue("@id", isServiceProvider ? address.ServiceProviderId : address.SiteId);

                int addressCount = (int)checkCommand.ExecuteScalar();

                if (addressCount > 0)
                {
                    string updateQuery = @"
                                UPDATE Addresses SET 
                                AddressLine1 = @addressLine1,
                                AddressTypeId = @addressTypeId,
                                CountryId = @countryId,
                                PinCode = @pinCode
                                WHERE " + (isServiceProvider ? "ServiceProviderId = @id" : "SiteId = @id");

                    SqlCommand updateCommand = new SqlCommand(updateQuery, sqlConnection);
                    updateCommand.Parameters.AddWithValue("@addressLine1", address.AddressLine1 ?? (object)DBNull.Value);
                    updateCommand.Parameters.AddWithValue("@addressTypeId", address.AddressTypeId);
                    updateCommand.Parameters.AddWithValue("@countryId", address.CountryId);
                    updateCommand.Parameters.AddWithValue("@pinCode", address.PinCode);
                    updateCommand.Parameters.AddWithValue("@id", isServiceProvider ? address.ServiceProviderId : address.SiteId);
                    updateCommand.ExecuteNonQuery();
                }
                else
                {
                    string insertQuery = @"
                               INSERT INTO Addresses 
                                      (ServiceProviderId, SiteId, AddressLine1, AddressTypeId, CountryId, PinCode)
                                VALUES 
                                      (@serviceProviderId, @siteId, @addressLine1, @addressTypeId, @countryId, @pinCode)";

                    SqlCommand insertCommand = new SqlCommand(insertQuery, sqlConnection);
                    insertCommand.Parameters.AddWithValue("@serviceProviderId", isServiceProvider ? address.ServiceProviderId : (object)DBNull.Value);

                    insertCommand.Parameters.AddWithValue("@siteId", isSite ? address.SiteId : (object)DBNull.Value);
                    insertCommand.Parameters.AddWithValue("@addressLine1", address.AddressLine1 ?? (object)DBNull.Value);
                    insertCommand.Parameters.AddWithValue("@addressTypeId", address.AddressTypeId);
                    insertCommand.Parameters.AddWithValue("@countryId", address.CountryId);
                    insertCommand.Parameters.AddWithValue("@pinCode", address.PinCode);
                    insertCommand.ExecuteNonQuery();
                }
                sqlConnection.Close();
            }
        }

        public void Delete(int serviceProviderId, int? siteId)
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                sqlConnection.Open();

                string deleteQuery = @"DELETE FROM Addresses WHERE ";

                if (siteId.HasValue && siteId.Value > 0)
                {
                    deleteQuery += "SiteId = @siteId";
                }
                else
                {
                    deleteQuery += "ServiceProviderId = @serviceProviderId";
                }

                SqlCommand deleteCommand = new SqlCommand(deleteQuery, sqlConnection);

                if (siteId.HasValue && siteId.Value > 0)
                    deleteCommand.Parameters.AddWithValue("@siteId", siteId.Value);
                else
                    deleteCommand.Parameters.AddWithValue("@serviceProviderId", serviceProviderId);

                deleteCommand.ExecuteNonQuery();
                sqlConnection.Close();
            }
        }
    }
}
