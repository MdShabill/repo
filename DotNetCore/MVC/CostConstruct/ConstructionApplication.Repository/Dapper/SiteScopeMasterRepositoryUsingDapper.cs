using ConstructionApplication.Core.DataModels.SiteScopeMaster;
using ConstructionApplication.Repository.Interfaces;
using Dapper;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Data.SqlClient;

namespace ConstructionApplication.Repository.Dapper
{
    public class SiteScopeMasterRepositoryUsingDapper : ISiteScopeMasterRepository
    {
        private readonly string _connectionString;
        private readonly ILogger<SiteScopeMasterRepositoryUsingDapper> _logger;

        public SiteScopeMasterRepositoryUsingDapper(string connectionString,
               ILogger<SiteScopeMasterRepositoryUsingDapper> logger)
        {
            _connectionString = connectionString;
            _logger = logger;
        }

        public List<SiteScopeMaster> GetAll()
        {
            var methodStopwatch = Stopwatch.StartNew();

            using var conn = new SqlConnection(_connectionString);

            var databaseStopwatch = Stopwatch.StartNew();

            try
            {
                var result = conn.Query<SiteScopeMaster>("SELECT Id, ScopeName FROM SiteScopeMaster ORDER BY Id"
                                                         ).ToList();
                databaseStopwatch.Stop();

                _logger.LogInformation("Database: SELECT All Site Scope Master | Time: {ElapsedMs} ms",
                    databaseStopwatch.ElapsedMilliseconds);

                methodStopwatch.Stop();

                _logger.LogInformation("Method: ISiteScopeMasterRepository.GetAll | Time: {ElapsedMs} ms",
                    methodStopwatch.ElapsedMilliseconds);

                return result;
            }
            catch (Exception ex)
            {
                databaseStopwatch.Stop();

                _logger.LogError(ex, "Database: SELECT All Site Scope Master | Failed | Time: {ElapsedMs} ms",
                    databaseStopwatch.ElapsedMilliseconds);

                methodStopwatch.Stop();

                _logger.LogInformation("Method: ISiteScopeMasterRepository.GetAll | Time: {ElapsedMs} ms",
                    methodStopwatch.ElapsedMilliseconds);
                throw;
            }
        }
    }
}