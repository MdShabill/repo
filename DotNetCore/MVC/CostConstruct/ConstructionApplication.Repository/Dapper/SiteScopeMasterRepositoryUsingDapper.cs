using ConstructionApplication.Core.DataModels.SiteScopeMaster;
using ConstructionApplication.Repository.Interfaces;
using Dapper;
using System.Data.SqlClient;

namespace ConstructionApplication.Repository.Dapper
{
    public class SiteScopeMasterRepositoryUsingDapper : ISiteScopeMasterRepository
    {
        private readonly string _connectionString;

        public SiteScopeMasterRepositoryUsingDapper(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<SiteScopeMaster> GetAll()
        {
            using var conn = new SqlConnection(_connectionString);
            return conn.Query<SiteScopeMaster>(
                "SELECT Id, ScopeName FROM SiteScopeMaster ORDER BY Id"
            ).ToList();
        }
    }
}