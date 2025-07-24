using ConstructionApplication.Core.DataModels.SiteStatus;
using ConstructionApplication.Repository.Interfaces;
using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

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
                return db.Query<SiteStatus>(sqlQuery).AsList();
            }
        }
    }
}
