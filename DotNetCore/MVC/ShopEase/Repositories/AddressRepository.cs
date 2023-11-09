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
                string sqlQuery = @"SELECT Addresses.Id, ' - ', CONCAT(Addresses.Id, Addresses.AddressLine1, ' - ', 
                            Addresses.AddressLine2, ' - ', Addresses.PinCode, 
                            ' - ', Countries.CountryName, ' - ', AddressTypes.AddressTypeName) As AddressDetail
                            FROM Addresses
                            INNER JOIN Countries ON Addresses.CountryId = Countries.Id
                            INNER JOIN AddressTypes ON Addresses.AddressTypeID = AddressTypes.Id ";
                SqlDataAdapter sqlDataAdapter = new(sqlQuery, sqlConnection);
                DataTable dataTable = new();
                sqlDataAdapter.Fill(dataTable);

                List<Address> addresses = new();

                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    Address address = new()
                    {
                        Id = (int)dataTable.Rows[i]["Id"],
                        AddressDetail = dataTable.Rows[i]["AddressDetail"].ToString()
                        //AddressLine1 = (string)dataTable.Rows[i]["AddressLine1"],
                        //AddressLine2 = (string)dataTable.Rows[i]["AddressLine2"],
                        //PinCode = (int)dataTable.Rows[i]["PinCode"],
                        //CountryName = (string)dataTable.Rows[i]["CountryName"],
                        //AddressTypeName = (string)dataTable.Rows[i]["AddressTypeName"],
                    };
                    addresses.Add(address);
                }
                return addresses;
            }
        }
    }
}
