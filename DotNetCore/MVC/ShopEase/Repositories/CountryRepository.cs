using System.Data.SqlClient;
using System.Data;
using ShopEase.DataModels.Address;

namespace ShopEase.Repositories
{
    public class CountryRepository : ICountryRepository
    {
        private readonly string _connectionString;

        public CountryRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<Country> GetAllCountries()
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                string sqlQuery = "Select Id, CountryName From Countries";
                SqlDataAdapter sqlDataAdapter = new(sqlQuery, sqlConnection);
                DataTable dataTable = new();
                sqlDataAdapter.Fill(dataTable);

                List<Country> countries = new();

                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    Country country = new()
                    {
                        Id = (int)dataTable.Rows[i]["Id"],
                        CountryName = (string)dataTable.Rows[i]["CountryName"]
                    };
                    countries.Add(country);
                }
                return countries;
            }
        }
    }
}
