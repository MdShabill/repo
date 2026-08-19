using ConstructionApplication.Core.DataModels.SiteScope;
using ConstructionApplication.Core.DataModels.SiteScopeStatus;
using ConstructionApplication.Repository.Interfaces;
using Dapper;
using System.Data.SqlClient;

namespace ConstructionApplication.Repository.Dapper
{
    public class SiteScopeRepositoryUsingDapper : ISiteScopeRepository
    {
        private readonly string _connectionString;

        public SiteScopeRepositoryUsingDapper(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<SiteScope> GetBySiteId(int siteId)
        {
            using var conn = new SqlConnection(_connectionString);
            return conn.Query<SiteScope>(@"
                SELECT
                    SiteScope.Id,
                    SiteScope.SiteId,
                    SiteScope.SiteScopeId,
                    SiteScopeMaster.ScopeName,
                    SiteScope.ScopeStatusId,
                    ScopeStatus.StatusName,
                    SiteScope.CompletedDate,
                    SiteScope.Remarks
                FROM  SiteScope
                JOIN  SiteScopeMaster ON SiteScope.SiteScopeId   = SiteScopeMaster.Id
                JOIN  ScopeStatus     ON SiteScope.ScopeStatusId = ScopeStatus.Id
                WHERE SiteScope.SiteId = @SiteId
                ORDER BY SiteScopeMaster.Id
            ", new { SiteId = siteId }).ToList();
        }

        public List<SiteScopeStatus> GetAllStatuses()
        {
            using var conn = new SqlConnection(_connectionString);
            return conn.Query<SiteScopeStatus>(
                "SELECT Id, StatusName FROM ScopeStatus ORDER BY Id"
            ).ToList();
        }

        public void SaveScopes(int siteId,
            List<(int scopeMasterId, int statusId, string? remarks)> scopes)
        {
            using var conn = new SqlConnection(_connectionString);
            conn.Open();
            using var tx = conn.BeginTransaction();

            // Old scopes delete
            conn.Execute(
                "DELETE FROM SiteScope WHERE SiteId = @SiteId",
                new { SiteId = siteId }, tx);

            // New scopes insert
            foreach (var (scopeMasterId, statusId, remarks) in scopes)
            {
                conn.Execute(@"
                    INSERT INTO SiteScope (SiteId, SiteScopeId, ScopeStatusId, Remarks)
                    VALUES (@SiteId, @SiteScopeId, @ScopeStatusId, @Remarks)
                ", new
                {
                    SiteId = siteId,
                    SiteScopeId = scopeMasterId,
                    ScopeStatusId = statusId,
                    Remarks = remarks
                }, tx);
            }

            tx.Commit();
        }

        public void UpdateScopeStatus(
            int id, int scopeStatusId,
            string? remarks, DateTime? completedDate)
        {
            using var conn = new SqlConnection(_connectionString);
            conn.Execute(@"
                UPDATE SiteScope
                SET    ScopeStatusId = @ScopeStatusId,
                       Remarks       = @Remarks,
                       CompletedDate = @CompletedDate
                WHERE  Id = @Id
            ", new { Id = id, ScopeStatusId = scopeStatusId, Remarks = remarks, CompletedDate = completedDate });
        }
    }
}