using ConstructionApplication.Core.DataModels.Address;
using ConstructionApplication.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConstructionApplication.Repository
{
    public class AddressRepositoryUsingSp : IAddressRepository
    {
        private readonly string _connectionString;

        public AddressRepositoryUsingSp(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void InsertOrUpdateAddress(Address address)
        {
            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                sqlConnection.Open();

                // Check if an address record exists for the given ContractorId
                string checkQuery = "SELECT COUNT(*) FROM Addresses WHERE ContractorId = @contractorId";
                SqlCommand checkCommand = new SqlCommand(checkQuery, sqlConnection);
                checkCommand.Parameters.AddWithValue("@contractorId", address.ContractorId);

                int addressCount = (int)checkCommand.ExecuteScalar();
                string mode = addressCount > 0 ? "UPDATE" : "INSERT";

                SqlCommand sqlCommand = new SqlCommand("Sp_AddressCRUD", sqlConnection)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };

                sqlCommand.Parameters.AddWithValue("@Mode", mode);
                sqlCommand.Parameters.AddWithValue("@ContractorId", address.ContractorId);
                sqlCommand.Parameters.AddWithValue("@AddressLine1", address.AddressLine1 ?? (object)DBNull.Value);
                sqlCommand.Parameters.AddWithValue("@AddressTypeId", address.AddressTypeId);
                sqlCommand.Parameters.AddWithValue("@CountryId", address.CountryId);
                sqlCommand.Parameters.AddWithValue("@PinCode", address.PinCode);

                sqlCommand.ExecuteNonQuery();
                sqlConnection.Close();
            }
        }

        public void Delete(int contractorId)
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                sqlConnection.Open();

                SqlCommand deleteAddressesCommand = new SqlCommand("Sp_AddressCRUD", sqlConnection)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };

                deleteAddressesCommand.Parameters.AddWithValue("@Mode", "DELETE");
                deleteAddressesCommand.Parameters.AddWithValue("@contractorId", contractorId);
                deleteAddressesCommand.ExecuteNonQuery();
                sqlConnection.Close();
            }
        }
    }
}
