using ConstructionApplication.Core.DataModels.Country;
using ConstructionApplication.Repository.Interfaces;
using System.Data.SqlClient;
using System.Data;
using Dapper;

namespace ConstructionApplication.Repository.Dapper
{
    public class CountryRepositoryUsingDapper : ICountryRepository
    {
        private readonly string _connectionString;

        public CountryRepositoryUsingDapper(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<Country> GetAllCountries()
        {
            using (IDbConnection connection = new SqlConnection(_connectionString))
            {
                string sqlQuery = "Select Id, Name From Countries";
                return connection.Query<Country>(sqlQuery).ToList();
            }
        }
    }
}
