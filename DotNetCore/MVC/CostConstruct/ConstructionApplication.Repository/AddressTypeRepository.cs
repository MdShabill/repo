using ConstructionApplication.Core.DataModels.AddressType;
using ConstructionApplication.Core.DataModels.Country;
using System.Data.SqlClient;
using System.Data;
using ConstructionApplication.Repository.Interfaces;

namespace ConstructionApplication.Repositories
{
    public class AddressTypeRepository : IAddressTypeRepository
    {
        private readonly string _connectionString;

        public AddressTypeRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<AddressType> GetAll()
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                string sqlQuery = "Select Id, Name From AddressTypes";
                SqlDataAdapter sqlDataAdapter = new(sqlQuery, sqlConnection);
                DataTable dataTable = new();
                sqlDataAdapter.Fill(dataTable);

                List<AddressType> addressTypes = new();

                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    AddressType addressType = new()
                    {
                        Id = (int)dataTable.Rows[i]["Id"],
                        Name = (string)dataTable.Rows[i]["Name"]
                    };
                    addressTypes.Add(addressType);
                }
                return addressTypes;
            }
        }
    }
}
