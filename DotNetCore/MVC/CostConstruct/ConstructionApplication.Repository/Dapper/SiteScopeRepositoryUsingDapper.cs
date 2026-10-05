using ConstructionApplication.Core.DataModels.SiteScope;
using ConstructionApplication.Core.DataModels.SiteScopeStatus;
using ConstructionApplication.Repository.Interfaces;
using Dapper;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Data.SqlClient;

namespace ConstructionApplication.Repository.Dapper
{
    public class SiteScopeRepositoryUsingDapper : ISiteScopeRepository
    {
        private readonly string _connectionString;
        private readonly ILogger<SiteScopeRepositoryUsingDapper> _logger;

        public SiteScopeRepositoryUsingDapper(string connectionString,
               ILogger<SiteScopeRepositoryUsingDapper> logger)
        {
            _connectionString = connectionString;
            _logger = logger;
        }

        public List<SiteScope> GetBySiteId(int siteId)
        {
            var methodStopwatch = Stopwatch.StartNew();

            using var conn = new SqlConnection(_connectionString);

            var databaseStopwatch = Stopwatch.StartNew();

            try
            {
                var result = conn.Query<SiteScope>(@"
                    SELECT
                        SiteScope.Id,
                        SiteScope.SiteId,
                        SiteScope.SiteScopeId,
                        SiteScopeMaster.ScopeName,
                        SiteScope.ScopeStatusId,
                        ScopeStatus.StatusName,
                        SiteScope.CompletedDate,
                        SiteScope.Remarks
                    FROM 
                        SiteScope
                    JOIN 
                        SiteScopeMaster ON SiteScope.SiteScopeId = SiteScopeMaster.Id
                    JOIN 
                        ScopeStatus ON SiteScope.ScopeStatusId = ScopeStatus.Id
                    WHERE   
                        SiteScope.SiteId = @SiteId
                    ORDER 
                        BY SiteScopeMaster.Id
                ", new { SiteId = siteId }).ToList();

                databaseStopwatch.Stop();

                _logger.LogInformation("Database: SELECT Site Scope By Site ID | Time: {ElapsedMs} ms",
                    databaseStopwatch.ElapsedMilliseconds);

                methodStopwatch.Stop();

                _logger.LogInformation("Method: ISiteScopeRepository.GetBySiteId | Time: {ElapsedMs} ms",
                    methodStopwatch.ElapsedMilliseconds);

                return result;
            }
            catch (Exception ex)
            {
                databaseStopwatch.Stop();

                _logger.LogError(ex, "Database: SELECT Site Scope By Site ID | Failed | Time: {ElapsedMs} ms",
                                 databaseStopwatch.ElapsedMilliseconds);

                methodStopwatch.Stop();

                _logger.LogInformation("Method: ISiteScopeRepository.GetBySiteId | Time: {ElapsedMs} ms",
                    methodStopwatch.ElapsedMilliseconds);

                throw;
            }
        }

        public List<SiteScopeStatus> GetAllStatuses()
        {
            var methodStopwatch = Stopwatch.StartNew();

            using var conn = new SqlConnection(_connectionString);

            var databaseStopwatch = Stopwatch.StartNew();

            try
            {
                var result = conn.Query<SiteScopeStatus>("SELECT Id, StatusName FROM ScopeStatus ORDER BY Id"
                                                        ).ToList();
                databaseStopwatch.Stop();

                _logger.LogInformation("Database: SELECT All Scope Statuses | Time: {ElapsedMs} ms",
                    databaseStopwatch.ElapsedMilliseconds);

                methodStopwatch.Stop();

                _logger.LogInformation("Method: ISiteScopeRepository.GetAllStatuses | Time: {ElapsedMs} ms",
                    methodStopwatch.ElapsedMilliseconds);

                return result;
            }
            catch (Exception ex)
            {
                databaseStopwatch.Stop();

                _logger.LogError(ex,"Database: SELECT All Scope Statuses | Failed | Time: {ElapsedMs} ms",
                    databaseStopwatch.ElapsedMilliseconds);

                methodStopwatch.Stop();

                _logger.LogInformation("Method: ISiteScopeRepository.GetAllStatuses | Time: {ElapsedMs} ms",
                    methodStopwatch.ElapsedMilliseconds);

                throw;
            }
        }

        public void SaveScopes(
            int siteId,
            List<(int scopeMasterId, int statusId, string? remarks)> scopes)
        {
            var methodStopwatch = Stopwatch.StartNew();

            using var conn = new SqlConnection(_connectionString);

            try
            {
                conn.Open();

                using var tx = conn.BeginTransaction();

                // Old scopes delete
                var deleteStopwatch = Stopwatch.StartNew();

                try
                {
                    conn.Execute("DELETE FROM SiteScope WHERE SiteId = @SiteId", new { SiteId = siteId }, tx);

                    deleteStopwatch.Stop();

                    _logger.LogInformation("Database: DELETE Existing Site Scopes | Time: {ElapsedMs} ms",
                        deleteStopwatch.ElapsedMilliseconds);
                }
                catch (Exception ex)
                {
                    deleteStopwatch.Stop();

                    _logger.LogError(ex, "Database: DELETE Existing Site Scopes | Failed | Time: {ElapsedMs} ms",
                                     deleteStopwatch.ElapsedMilliseconds);

                    throw;
                }

                // New scopes insert
                foreach (var (scopeMasterId, statusId, remarks) in scopes)
                {
                    var insertStopwatch = Stopwatch.StartNew();

                    try
                    {
                        conn.Execute(@"
                            INSERT INTO SiteScope
                                (SiteId, SiteScopeId, ScopeStatusId, Remarks)
                            VALUES
                                (@SiteId, @SiteScopeId, @ScopeStatusId, @Remarks)
                        ",
                        new
                        {
                            SiteId = siteId,
                            SiteScopeId = scopeMasterId,
                            ScopeStatusId = statusId,
                            Remarks = remarks
                        },
                        tx);

                        insertStopwatch.Stop();

                        _logger.LogInformation("Database: INSERT Site Scope | Time: {ElapsedMs} ms",
                            insertStopwatch.ElapsedMilliseconds);
                    }
                    catch (Exception ex)
                    {
                        insertStopwatch.Stop();

                        _logger.LogError(ex, "Database: INSERT Site Scope | Failed | Time: {ElapsedMs} ms",
                                         insertStopwatch.ElapsedMilliseconds);

                        throw;
                    }
                }
                tx.Commit();

                methodStopwatch.Stop();

                _logger.LogInformation("Method: ISiteScopeRepository.SaveScopes | Time: {ElapsedMs} ms",
                    methodStopwatch.ElapsedMilliseconds);
            }
            catch
            {
                methodStopwatch.Stop();

                _logger.LogInformation("Method: ISiteScopeRepository.SaveScopes | Time: {ElapsedMs} ms",
                    methodStopwatch.ElapsedMilliseconds);

                throw;
            }
        }

        public void UpdateScopeStatus(int id, int scopeStatusId, string? remarks, DateTime? completedDate)
        {
            var methodStopwatch = Stopwatch.StartNew();

            using var conn = new SqlConnection(_connectionString);

            var databaseStopwatch = Stopwatch.StartNew();

            try
            {
                conn.Execute(@"
                    UPDATE 
                        SiteScope
                    SET ScopeStatusId = @ScopeStatusId,
                        Remarks = @Remarks,
                        CompletedDate = @CompletedDate
                    WHERE 
                        Id = @Id
                ",
                new
                {
                    Id = id,
                    ScopeStatusId = scopeStatusId,
                    Remarks = remarks,
                    CompletedDate = completedDate
                });

                databaseStopwatch.Stop();

                _logger.LogInformation("Database: UPDATE Site Scope Status | Time: {ElapsedMs} ms",
                    databaseStopwatch.ElapsedMilliseconds);

                methodStopwatch.Stop();

                _logger.LogInformation("Method: ISiteScopeRepository.UpdateScopeStatus | Time: {ElapsedMs} ms",
                    methodStopwatch.ElapsedMilliseconds);
            }
            catch (Exception ex)
            {
                databaseStopwatch.Stop();

                _logger.LogError(ex, "Database: UPDATE Site Scope Status | Failed | Time: {ElapsedMs} ms",
                    databaseStopwatch.ElapsedMilliseconds);

                methodStopwatch.Stop();

                _logger.LogInformation("Method: ISiteScopeRepository.UpdateScopeStatus | Time: {ElapsedMs} ms",
                    methodStopwatch.ElapsedMilliseconds);
                throw;
            }
        }
    }
}