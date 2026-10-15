using ConstructionApplication.Repository.Interfaces;
using Dapper;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Data;
using System.Data.SqlClient;
using ConstructionApplication.Core.DataModels;

namespace ConstructionApplication.Repository.Dapper
{
    public class AddressRepositoryUsingDapper : IAddressRepository
    {
        private readonly string _connectionString;
        public AddressRepositoryUsingDapper(string connectionString)
        {
            _connectionString = connectionString;
        }

        public Address GetBySiteId(int siteId)
        {
            using (IDbConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"SELECT TOP 1 * FROM Addresses WHERE SiteId = @SiteId";
                try
                {
                    var result = connection.QueryFirstOrDefault<Address>(query, new { SiteId = siteId });
                    return result;
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        public void InsertOrUpdateAddress(Address address)
        {
            using (IDbConnection connection = new SqlConnection(_connectionString))
            {
                try
                {
                    bool isServiceProvider = address.ServiceProviderId > 0;
                    bool isSite = address.SiteId > 0;

                    string checkQuery = @"SELECT COUNT(*) FROM Addresses 
                                          WHERE " + (isServiceProvider? "ServiceProviderId = @Id" : "SiteId = @Id");
                    int addressCount;
                    try
                    {
                        addressCount = connection.ExecuteScalar<int>(checkQuery,
                            new{Id = isServiceProvider ? address.ServiceProviderId : address.SiteId});
                    }
                    catch (Exception)
                    {
                        throw;
                    }

                    if (addressCount > 0)
                    {
                        string updateQuery = @"UPDATE Addresses SET 
                               AddressLine1 = @AddressLine1, AddressTypeId = @AddressTypeId,
                               CountryId = @CountryId, PinCode = @PinCode
                               WHERE " + (isServiceProvider ? "ServiceProviderId = @Id" : "SiteId = @Id"
                        );
                        try
                        {
                            connection.Execute(updateQuery, new{address.AddressLine1, address.AddressTypeId,
                                               address.CountryId, address.PinCode, Id = isServiceProvider 
                                               ? address.ServiceProviderId : address.SiteId});
                        }
                        catch (Exception)
                        {
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
                        try
                        {
                            connection.Execute(insertQuery, new{ServiceProviderId = isServiceProvider 
                                ? (int?)address.ServiceProviderId : null, SiteId = isSite ? (int?)address.SiteId : null,
                                    address.AddressLine1, address.AddressTypeId, address.CountryId, address.PinCode});
                        }
                        catch (Exception)
                        {
                            throw;
                        }
                    }
                }
                catch
                {
                    throw;
                }
            }
        }

        public void Delete(int serviceProviderId, int? siteId)
        {
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
                try
                {
                    connection.Execute(deleteQuery, parameters);
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
    }
}