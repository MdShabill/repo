using ConstructionApplication.Core.DataModels.Address;
using ConstructionApplication.Core.DataModels.ServiceProviders;
using ConstructionApplication.Repository.Interfaces;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConstructionApplication.Repository.DapperUsingSp
{
    public class AddressRepositoryDapperUsingSp : IAddressRepository
    {
        private readonly string _connectionString;

        public AddressRepositoryDapperUsingSp(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void InsertOrUpdateAddress(Address address)
        {
            using (IDbConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                // Check if an address record exists for the given ServiceProviderId
                int addressCount = connection.ExecuteScalar<int>(
                    "SELECT COUNT(*) FROM Addresses WHERE ServiceProviderId = @ServiceProviderId",
                    new { address.ServiceProviderId });

                string mode = addressCount > 0 ? "UPDATE" : "INSERT";

                // Execute stored procedure using Dapper
                connection.Execute("Sp_AddressCRUD",
                new
                {
                    Mode = mode,
                    address.ServiceProviderId,
                    AddressLine1 = address.AddressLine1 ?? (object)DBNull.Value,
                    address.AddressTypeId,
                    address.CountryId,
                    address.PinCode
                },
                commandType: CommandType.StoredProcedure);
            }
        }

        public void Delete(int serviceProviderId)
        {
            using (IDbConnection connection = new SqlConnection(_connectionString))
            {
                connection.Execute("Sp_AddressCRUD", new { ServiceProviderId = serviceProviderId, Mode = "DELETE" },
                commandType: CommandType.StoredProcedure);
            }
        }
    }
}
