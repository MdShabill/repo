using ConstructionApplication.Core.DataModels.Address;
using ConstructionApplication.Repository.Interfaces;
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
                            (AddressLine1, ContractorId, AddressTypeId, CountryId, PinCode)
                            VALUES 
                            (@addressLine1, @contractorId, @addressTypeId, @countryId, @pinCode)";

                    SqlCommand sqlCommand = new(sqlQuery, sqlConnection);
                    sqlCommand.Parameters.AddWithValue("@addressLine1", address.AddressLine1 ?? (object)DBNull.Value);
                    sqlCommand.Parameters.AddWithValue("@contractorId", address.ContractorId);
                    sqlCommand.Parameters.AddWithValue("@addressTypeId", address.AddressTypeId == 0 ? (object)DBNull.Value : address.AddressTypeId);
                    sqlCommand.Parameters.AddWithValue("@countryId", address.CountryId == 0 ? (object)DBNull.Value : address.CountryId);
                    sqlCommand.Parameters.AddWithValue("@pinCode", address.PinCode == 0 ? (object)DBNull.Value : address.PinCode);
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

        public void InsertOrUpdateAddress(Address address)
        {
            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                // Check if an address record exists for the given ContractorId
                string checkQuery = "SELECT COUNT(*) FROM Addresses WHERE ContractorId = @contractorId";
                SqlCommand checkCommand = new SqlCommand(checkQuery, sqlConnection);
                checkCommand.Parameters.AddWithValue("@contractorId", address.ContractorId);

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
                                WHERE ContractorId = @contractorId";

                    SqlCommand updateCommand = new SqlCommand(updateQuery, sqlConnection);
                    updateCommand.Parameters.AddWithValue("@addressLine1", address.AddressLine1 ?? (object)DBNull.Value);
                    updateCommand.Parameters.AddWithValue("@addressTypeId", address.AddressTypeId);
                    updateCommand.Parameters.AddWithValue("@countryId", address.CountryId);
                    updateCommand.Parameters.AddWithValue("@pinCode", address.PinCode);
                    updateCommand.Parameters.AddWithValue("@contractorId", address.ContractorId);
                    updateCommand.ExecuteNonQuery();
                }
                else
                {
                    string insertQuery = @"
                               INSERT INTO Addresses 
                                      (ContractorId, AddressLine1, AddressTypeId, CountryId, PinCode)
                                VALUES 
                                      (@contractorId, @addressLine1, @addressTypeId, @countryId, @pinCode)";

                    SqlCommand insertCommand = new SqlCommand(insertQuery, sqlConnection);
                    insertCommand.Parameters.AddWithValue("@contractorId", address.ContractorId);
                    insertCommand.Parameters.AddWithValue("@addressLine1", address.AddressLine1 ?? (object)DBNull.Value);
                    insertCommand.Parameters.AddWithValue("@addressTypeId", address.AddressTypeId);
                    insertCommand.Parameters.AddWithValue("@countryId", address.CountryId);
                    insertCommand.Parameters.AddWithValue("@pinCode", address.PinCode);
                    insertCommand.ExecuteNonQuery();
                }

                sqlConnection.Close();
            }
        }
    }
}
