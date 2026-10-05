using ConstructionApplication.Core.DataModels.ServiceProviders;
using ConstructionApplication.Repository.Interfaces;
using Dapper;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Data;
using System.Data.SqlClient;

namespace ConstructionApplication.Repository.Dapper
{
    public class ServiceProviderRepositoryUsingDapper : IServiceProviderRepository
    {
        private readonly string _connectionString;
        private readonly ILogger<ServiceProviderRepositoryUsingDapper> _logger;

        public ServiceProviderRepositoryUsingDapper(string connectionString,
               ILogger<ServiceProviderRepositoryUsingDapper> logger)
        {
            _connectionString = connectionString;
            _logger = logger;
        }

        public List<ServiceProvider> GetAll(int? serviceTypeId, int? id)
        {
            var methodStopwatch = Stopwatch.StartNew();

            using (IDbConnection connection = new SqlConnection(_connectionString))
            {
                string sqlQuery = @"
                       SELECT 
                           ServiceProviders.Id AS ServiceProviderId, ServiceProviders.ServiceTypeId, ServiceTypes.Name AS ServiceTypes, 
                           ServiceProviders.Name AS ServiceProviderName, ServiceProviders.Gender, ServiceProviders.DOB, 
                           ServiceProviders.MobileNumber, ServiceProviders.ReferredBy, Addresses.AddressLine1, 
                           Addresses.AddressTypeId, AddressTypes.Name AS AddressTypes, 
                           Addresses.CountryId, Countries.Name AS CountryName, Addresses.PinCode
                       FROM 
                           ServiceProviders
                       LEFT JOIN 
                            ServiceTypes ON ServiceProviders.ServiceTypeId = ServiceTypes.Id
                       LEFT JOIN 
                            Addresses ON ServiceProviders.Id = Addresses.ServiceProviderId
                       LEFT JOIN 
                            AddressTypes ON Addresses.AddressTypeId = AddressTypes.Id
                       LEFT JOIN 
                            Countries ON Addresses.CountryId = Countries.Id
                       WHERE 
                           (@serviceTypeId IS NULL OR ServiceProviders.ServiceTypeId = @serviceTypeId)
                       AND (@id IS NULL OR ServiceProviders.Id = @id);";

                var databaseStopwatch = Stopwatch.StartNew();

                try
                {
                    var result = connection.Query<ServiceProvider>(sqlQuery, new { serviceTypeId, id }).ToList();

                    databaseStopwatch.Stop();

                    _logger.LogInformation("Database: SELECT Service Providers | Time: {ElapsedMs} ms",
                        databaseStopwatch.ElapsedMilliseconds);

                    methodStopwatch.Stop();

                    _logger.LogInformation("Method: IServiceProviderRepository.GetAll | Time: {ElapsedMs} ms",
                        methodStopwatch.ElapsedMilliseconds);

                    return result;
                }
                catch (Exception ex)
                {
                    databaseStopwatch.Stop();

                    _logger.LogError(ex, "Database: SELECT Service Providers | Failed | Time: {ElapsedMs} ms",
                                     databaseStopwatch.ElapsedMilliseconds);

                    methodStopwatch.Stop();

                    _logger.LogInformation("Method: IServiceProviderRepository.GetAll | Time: {ElapsedMs} ms",
                        methodStopwatch.ElapsedMilliseconds);
                    throw;
                }
            }
        }

        public List<ServiceProviderName> GetAllServiceProviders()
        {
            var methodStopwatch = Stopwatch.StartNew();

            using (IDbConnection connection = new SqlConnection(_connectionString))
            {
                string sqlQuery = @"SELECT Id, Name, ServiceTypeId FROM ServiceProviders";

                var databaseStopwatch = Stopwatch.StartNew();

                try
                {
                    var result = connection.Query<ServiceProviderName>(sqlQuery).ToList();

                    databaseStopwatch.Stop();

                    _logger.LogInformation("Database: SELECT All Service Providers | Time: {ElapsedMs} ms",
                        databaseStopwatch.ElapsedMilliseconds);

                    methodStopwatch.Stop();

                    _logger.LogInformation("Method: IServiceProviderRepository.GetAllServiceProviders | Time: {ElapsedMs} ms",
                        methodStopwatch.ElapsedMilliseconds);

                    return result;
                }
                catch (Exception ex)
                {
                    databaseStopwatch.Stop();

                    _logger.LogError(ex, "Database: SELECT All Service Providers | Failed | Time: {ElapsedMs} ms",
                        databaseStopwatch.ElapsedMilliseconds);

                    methodStopwatch.Stop();

                    _logger.LogInformation("Method: IServiceProviderRepository.GetAllServiceProviders | Time: {ElapsedMs} ms",
                        methodStopwatch.ElapsedMilliseconds);
                    throw;
                }
            }
        }

        public int Add(ServiceProvider serviceProvider)
        {
            var methodStopwatch = Stopwatch.StartNew();

            using (IDbConnection connection = new SqlConnection(_connectionString))
            {
                string sqlQuery = @"
                        INSERT INTO ServiceProviders 
                               (ServiceTypeId, Name, Gender, DOB, ImageName, MobileNumber, ReferredBy)
                        VALUES 
                               (@ServiceTypeId, @ServiceProviderName, @Gender, @DOB, @ImageName, @MobileNumber, @ReferredBy);
                        SELECT CAST(SCOPE_IDENTITY() AS INT);";

                var databaseStopwatch = Stopwatch.StartNew();
                try
                {
                    int result = connection.ExecuteScalar<int>(sqlQuery, serviceProvider);

                    databaseStopwatch.Stop();

                    _logger.LogInformation("Database: INSERT Service Provider | Time: {ElapsedMs} ms",
                        databaseStopwatch.ElapsedMilliseconds);

                    methodStopwatch.Stop();

                    _logger.LogInformation("Method: IServiceProviderRepository.Add | Time: {ElapsedMs} ms",
                        methodStopwatch.ElapsedMilliseconds);

                    return result;
                }
                catch (Exception ex)
                {
                    databaseStopwatch.Stop();

                    _logger.LogError(ex, "Database: INSERT Service Provider | Failed | Time: {ElapsedMs} ms",
                                     databaseStopwatch.ElapsedMilliseconds);

                    methodStopwatch.Stop();

                    _logger.LogInformation("Method: IServiceProviderRepository.Add | Time: {ElapsedMs} ms",
                        methodStopwatch.ElapsedMilliseconds);

                    throw;
                }
            }
        }

        public void Delete(int serviceProviderId)
        {
            var methodStopwatch = Stopwatch.StartNew();

            using (IDbConnection connection = new SqlConnection(_connectionString))
            {
                string sqlQuery = "DELETE FROM ServiceProviders WHERE Id = @ServiceProviderId";

                var databaseStopwatch = Stopwatch.StartNew();
                try
                {
                    connection.Execute(sqlQuery, new { ServiceProviderId = serviceProviderId });

                    databaseStopwatch.Stop();

                    _logger.LogInformation("Database: DELETE Service Provider | Time: {ElapsedMs} ms",
                        databaseStopwatch.ElapsedMilliseconds);

                    methodStopwatch.Stop();

                    _logger.LogInformation("Method: IServiceProviderRepository.Delete | Time: {ElapsedMs} ms",
                        methodStopwatch.ElapsedMilliseconds);
                }
                catch (Exception ex)
                {
                    databaseStopwatch.Stop();

                    _logger.LogError(ex, "Database: DELETE Service Provider | Failed | Time: {ElapsedMs} ms",
                                     databaseStopwatch.ElapsedMilliseconds);

                    methodStopwatch.Stop();

                    _logger.LogInformation("Method: IServiceProviderRepository.Delete | Time: {ElapsedMs} ms",
                        methodStopwatch.ElapsedMilliseconds);

                    throw;
                }
            }
        }

        public int Update(ServiceProvider serviceProvider)
        {
            var methodStopwatch = Stopwatch.StartNew();

            using (IDbConnection connection = new SqlConnection(_connectionString))
            {
                string sqlQuery = @"
                       UPDATE ServiceProviders SET
                              ServiceTypeId = @ServiceTypeId,
                              Name = @ServiceProviderName,
                              Gender = @Gender,
                              DOB = @DOB,
                              MobileNumber = @MobileNumber,
                              ReferredBy = @ReferredBy
                       WHERE Id = @ServiceProviderId";

                var databaseStopwatch = Stopwatch.StartNew();

                try
                {
                    int result = connection.Execute(sqlQuery,serviceProvider);

                    databaseStopwatch.Stop();

                    _logger.LogInformation("Database: UPDATE Service Provider | Time: {ElapsedMs} ms",
                        databaseStopwatch.ElapsedMilliseconds);

                    methodStopwatch.Stop();

                    _logger.LogInformation("Method: IServiceProviderRepository.Update | Time: {ElapsedMs} ms",
                        methodStopwatch.ElapsedMilliseconds);

                    return result;
                }
                catch (Exception ex)
                {
                    databaseStopwatch.Stop();

                    _logger.LogError(ex, "Database: UPDATE Service Provider | Failed | Time: {ElapsedMs} ms",
                                     databaseStopwatch.ElapsedMilliseconds);

                    methodStopwatch.Stop();

                    _logger.LogInformation("Method: IServiceProviderRepository.Update | Time: {ElapsedMs} ms",
                        methodStopwatch.ElapsedMilliseconds);
                    throw;
                }
            }
        }
    }
}