using ShopEase.DataModels;
using System.Data;
using System.Data.SqlClient;

namespace ShopEase.Repositories
{
    public class AddressRepository : IAddressRepository
    {
        private readonly string _connectionString;

        public AddressRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<Address> GetAllAddress()
        {
            using(SqlConnection sqlConnection = new(_connectionString)) 
            {
                string sqlQuery = "Select Id, PinCode From Addresses ";
                SqlDataAdapter sqlDataAdapter = new(sqlQuery, sqlConnection);
                DataTable dataTable = new();
                sqlDataAdapter.Fill(dataTable);

                List<Address> addresses = new();

                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    Address address = new()
                    {
                        Id = (int)dataTable.Rows[i]["Id"],
                        PinCode = (int)dataTable.Rows[i]["PinCode"]
                    };
                    addresses.Add(address);
                }
                return addresses;
            }
        }
    }
}
