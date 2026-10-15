using ConstructionApplication.Repository.Interfaces;
using Dapper;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Data.SqlClient;
using ConstructionApplication.Core.DataModels;

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
            try
            {
                var result = conn.Query<SiteScopeMaster>("SELECT Id, ScopeName FROM SiteScopeMaster ORDER BY Id").ToList();
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}