using ConstructionApplication.Core.DataModels.AddressType;
using ConstructionApplication.Repository.Interfaces;
using Dapper;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Data;
using System.Data.SqlClient;

namespace ConstructionApplication.Repository.Dapper
{
    public class AddressTypeRepositoryUsingDapper : IAddressTypeRepository
    {
        private readonly string _connectionString;
        private readonly ILogger<AddressTypeRepositoryUsingDapper> _logger;

        public AddressTypeRepositoryUsingDapper(string connectionString,
               ILogger<AddressTypeRepositoryUsingDapper> logger)
        {
            _connectionString = connectionString;
            _logger = logger;
        }

        public List<AddressType> GetAll()
        {
            var methodStopwatch = Stopwatch.StartNew();

            using (IDbConnection connection = new SqlConnection(_connectionString))
            {
                string sqlQuery = "Select Id, Name From AddressTypes";

                var databaseStopwatch = Stopwatch.StartNew();

                try
                {
                    var result = connection.Query<AddressType>(sqlQuery).ToList();

                    databaseStopwatch.Stop();

                    _logger.LogInformation("Database: SELECT All Address Types | Time: {ElapsedMs} ms",
                        databaseStopwatch.ElapsedMilliseconds);

                    methodStopwatch.Stop();

                    _logger.LogInformation("Method: IAddressTypeRepository.GetAll | Time: {ElapsedMs} ms",
                        methodStopwatch.ElapsedMilliseconds);

                    return result;
                }
                catch (Exception ex)
                {
                    databaseStopwatch.Stop();

                    _logger.LogError(ex,"Database: SELECT All Address Types | Failed | Time: {ElapsedMs} ms",
                                     databaseStopwatch.ElapsedMilliseconds);

                    methodStopwatch.Stop();

                    _logger.LogInformation("Method: IAddressTypeRepository.GetAll | Time: {ElapsedMs} ms",
                        methodStopwatch.ElapsedMilliseconds);
                    throw;
                }
            }
        }
    }
}