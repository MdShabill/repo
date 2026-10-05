using ConstructionApplication.Core.DataModels.Address;
using ConstructionApplication.Repository.Interfaces;
using Dapper;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Data;
using System.Data.SqlClient;

namespace ConstructionApplication.Repository.Dapper
{
    public class AddressRepositoryUsingDapper : IAddressRepository
    {
        private readonly string _connectionString;
        private readonly ILogger<AddressRepositoryUsingDapper> _logger;

        public AddressRepositoryUsingDapper(string connectionString,
               ILogger<AddressRepositoryUsingDapper> logger)
        {
            _connectionString = connectionString;
            _logger = logger;
        }

        public Address GetBySiteId(int siteId)
        {
            var methodStopwatch = Stopwatch.StartNew();

            using (IDbConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"SELECT TOP 1 * FROM Addresses WHERE SiteId = @SiteId";

                var databaseStopwatch = Stopwatch.StartNew();

                try
                {
                    var result = connection.QueryFirstOrDefault<Address>(query, new { SiteId = siteId });

                    databaseStopwatch.Stop();

                    _logger.LogInformation("Database: SELECT Address By Site ID | Time: {ElapsedMs} ms",
                        databaseStopwatch.ElapsedMilliseconds);

                    methodStopwatch.Stop();

                    _logger.LogInformation("Method: IAddressRepository.GetBySiteId | Time: {ElapsedMs} ms",
                        methodStopwatch.ElapsedMilliseconds);

                    return result;
                }
                catch (Exception ex)
                {
                    databaseStopwatch.Stop();

                    _logger.LogError(ex,"Database: SELECT Address By Site ID | Failed | Time: {ElapsedMs} ms",
                                     databaseStopwatch.ElapsedMilliseconds);

                    methodStopwatch.Stop();

                    _logger.LogInformation("Method: IAddressRepository.GetBySiteId | Time: {ElapsedMs} ms",
                        methodStopwatch.ElapsedMilliseconds);
                    throw;
                }
            }
        }

        public void InsertOrUpdateAddress(Address address)
        {
            var methodStopwatch = Stopwatch.StartNew();

            using (IDbConnection connection = new SqlConnection(_connectionString))
            {
                try
                {
                    bool isServiceProvider = address.ServiceProviderId > 0;
                    bool isSite = address.SiteId > 0;

                    string checkQuery = @"SELECT COUNT(*) 
                                          FROM 
                                                Addresses 
                                          WHERE 
                                              " + (isServiceProvider
                                              ? "ServiceProviderId = @Id"
                                              : "SiteId = @Id");

                    int addressCount;

                    var checkStopwatch = Stopwatch.StartNew();

                    try
                    {
                        addressCount = connection.ExecuteScalar<int>(checkQuery,
                            new
                            {
                                Id = isServiceProvider ? address.ServiceProviderId : address.SiteId
                            });

                        checkStopwatch.Stop();

                        _logger.LogInformation("Database: SELECT Address Exists | Time: {ElapsedMs} ms",
                            checkStopwatch.ElapsedMilliseconds);
                    }
                    catch (Exception ex)
                    {
                        checkStopwatch.Stop();

                        _logger.LogError(ex, "Database: SELECT Address Exists | Failed | Time: {ElapsedMs} ms",
                                         checkStopwatch.ElapsedMilliseconds);
                        throw;
                    }

                    if (addressCount > 0)
                    {
                        string updateQuery = @"
                               UPDATE Addresses SET 
                                   AddressLine1 = @AddressLine1,
                                   AddressTypeId = @AddressTypeId,
                                   CountryId = @CountryId,
                                   PinCode = @PinCode
                               WHERE " + (isServiceProvider
                                   ? "ServiceProviderId = @Id"
                                   : "SiteId = @Id");

                        var updateStopwatch = Stopwatch.StartNew();

                        try
                        {
                            connection.Execute(updateQuery,
                                new
                                {
                                    address.AddressLine1,
                                    address.AddressTypeId,
                                    address.CountryId,
                                    address.PinCode,
                                    Id = isServiceProvider
                                        ? address.ServiceProviderId
                                        : address.SiteId
                                });

                            updateStopwatch.Stop();

                            _logger.LogInformation("Database: UPDATE Address | Time: {ElapsedMs} ms",
                                updateStopwatch.ElapsedMilliseconds);
                        }
                        catch (Exception ex)
                        {
                            updateStopwatch.Stop();

                            _logger.LogError(ex, "Database: UPDATE Address | Failed | Time: {ElapsedMs} ms",
                                             updateStopwatch.ElapsedMilliseconds);
                            throw;
                        }
                    }
                    else
                    {
                        string insertQuery = @"
                               INSERT INTO Addresses 
                                   (ServiceProviderId, SiteId, AddressLine1, AddressTypeId, CountryId, PinCode)
                               VALUES 
                                   (@ServiceProviderId, @SiteId, @AddressLine1, @AddressTypeId, @CountryId, @PinCode)";

                        var insertStopwatch = Stopwatch.StartNew();

                        try
                        {
                            connection.Execute(insertQuery,
                                new
                                {
                                    ServiceProviderId = isServiceProvider
                                        ? (int?)address.ServiceProviderId
                                        : null,
                                    SiteId = isSite
                                        ? (int?)address.SiteId
                                        : null,
                                    address.AddressLine1,
                                    address.AddressTypeId,
                                    address.CountryId,
                                    address.PinCode
                                });

                            insertStopwatch.Stop();

                            _logger.LogInformation("Database: INSERT Address | Time: {ElapsedMs} ms",
                                insertStopwatch.ElapsedMilliseconds);
                        }
                        catch (Exception ex)
                        {
                            insertStopwatch.Stop();

                            _logger.LogError(ex, "Database: INSERT Address | Failed | Time: {ElapsedMs} ms",
                                             insertStopwatch.ElapsedMilliseconds);

                            throw;
                        }
                    }

                    methodStopwatch.Stop();

                    _logger.LogInformation("Method: IAddressRepository.InsertOrUpdateAddress | Time: {ElapsedMs} ms",
                        methodStopwatch.ElapsedMilliseconds);
                }
                catch
                {
                    methodStopwatch.Stop();

                    _logger.LogInformation("Method: IAddressRepository.InsertOrUpdateAddress | Time: {ElapsedMs} ms",
                        methodStopwatch.ElapsedMilliseconds);

                    throw;
                }
            }
        }

        public void Delete(int serviceProviderId, int? siteId)
        {
            var methodStopwatch = Stopwatch.StartNew();

            using (IDbConnection connection = new SqlConnection(_connectionString))
            {
                string deleteQuery;
                object parameters;

                if (siteId.HasValue && siteId.Value > 0)
                {
                    deleteQuery = "DELETE FROM Addresses WHERE SiteId = @SiteId";
                    parameters = new { SiteId = siteId.Value };
                }
                else
                {
                    deleteQuery = "DELETE FROM Addresses WHERE ServiceProviderId = @ServiceProviderId";
                    parameters = new { ServiceProviderId = serviceProviderId };
                }

                var databaseStopwatch = Stopwatch.StartNew();

                try
                {
                    connection.Execute(deleteQuery, parameters);

                    databaseStopwatch.Stop();

                    _logger.LogInformation("Database: DELETE Address | Time: {ElapsedMs} ms",
                        databaseStopwatch.ElapsedMilliseconds);

                    methodStopwatch.Stop();

                    _logger.LogInformation("Method: IAddressRepository.Delete | Time: {ElapsedMs} ms",
                        methodStopwatch.ElapsedMilliseconds);
                }
                catch (Exception ex)
                {
                    databaseStopwatch.Stop();

                    _logger.LogError(ex, "Database: DELETE Address | Failed | Time: {ElapsedMs} ms",
                                     databaseStopwatch.ElapsedMilliseconds);

                    methodStopwatch.Stop();

                    _logger.LogInformation("Method: IAddressRepository.Delete | Time: {ElapsedMs} ms",
                        methodStopwatch.ElapsedMilliseconds);
                    throw;
                }
            }
        }
    }
}