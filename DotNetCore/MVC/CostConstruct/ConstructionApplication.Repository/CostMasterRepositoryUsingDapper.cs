using ConstructionApplication.Core.DataModels.CostMaster;
using ConstructionApplication.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using ConstructionApplication.Core.DataModels.JobCategory;

namespace ConstructionApplication.Repository
{
    public class CostMasterRepositoryUsingDapper : ICostMasterRepository
    {
        private readonly string _connectionString;

        public CostMasterRepositoryUsingDapper(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<CostMaster> GetByJobCategory(int jobCategoryId)
        {
            using (IDbConnection connection = new SqlConnection(_connectionString))
            {
                string sqlQuery = @"
                        SELECT 
                            CostMaster.Id, CostMaster.JobCategoryId, 
                            JobCategories.Name, CostMaster.Cost, CostMaster.Date
                        FROM 
                            CostMaster
                        JOIN 
                            JobCategories ON CostMaster.JobCategoryId = JobCategories.Id
                        WHERE 
                            CostMaster.JobCategoryId = @jobCategoryId
                        ORDER BY CostMaster.Date DESC";

                return connection.Query<CostMaster>(sqlQuery, new { jobCategoryId }).ToList();
            }
        }

        public CostMaster GetActiveCostDetail(int jobCategoryId)
        {
            using (IDbConnection connection = new SqlConnection(_connectionString))
            {
                string sqlQuery = @"
                        SELECT 
                            TOP 1 CostMaster.JobCategoryId, JobCategories.Name, 
                                  CostMaster.Cost, CostMaster.Date
                        FROM 
                            CostMaster 
                        JOIN 
                            JobCategories ON CostMaster.JobCategoryId = JobCategories.Id 
                        WHERE 
                            CostMaster.JobCategoryId = @jobCategoryId 
                            AND CostMaster.Date <= @currentDate 
                        ORDER BY CostMaster.Date DESC";

                return connection.QueryFirstOrDefault<CostMaster>(sqlQuery,
                    new { jobCategoryId, currentDate = DateTime.Now }) ?? new CostMaster();
            }
        }

        public int Create(CostMaster costMaster)
        {
            using (IDbConnection connection = new SqlConnection(_connectionString))
            {
                string insertQuery = @"
                       INSERT INTO CostMaster 
                             (JobCategoryId, Cost, Date)
                       VALUES 
                             (@JobCategoryId, @Cost, @Date);
                       SELECT CAST(SCOPE_IDENTITY() AS INT);";

                return connection.ExecuteScalar<int>(insertQuery, costMaster);
            }
        }
    }
}
