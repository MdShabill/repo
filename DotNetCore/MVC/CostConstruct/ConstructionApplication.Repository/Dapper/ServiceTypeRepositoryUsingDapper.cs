using ConstructionApplication.Core.DataModels;
using ConstructionApplication.Repository.Interfaces;
using System.Data.SqlClient;
using System.Data;
using Dapper;

namespace ConstructionApplication.Repository.Dapper
{
    public class ServiceTypeRepositoryUsingDapper : IServiceTypeRepository
    {
        private readonly string _connectionString;

        public ServiceTypeRepositoryUsingDapper(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<ServiceType> GetAll()
        {
            using (IDbConnection connection = new SqlConnection(_connectionString))
            {
                string sqlQuery = "SELECT Id, Name FROM ServiceTypes ";
                return connection.Query<ServiceType>(sqlQuery).ToList();
            }
        }
    }
}
