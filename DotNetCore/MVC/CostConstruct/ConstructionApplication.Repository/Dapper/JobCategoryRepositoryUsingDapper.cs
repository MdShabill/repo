using ConstructionApplication.Core.DataModels.JobCategory;
using ConstructionApplication.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConstructionApplication.Core.DataModels.Suppliers;
using Dapper;

namespace ConstructionApplication.Repository.Dapper
{
    public class JobCategoryRepositoryUsingDapper : IJobCategoryRepository
    {
        private readonly string _connectionString;

        public JobCategoryRepositoryUsingDapper(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<JobCategory> GetAll()
        {
            using (IDbConnection connection = new SqlConnection(_connectionString))
            {
                string sqlQuery = "SELECT Id, Name FROM JobCategories ";
                return connection.Query<JobCategory>(sqlQuery).ToList();
            }
        }
    }
}
