using ShopEase.DataModels;
using System.Data.SqlClient;
using System.Data;

namespace ShopEase.Repositories
{
    public class AddressTypeRepository : IAddressTypeRepository
    {
        private readonly string _connectionString;

        public AddressTypeRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<AddressType> GetAllAddresses()
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                string sqlQuery = "Select Id, AddressTypeName From AddressTypes";
                SqlDataAdapter sqlDataAdapter = new(sqlQuery, sqlConnection);
                DataTable dataTable = new();
                sqlDataAdapter.Fill(dataTable);

                List<AddressType> addressTypes = new();

                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    AddressType addressType = new()
                    {
                        Id = (int)dataTable.Rows[i]["Id"],
                        AddressTypeName = (string)dataTable.Rows[i]["AddressTypeName"]
                    };
                    addressTypes.Add(addressType);
                }
                return addressTypes;
            }
        }
    }
}
