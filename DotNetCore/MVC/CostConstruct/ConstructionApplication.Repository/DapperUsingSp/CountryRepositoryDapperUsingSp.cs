using ConstructionApplication.Core.DataModels.Country;
using ConstructionApplication.Repository.Interfaces;
using System.Data.SqlClient;
using System.Data;
using Dapper;

namespace ConstructionApplication.Repository.DapperUsingSp
{
    public class CountryRepositoryDapperUsingSp : ICountryRepository
    {
        private readonly string _connectionString;

        public CountryRepositoryDapperUsingSp(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<Country> GetAllCountries()
        {
            using (IDbConnection connection = new SqlConnection(_connectionString))
            {
                return connection.Query<Country>(
                    "Sp_GetAllCountries",
                    commandType: CommandType.StoredProcedure
                ).ToList();
            }
        }
    }
}
