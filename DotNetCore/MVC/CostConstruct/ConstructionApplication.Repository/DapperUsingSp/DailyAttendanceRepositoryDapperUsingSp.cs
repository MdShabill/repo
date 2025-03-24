using ConstructionApplication.Core.DataModels.DailyAttendance;
using ConstructionApplication.Repository.Interfaces;
using System.Data.SqlClient;
using System.Data;
using Dapper;

namespace ConstructionApplication.Repository.DapperUsingSp
{
    public class DailyAttendanceRepositoryDapperUsingSp : IDailyAttendanceRepository
    {
        private readonly string _connectionString;

        public DailyAttendanceRepositoryDapperUsingSp(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<DailyAttendance> GetAll(DateTime? DateFrom, DateTime? DateTo)
        {
            using (IDbConnection connection = new SqlConnection(_connectionString))
            {
                var parameters = new
                {
                    Mode = "GET_ALL", DateFrom, DateTo
                };
                return connection.Query<DailyAttendance>("Sp_DailyAttendanceCRUD", parameters, commandType: CommandType.StoredProcedure).ToList();
            }
        }

        public int Create(DailyAttendance dailyAttendance)
        {
            using (IDbConnection connection = new SqlConnection(_connectionString))
            {
                var parameters = new
                {
                    Mode = "CREATE",
                    dailyAttendance.Date,
                    dailyAttendance.JobCategoryId,
                    dailyAttendance.ContractorId,
                    dailyAttendance.TotalWorker,
                    dailyAttendance.AmountPerWorker,
                    dailyAttendance.TotalAmount,
                    dailyAttendance.Notes
                };
                return connection.ExecuteScalar<int>("Sp_DailyAttendanceCRUD", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public void Delete(int contractorId)
        {
            using (IDbConnection connection = new SqlConnection(_connectionString))
            {
                var parameters = new
                {
                    Mode = "DELETE", ContractorId = contractorId
                };
                connection.Execute("Sp_DailyAttendanceCRUD", parameters, commandType: CommandType.StoredProcedure);
            }
        }
    }
}
