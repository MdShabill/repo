using ConstructionApplication.Core.DataModels.Site;
using ConstructionApplication.Core.Enums;
using ConstructionApplication.Repository.Interfaces;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace ConstructionApplication.Repository.Dapper
{
    public class SiteRepositoryUsingDapper : ISiteRepository
    {
        private readonly string _connectionString;

        public SiteRepositoryUsingDapper(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<Site> GetAllSites()
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                string query = @"SELECT Sites.Id, Sites.Name, Sites.StartedDate,
                                Sites.SiteStatusId, SiteStatus.Status, Addresses.AddressLine1,
                                Addresses.AddressTypeId, AddressTypes.Name AS AddressTypes,
                                Addresses.CountryId, Countries.Name AS CountryName,
                                Addresses.PinCode
                                FROM Sites
                                LEFT JOIN SiteStatus ON Sites.SiteStatusId = SiteStatus.Id
                                LEFT JOIN Addresses ON Sites.Id = Addresses.SiteId
                                LEFT JOIN AddressTypes ON Addresses.AddressTypeId = AddressTypes.Id
                                LEFT JOIN Countries ON Addresses.CountryId = Countries.Id";

                return db.Query<Site>(query).AsList();
            }
        }

        public Site GetSiteById(int id)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                string query = @"SELECT Sites.Id, Sites.Name, Sites.StartedDate,
                                Sites.SiteStatusId, SiteStatus.Status, Addresses.AddressLine1,
                                Addresses.AddressTypeId, AddressTypes.Name AS AddressTypes,
                                Addresses.CountryId, Countries.Name AS CountryName,
                                Addresses.PinCode
                                FROM Sites
                                LEFT JOIN SiteStatus ON Sites.SiteStatusId = SiteStatus.Id
                                LEFT JOIN Addresses ON Sites.Id = Addresses.SiteId
                                LEFT JOIN AddressTypes ON Addresses.AddressTypeId = AddressTypes.Id
                                LEFT JOIN Countries ON Addresses.CountryId = Countries.Id
                                WHERE Sites.Id = @Id";

                return db.QueryFirstOrDefault<Site>(query, new { Id = id });
            }
        }

        public int Create(Site site)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                string insertQuery = @"INSERT INTO Sites (Name, StartedDate, SiteStatusId, Note)
                                       VALUES (@Name, @StartedDate, @SiteStatusId, @Note);
                                       SELECT CAST(SCOPE_IDENTITY() as int);";

                return db.ExecuteScalar<int>(insertQuery, new
                {
                    site.Name,
                    site.StartedDate,
                    site.SiteStatusId,
                    Note = string.IsNullOrEmpty(site.Note) ? null : site.Note
                });
            }
        }

        public int Update(Site site)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                string updateQuery = @"UPDATE Sites SET
                                       Name = @Name,
                                       StartedDate = @StartedDate,
                                       SiteStatusId = @SiteStatusId,
                                       Note = @Note
                                       WHERE Id = @Id";

                return db.Execute(updateQuery, new
                {
                    site.Id,
                    site.Name,
                    site.StartedDate,
                    site.SiteStatusId,
                    Note = string.IsNullOrEmpty(site.Note) ? null : site.Note
                });
            }
        }

        public void Delete(int siteId)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                string deleteQuery = "DELETE FROM Sites WHERE Id = @Id";
                db.Execute(deleteQuery, new { Id = siteId });
            }
        }

        public List<int> GetServiceProviderIdsByTypes(int siteId, List<ServiceTypes> serviceTypes)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                string query = @"SELECT ServiceProviderId 
                         FROM SiteServiceProviders
                         WHERE SiteId = @SiteId 
                         AND ServiceTypeId IN @ServiceTypeIds";

                var serviceTypeIds = serviceTypes.Select(x => (int)x).ToList();

                return db.Query<int>(query, new
                {
                    SiteId = siteId,
                    ServiceTypeIds = serviceTypeIds
                }).ToList();
            }
        }

        public void AddAndUpdateSiteServiceProviderBridge(int siteId, ServiceTypes serviceType, List<int> serviceProviderIds)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                string csvIds = string.Join(",", serviceProviderIds);

                string query = @"
                        DECLARE @NewServiceProviderIds TABLE (ServiceProviderId INT);

                        INSERT INTO 
                               @NewServiceProviderIds (ServiceProviderId)
                        SELECT 
                            TRY_CAST(value AS INT)
                        FROM 
                            STRING_SPLIT(@ServiceProviderIdsCSV, ',')
                        WHERE TRY_CAST(value AS INT) IS NOT NULL;

                        DELETE FROM SiteServiceProviders
                        WHERE 
                            SiteId = @SiteId
                          AND 
                            ServiceTypeId = @ServiceTypeId
                          AND 
                            ServiceProviderId NOT IN (SELECT ServiceProviderId FROM @NewServiceProviderIds);

                        MERGE 
                            SiteServiceProviders AS target
                        USING 
                            @NewServiceProviderIds AS source
                          ON 
                            target.SiteId = @SiteId 
                          AND 
                            target.ServiceTypeId = @ServiceTypeId
                          AND 
                            target.ServiceProviderId = source.ServiceProviderId
                        WHEN NOT MATCHED BY TARGET THEN
                            INSERT 
                                (SiteId, ServiceTypeId, ServiceProviderId)
                            VALUES 
                                (@SiteId, @ServiceTypeId, source.ServiceProviderId);";

                db.Execute(query, new
                {
                    SiteId = siteId,
                    ServiceTypeId = (int)serviceType,
                    ServiceProviderIdsCSV = csvIds
                });
            }
        }
    }
}
