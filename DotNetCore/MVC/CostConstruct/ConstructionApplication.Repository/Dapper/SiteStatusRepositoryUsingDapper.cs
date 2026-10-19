using ConstructionApplication.Repository.Interfaces;
using Dapper;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Data;
using System.Data.SqlClient;
using ConstructionApplication.Core.DataModels;

namespace ConstructionApplication.Repository.Dapper
{
    public class SiteStatusRepositoryUsingDapper : ISiteStatusRepository
    {
        private readonly string _connectionString;
        public SiteStatusRepositoryUsingDapper(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<SiteStatus> GetAll()
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                string sqlQuery = "SELECT Id, Status FROM SiteStatus";
                
                List<SiteStatus> result = db.Query<SiteStatus>(sqlQuery).AsList();
                return result;
            }
        }
    }
}