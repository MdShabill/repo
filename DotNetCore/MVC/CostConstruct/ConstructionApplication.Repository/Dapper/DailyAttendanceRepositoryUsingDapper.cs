using ConstructionApplication.Core.DataModels.DailyAttendance;
using ConstructionApplication.Repository.Interfaces;
using System.Data.SqlClient;
using System.Data;
using Dapper;

namespace ConstructionApplication.Repository.Dapper
{
    public class DailyAttendanceRepositoryUsingDapper : IDailyAttendanceRepository
    {
        private readonly string _connectionString;

        public DailyAttendanceRepositoryUsingDapper(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<DailyAttendance> GetAll(int siteId, DateTime? DateFrom, DateTime? DateTo)
        {
            using (IDbConnection connection = new SqlConnection(_connectionString))
            {
                string sqlQuery = @"
                       SELECT 
                           DailyAttendance.Date, JobCategories.Name, Contractors.Name AS ContractorName,
                           DailyAttendance.TotalWorker, DailyAttendance.AmountPerWorker, DailyAttendance.TotalAmount
                       FROM 
                           DailyAttendance
                       JOIN 
                           JobCategories ON DailyAttendance.JobCategoryId = JobCategories.Id
                       JOIN 
                           Contractors ON DailyAttendance.ContractorId = Contractors.Id
                       WHERE
                           (@DateFrom IS NULL OR DailyAttendance.Date >= @DateFrom) 
                           AND 
                           (@DateTo IS NULL OR DailyAttendance.Date <= @DateTo)
                       ORDER BY DailyAttendance.Date DESC";

                // Use Dapper's Query method to directly map the results to a list of DailyAttendance
                return connection.Query<DailyAttendance>(sqlQuery, new { DateFrom, DateTo }).ToList();
            }
        }

        public int Create(DailyAttendance dailyAttendance)
        {
            using (IDbConnection connection = new SqlConnection(_connectionString))
            {
                string sqlQuery = @"
                       INSERT INTO DailyAttendance
                          (Date, JobCategoryId, ContractorId, TotalWorker, AmountPerWorker, TotalAmount)
                       VALUES
                          (@Date, @JobCategoryId, @ContractorId, @TotalWorker, @AmountPerWorker, @TotalAmount);
                       SELECT CAST(SCOPE_IDENTITY() AS INT);";

                // Executes the SQL query and returns the newly inserted DailyAttendance Id
                return connection.ExecuteScalar<int>(sqlQuery, dailyAttendance);
            }
        }

        public void Delete(int contractorId)
        {
            using (IDbConnection connection = new SqlConnection(_connectionString))
            {
                string deleteQuery = "DELETE FROM DailyAttendance WHERE ContractorId = @ContractorId";
                connection.Execute(deleteQuery, new { ContractorId = contractorId });
            }
        }
    }
}
