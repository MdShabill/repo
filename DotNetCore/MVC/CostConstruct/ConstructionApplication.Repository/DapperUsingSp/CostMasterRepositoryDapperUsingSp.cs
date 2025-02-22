using ConstructionApplication.Core.DataModels.CostMaster;
using ConstructionApplication.Repository.Interfaces;
using System.Data.SqlClient;
using System.Data;
using Dapper;

namespace ConstructionApplication.Repository.DapperUsingSp
{
    public class CostMasterRepositoryDapperUsingSp : ICostMasterRepository
    {
        private readonly string _connectionString;

        public CostMasterRepositoryDapperUsingSp(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<CostMaster> GetByJobCategory(int jobCategoryId)
        {
            using (IDbConnection connection = new SqlConnection(_connectionString))
            {
                var parameters = new { Mode = "GET_BY_JOBCATEGORY", JobCategoryId = jobCategoryId };
                return connection.Query<CostMaster>("Sp_CostMasterCRUD", parameters, commandType: CommandType.StoredProcedure).ToList();
            }
        }

        public CostMaster GetActiveCostDetail(int jobCategoryId)
        {
            using (IDbConnection connection = new SqlConnection(_connectionString))
            {
                var parameters = new
                {
                    Mode = "GET_ACTIVE_COST",
                    JobCategoryId = jobCategoryId,
                    CurrentDate = DateTime.Now
                };
                return connection.QueryFirstOrDefault<CostMaster>("Sp_CostMasterCRUD", parameters, commandType: CommandType.StoredProcedure) ?? new CostMaster();
            }
        }

        public int Create(CostMaster costMaster)
        {
            using (IDbConnection connection = new SqlConnection(_connectionString))
            {
                var parameters = new
                {
                    Mode = "CREATE",
                    costMaster.JobCategoryId,
                    costMaster.Cost,
                    costMaster.Date
                };
                return connection.ExecuteScalar<int>("Sp_CostMasterCRUD", parameters, commandType: CommandType.StoredProcedure);
            }
        }
    }
}
