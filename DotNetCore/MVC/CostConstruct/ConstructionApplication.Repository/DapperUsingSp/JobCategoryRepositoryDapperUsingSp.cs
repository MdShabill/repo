using ConstructionApplication.Core.DataModels.JobCategory;
using ConstructionApplication.Repository.Interfaces;
using System.Data.SqlClient;
using System.Data;
using Dapper;

namespace ConstructionApplication.Repository.DapperUsingSp
{
    public class JobCategoryRepositoryDapperUsingSp : IJobCategoryRepository
    {
        private readonly string _connectionString;

        public JobCategoryRepositoryDapperUsingSp(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<JobCategory> GetAll()
        {
            using (IDbConnection connection = new SqlConnection(_connectionString))
            {
                return connection.Query<JobCategory>("Sp_GetAllJobCategories", commandType: CommandType.StoredProcedure).ToList();
            }
        }
    }
}
