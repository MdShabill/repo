using ConstructionApplication.Repository.Interfaces;
using System.Data.SqlClient;
using System.Data;
using Dapper;
using ConstructionApplication.Core.Enums;
using ConstructionApplication.Core.DataModels;

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


        public List<ServiceProviderName> GetAllServiceProviders()
        {
            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                string storedProcedure = "SP_ServiceProviderCRUD";

                using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(storedProcedure, sqlConnection))
                {
                    sqlDataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;

                    sqlDataAdapter.SelectCommand.Parameters.Add("@Mode", SqlDbType.Int).Value = 2;

                    sqlDataAdapter.SelectCommand.Parameters.Add("@ServiceProviderId", SqlDbType.Int).Value = DBNull.Value;

                    sqlDataAdapter.SelectCommand.Parameters.Add("@FilterServiceTypeId", SqlDbType.Int).Value = DBNull.Value;

                    DataTable dataTable = new DataTable();

                    sqlDataAdapter.Fill(dataTable);

                    List<ServiceProviderName> serviceProviders = new List<ServiceProviderName>();

                    foreach (DataRow row in dataTable.Rows)
                    {
                        ServiceProviderName serviceProvider = new ServiceProviderName
                        {
                            Id = Convert.ToInt32(row["ServiceProviderId"]),
                            Name = Convert.ToString(row["ServiceProviderName"]) ?? "",
                            ServiceTypeId = Convert.ToInt32(row["ServiceTypeId"])
                        };

                        serviceProviders.Add(serviceProvider);
                    }

                    return serviceProviders;
                }
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
