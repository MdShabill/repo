using ConstructionApplication.Core.DataModels.ServiceTypes;
using ConstructionApplication.Repository.Interfaces;
using System.Data.SqlClient;
using System.Data;
using Dapper;

namespace ConstructionApplication.Repository.DapperUsingSp
{
    public class ServiceTypeRepositoryDapperUsingSp : IServiceTypeRepository
    {
        private readonly string _connectionString;

        public ServiceTypeRepositoryDapperUsingSp(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<ServiceType> GetAll()
        {
            using (IDbConnection connection = new SqlConnection(_connectionString))
            {
                return connection.Query<ServiceType>("Sp_GetAllServiceTypes", commandType: CommandType.StoredProcedure).ToList();
            }
        }
    }
}
