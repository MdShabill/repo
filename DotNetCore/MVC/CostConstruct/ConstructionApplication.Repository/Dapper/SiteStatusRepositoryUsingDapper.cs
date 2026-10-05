using ConstructionApplication.Core.DataModels.SiteStatus;
using ConstructionApplication.Repository.Interfaces;
using Dapper;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Data;
using System.Data.SqlClient;

namespace ConstructionApplication.Repository.Dapper
{
    public class SiteStatusRepositoryUsingDapper : ISiteStatusRepository
    {
        private readonly string _connectionString;
        private readonly ILogger<SiteStatusRepositoryUsingDapper> _logger;

        public SiteStatusRepositoryUsingDapper(string connectionString,
               ILogger<SiteStatusRepositoryUsingDapper> logger)
        {
            _connectionString = connectionString;
            _logger = logger;
        }

        public List<SiteStatus> GetAll()
        {
            var methodStopwatch = Stopwatch.StartNew();

            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                string sqlQuery = "SELECT Id, Status FROM SiteStatus";

                var databaseStopwatch = Stopwatch.StartNew();

                try
                {
                    var result = db.Query<SiteStatus>(sqlQuery).AsList();

                    databaseStopwatch.Stop();

                    _logger.LogInformation("Database: SELECT All Site Status | Time: {ElapsedMs} ms",
                        databaseStopwatch.ElapsedMilliseconds);

                    methodStopwatch.Stop();

                    _logger.LogInformation("Method: ISiteStatusRepository.GetAll | Time: {ElapsedMs} ms",
                        methodStopwatch.ElapsedMilliseconds);

                    return result;
                }
                catch (Exception ex)
                {
                    databaseStopwatch.Stop();

                    _logger.LogError(ex, "Database: SELECT All Site Status | Failed | Time: {ElapsedMs} ms",
                        databaseStopwatch.ElapsedMilliseconds);

                    methodStopwatch.Stop();

                    _logger.LogInformation("Method: ISiteStatusRepository.GetAll | Time: {ElapsedMs} ms",
                        methodStopwatch.ElapsedMilliseconds);

                    throw;
                }
            }
        }
    }
}