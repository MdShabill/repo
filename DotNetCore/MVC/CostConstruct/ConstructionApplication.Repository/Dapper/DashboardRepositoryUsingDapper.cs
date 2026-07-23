// Path: ConstructionApplication.Repository/Dapper/DashboardRepositoryUsingDapper.cs
using ConstructionApplication.Core.DataModels.Dashboard;
using ConstructionApplication.Repository.Interfaces;
using Dapper;
using System.Data.SqlClient;

namespace ConstructionApplication.Repository.Dapper
{
    public class DashboardRepositoryUsingDapper : IDashboardRepository
    {
        private readonly string _connectionString;

        public DashboardRepositoryUsingDapper(string connectionString)
        {
            _connectionString = connectionString;
        }

        public DashboardStats GetStats(int siteId)
        {
            using var connection = new SqlConnection(_connectionString);

            string sql = @"
                SELECT

                    -- Total material purchase cost from ALL TIME
                    (
                        SELECT ISNULL(
                            SUM(MaterialCost + ISNULL(DeliveryCharge, 0)),
                            0
                        )
                        FROM MaterialPurchase
                        WHERE (@SiteId = 0 OR SiteId = @SiteId)
                    ) AS TotalMaterialSpend,

                    -- Total material purchase entries from ALL TIME
                    (
                        SELECT COUNT(*)
                        FROM MaterialPurchase
                        WHERE (@SiteId = 0 OR SiteId = @SiteId)
                    ) AS TotalPurchases,

                    -- Total workers from ALL ATTENDANCE RECORDS
                    (
                        SELECT ISNULL(SUM(TotalWorker), 0)
                        FROM DailyAttendance
                        WHERE (@SiteId = 0 OR SiteId = @SiteId)
                    ) AS TotalAttendanceWorkers;";

            return connection.QuerySingleOrDefault<DashboardStats>(
                sql,
                new { SiteId = siteId }
            ) ?? new DashboardStats();
        }
    }
}