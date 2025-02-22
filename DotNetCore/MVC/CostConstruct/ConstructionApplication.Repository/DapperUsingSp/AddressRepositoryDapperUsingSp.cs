using ConstructionApplication.Core.DataModels.Address;
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

                // Check if an address record exists for the given ContractorId
                int addressCount = connection.ExecuteScalar<int>(
                    "SELECT COUNT(*) FROM Addresses WHERE ContractorId = @ContractorId",
                    new { address.ContractorId });

                string mode = addressCount > 0 ? "UPDATE" : "INSERT";

                // Execute stored procedure using Dapper
                connection.Execute("Sp_AddressCRUD",
                new
                {
                    Mode = mode,
                    address.ContractorId,
                    AddressLine1 = address.AddressLine1 ?? (object)DBNull.Value,
                    address.AddressTypeId,
                    address.CountryId,
                    address.PinCode
                },
                commandType: CommandType.StoredProcedure);
            }
        }

        public void Delete(int contractorId)
        {
            using (IDbConnection connection = new SqlConnection(_connectionString))
            {
                connection.Execute("Sp_AddressCRUD", new { ContractorId = contractorId, Mode = "DELETE" },
                commandType: CommandType.StoredProcedure);
            }
        }
    }
}
