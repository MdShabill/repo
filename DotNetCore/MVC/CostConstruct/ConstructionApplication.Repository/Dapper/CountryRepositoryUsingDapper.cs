using ConstructionApplication.Repository.Interfaces;
using Dapper;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Data;
using System.Data.SqlClient;
using ConstructionApplication.Core.DataModels;

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
            var methodStopwatch = Stopwatch.StartNew();

            using (IDbConnection connection = new SqlConnection(_connectionString))
            {
                string sqlQuery = "Select Id, Name From Countries";
                
                List<Country> result = connection.Query<Country>(sqlQuery).ToList();
                return result;
            }
        }
    }
}