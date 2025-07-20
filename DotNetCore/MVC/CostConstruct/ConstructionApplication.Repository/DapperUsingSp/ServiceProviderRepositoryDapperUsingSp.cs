using ConstructionApplication.Core.DataModels.ServiceProviders;
using ConstructionApplication.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using ConstructionApplication.Core.DataModels.ServiceTypes;

namespace ConstructionApplication.Repository.DapperUsingSp
{
    public class ServiceProviderRepositoryDapperUsingSp : IServiceProviderRepository
    {
        private readonly string _connectionString;

        public ServiceProviderRepositoryDapperUsingSp(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<ServiceProvider> GetAll(int? serviceTypeId, int? id)
        {
            using (IDbConnection connection = new SqlConnection(_connectionString))
            {
                string sqlQuery = "SP_ServiceProviderCRUD";
                var parameters = new
                {
                    Mode = 2,
                    ServiceProviderId = id ?? 0,
                    FilterServiceTypeId = serviceTypeId ?? 0
                };
                // Execute the stored procedure and return mapped list of ServiceProviders
                return connection.Query<ServiceProvider>(sqlQuery, parameters, commandType: CommandType.StoredProcedure).ToList();
            }
        }

        public int Add(ServiceProvider serviceProvider)
        {
            using (IDbConnection connection = new SqlConnection(_connectionString))
            {
                string sqlQuery = "SP_ServiceProviderCRUD";
                var parameters = new
                {
                    Mode = 1,
                    ServiceProviderId = 0,
                    serviceProvider.ServiceTypeId,
                    serviceProvider.ServiceProviderName,
                    serviceProvider.Gender,
                    serviceProvider.DOB,
                    serviceProvider.ImageName,
                    serviceProvider.MobileNumber,
                    serviceProvider.ReferredBy
                };
                // Execute the stored procedure and return the newly inserted ServiceProviderId
                return connection.ExecuteScalar<int>(sqlQuery, parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public void Delete(int serviceProviderId)
        {
            using (IDbConnection connection = new SqlConnection(_connectionString))
            {
                string sqlQuery = "SP_ServiceProviderCRUD";
                var parameters = new
                {
                    Mode = 4,
                    ServiceProviderId = serviceProviderId
                };
                // Execute the stored procedure for deleting a ServiceProvider
                connection.Execute(sqlQuery, parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public int Update(ServiceProvider serviceProvider)
        {
            using (IDbConnection connection = new SqlConnection(_connectionString))
            {
                string sqlQuery = "SP_ServiceProviderCRUD";
                var parameters = new
                {
                    Mode = 3,
                    serviceProvider.ServiceProviderId,
                    serviceProvider.ServiceTypeId,
                    serviceProvider.ServiceProviderName,
                    serviceProvider.Gender,
                    serviceProvider.DOB,
                    serviceProvider.MobileNumber,
                    serviceProvider.ReferredBy
                };
                // Execute the stored procedure and return affected rows
                return connection.Execute(sqlQuery, parameters, commandType: CommandType.StoredProcedure);
            }
        }
    }
}
