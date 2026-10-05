using ConstructionApplication.Core.DataModels.Country;
using ConstructionApplication.Repository.Interfaces;
using Dapper;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Data;
using System.Data.SqlClient;

namespace ConstructionApplication.Repository.Dapper
{
    public class CountryRepositoryUsingDapper : ICountryRepository
    {
        private readonly string _connectionString;
        private readonly ILogger<CountryRepositoryUsingDapper> _logger;

        public CountryRepositoryUsingDapper(string connectionString,
               ILogger<CountryRepositoryUsingDapper> logger)
        {
            _connectionString = connectionString;
            _logger = logger;
        }

        public List<Country> GetAllCountries()
        {
            var methodStopwatch = Stopwatch.StartNew();

            using (IDbConnection connection = new SqlConnection(_connectionString))
            {
                string sqlQuery = "Select Id, Name From Countries";

                var databaseStopwatch = Stopwatch.StartNew();

                try
                {
                    var result = connection.Query<Country>(sqlQuery).ToList();

                    databaseStopwatch.Stop();

                    _logger.LogInformation("Database: SELECT All Countries | Time: {ElapsedMs} ms",
                        databaseStopwatch.ElapsedMilliseconds);

                    methodStopwatch.Stop();

                    _logger.LogInformation("Method: ICountryRepository.GetAllCountries | Time: {ElapsedMs} ms",
                        methodStopwatch.ElapsedMilliseconds);

                    return result;
                }
                catch (Exception ex)
                {
                    databaseStopwatch.Stop();

                    _logger.LogError(ex, "Database: SELECT All Countries | Failed | Time: {ElapsedMs} ms",
                                     databaseStopwatch.ElapsedMilliseconds);

                    methodStopwatch.Stop();

                    _logger.LogInformation("Method: ICountryRepository.GetAllCountries | Time: {ElapsedMs} ms",
                        methodStopwatch.ElapsedMilliseconds);
                    throw;
                }
            }
        }
    }
}