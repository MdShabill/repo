using ConstructionApplication.Core.DataModels.Site;
using ConstructionApplication.Core.Enums;
using ConstructionApplication.Repository.Interfaces;
using Dapper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;

namespace ConstructionApplication.Repository.Dapper
{
    public class SiteRepositoryUsingDapper : ISiteRepository
    {
        private readonly string _connectionString;
        private readonly ILogger<SiteRepositoryUsingDapper> _logger;

        public SiteRepositoryUsingDapper(string connectionString,
               ILogger<SiteRepositoryUsingDapper> logger)
        {
            _connectionString = connectionString;
            _logger = logger;
        }

        public List<Site> GetAllSites()
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                string query = @"SELECT 
                                    Sites.Id, Sites.Name, Sites.ContactName, Sites.ContactNumber,
                                    Sites.StartedDate, Sites.SiteStatusId, Sites.ExpectedBudget, 
                                    Sites.ExpectedCompletionDate, SiteStatus.Status, 
                                    Addresses.AddressLine1, Addresses.AddressTypeId, 
                                    AddressTypes.Name AS AddressTypes, Addresses.CountryId, 
                                    Countries.Name AS CountryName, Addresses.PinCode
                                FROM 
                                    Sites
                                LEFT JOIN 
                                    SiteStatus ON Sites.SiteStatusId = SiteStatus.Id
                                LEFT JOIN 
                                    Addresses ON Sites.Id = Addresses.SiteId
                                LEFT JOIN   
                                    AddressTypes ON Addresses.AddressTypeId = AddressTypes.Id
                                LEFT JOIN 
                                    Countries ON Addresses.CountryId = Countries.Id";

                Stopwatch dbTimer = Stopwatch.StartNew();

                try
                {
                    var result = db.Query<Site>(query).AsList();

                    dbTimer.Stop();

                    _logger.LogInformation("\nDatabase: SELECT All Sites | Time: {ElapsedMs} ms",
                        dbTimer.ElapsedMilliseconds);

                    return result;
                }
                catch (Exception ex)
                {
                    dbTimer.Stop();

                    _logger.LogError(ex,"\nDatabase: SELECT All Sites | Failed | Time: {ElapsedMs} ms",
                                     dbTimer.ElapsedMilliseconds);

                    throw;
                }
            }
        }

        public List<Site> GetSites(string? search, int? statusId, DateTime? fromDate,
                                   DateTime? toDate, decimal? budgetFrom, decimal? budgetTo)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                string query = @"SELECT 
                        Sites.Id, Sites.Name, Sites.ContactName, Sites.ContactNumber, Sites.StartedDate,
                        Sites.SiteStatusId, Sites.ExpectedBudget, Sites.ExpectedCompletionDate,
                        SiteStatus.Status, Addresses.AddressLine1, Addresses.AddressTypeId,
                        AddressTypes.Name AS AddressTypes, Addresses.CountryId,
                        Countries.Name AS CountryName, Addresses.PinCode
                    FROM 
                        Sites

                    LEFT JOIN SiteStatus
                        ON Sites.SiteStatusId = SiteStatus.Id

                    LEFT JOIN Addresses
                        ON Sites.Id = Addresses.SiteId

                    LEFT JOIN AddressTypes
                        ON Addresses.AddressTypeId = AddressTypes.Id

                    LEFT JOIN Countries
                        ON Addresses.CountryId = Countries.Id

                    WHERE
                        (
                            @Search IS NULL
                            OR @Search = ''
                            OR Sites.Name LIKE '%' + @Search + '%'
                            OR Sites.ContactName LIKE '%' + @Search + '%'
                        )

                        AND
                        (
                            @StatusId IS NULL
                            OR Sites.SiteStatusId = @StatusId
                        )

                        AND
                        (
                            @FromDate IS NULL
                            OR
                            (
                                Sites.ExpectedCompletionDate IS NULL
                                OR Sites.ExpectedCompletionDate >= @FromDate
                            )
                        )

                        AND
                        (
                            @ToDate IS NULL
                            OR
                            (
                                Sites.StartedDate IS NULL
                                OR Sites.StartedDate <= @ToDate
                            )
                        )

                        AND
                        (
                            @BudgetFrom IS NULL
                            OR Sites.ExpectedBudget >= @BudgetFrom
                        )

                        AND
                        (
                            @BudgetTo IS NULL
                            OR Sites.ExpectedBudget <= @BudgetTo
                        )";

                var parameters = new
                {
                    Search = string.IsNullOrWhiteSpace(search) ? null : search.Trim(),
                    StatusId = statusId,
                    FromDate = fromDate?.Date,
                    ToDate = toDate?.Date,
                    BudgetFrom = budgetFrom,
                    BudgetTo = budgetTo
                };

                Stopwatch dbTimer = Stopwatch.StartNew();

                try
                {
                    var result =
                        db.Query<Site>(query, parameters).AsList();

                    dbTimer.Stop();

                    _logger.LogInformation("\nDatabase: SELECT Filtered Sites | Time: {ElapsedMs} ms",
                        dbTimer.ElapsedMilliseconds);

                    return result;
                }
                catch (Exception ex)
                {
                    dbTimer.Stop();

                    _logger.LogError(ex, "\nDatabase: SELECT Filtered Sites | Failed | Time: {ElapsedMs} ms",
                                     dbTimer.ElapsedMilliseconds);
                    throw;
                }
            }
        }

        public Site GetSiteById(int id)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                string query = @"SELECT 
                                    Sites.Id, Sites.Name, Sites.ContactName, Sites.ContactNumber,
                                    Sites.StartedDate, Sites.ExpectedBudget, 
                                    Sites.ExpectedCompletionDate, Sites.SiteStatusId, 
                                    SiteStatus.Status, Addresses.AddressLine1, 
                                    AddressTypes.Name AS AddressTypes, Countries.Name AS CountryName, 
                                    Addresses.PinCode
                                FROM 
                                    Sites
                                LEFT JOIN 
                                    SiteStatus ON Sites.SiteStatusId = SiteStatus.Id
                                LEFT JOIN 
                                    Addresses ON Sites.Id = Addresses.SiteId
                                LEFT JOIN 
                                    AddressTypes ON Addresses.AddressTypeId = AddressTypes.Id
                                LEFT JOIN 
                                    Countries ON Addresses.CountryId = Countries.Id
                                WHERE Sites.Id = @Id";

                Stopwatch dbTimer = Stopwatch.StartNew();

                try
                {
                    var result = db.QueryFirstOrDefault<Site>(query, new { Id = id });

                    dbTimer.Stop();

                    _logger.LogInformation("\nDatabase: SELECT Site By ID | Time: {ElapsedMs} ms",
                        dbTimer.ElapsedMilliseconds);

                    return result;
                }
                catch (Exception ex)
                {
                    dbTimer.Stop();

                    _logger.LogError(ex, "\nDatabase: SELECT Site By ID | Failed | Time: {ElapsedMs} ms",
                                     dbTimer.ElapsedMilliseconds);
                    throw;
                }
            }
        }

        public int Create(Site site)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                string insertQuery = @"
                                     INSERT INTO Sites 
                                       (Name, ContactName, ContactNumber, StartedDate, SiteStatusId, 
                                            Note, ExpectedBudget, ExpectedCompletionDate)
                                     VALUES 
                                       (@Name, @ContactName, @ContactNumber, @StartedDate, @SiteStatusId, 
                                            @Note, @ExpectedBudget, @ExpectedCompletionDate);
                                     SELECT CAST(SCOPE_IDENTITY() as int);";

                Stopwatch dbTimer = Stopwatch.StartNew();

                try
                {
                    int insertedId =db.ExecuteScalar<int>(insertQuery,
                            new
                            {
                                site.Name,
                                site.ContactName,
                                site.ContactNumber,
                                site.StartedDate,
                                site.SiteStatusId,
                                Note = string.IsNullOrEmpty(site.Note)? null
                                    : site.Note,
                                site.ExpectedBudget,
                                site.ExpectedCompletionDate
                            });

                    dbTimer.Stop();

                    _logger.LogInformation(
                        "\nDatabase: INSERT Site | Time: {ElapsedMs} ms",
                        dbTimer.ElapsedMilliseconds);

                    return insertedId;
                }
                catch (Exception ex)
                {
                    dbTimer.Stop();

                    _logger.LogError(
                        ex,
                        "\nDatabase: INSERT Site | Failed | Time: {ElapsedMs} ms",
                        dbTimer.ElapsedMilliseconds);
                    throw;
                }
            }
        }

        public int Update(Site site)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                string updateQuery = @"UPDATE Sites SET
                                        Name = @Name,
                                        ContactName = @ContactName,
                                        ContactNumber = @ContactNumber,
                                        StartedDate = @StartedDate,
                                        SiteStatusId = @SiteStatusId,
                                        Note = @Note,
                                        ExpectedBudget = @ExpectedBudget,
                                        ExpectedCompletionDate = @ExpectedCompletionDate
                                       WHERE Id = @Id";

                Stopwatch dbTimer = Stopwatch.StartNew();

                try
                {
                    int affectedRows = db.Execute(updateQuery,
                            new
                            {
                                site.Id,
                                site.Name,
                                site.ContactName,
                                site.ContactNumber,
                                site.StartedDate,
                                site.SiteStatusId,
                                Note = string.IsNullOrEmpty(site.Note) ? null : site.Note,
                                site.ExpectedBudget,
                                site.ExpectedCompletionDate
                            });

                    dbTimer.Stop();

                    _logger.LogInformation("\nDatabase: UPDATE Site | Time: {ElapsedMs} ms",
                        dbTimer.ElapsedMilliseconds);

                    return affectedRows;
                }
                catch (Exception ex)
                {
                    dbTimer.Stop();

                    _logger.LogError(ex, "\nDatabase: UPDATE Site | Failed | Time: {ElapsedMs} ms",
                                     dbTimer.ElapsedMilliseconds);
                    throw;
                }
            }
        }

        public void Delete(int siteId)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                db.Open();

                using (var transaction = db.BeginTransaction())
                {
                    Stopwatch dbTimer = Stopwatch.StartNew();

                    try
                    {
                        db.Execute("DELETE FROM Addresses WHERE SiteId = @SiteId", new { SiteId = siteId },
                            transaction);

                        db.Execute("DELETE FROM SiteServiceProviders WHERE SiteId = @SiteId", new { SiteId = siteId },
                            transaction);

                        db.Execute("DELETE FROM Sites WHERE Id = @Id", new { Id = siteId },
                            transaction);

                        transaction.Commit();

                        dbTimer.Stop();

                        _logger.LogInformation("\nDatabase: DELETE Site | Time: {ElapsedMs} ms",
                            dbTimer.ElapsedMilliseconds);
                    }
                    catch (Exception ex)
                    {
                        dbTimer.Stop();

                        transaction.Rollback();

                        _logger.LogError(ex, "\nDatabase: DELETE Site | Failed | Time: {ElapsedMs} ms",
                                         dbTimer.ElapsedMilliseconds);
                        throw;
                    }
                }
            }
        }

        public List<int> GetServiceProviderIdsByTypes(int siteId, List<ServiceTypes> serviceTypes)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                string query = @"SELECT 
                                    ServiceProviderId 
                                 FROM 
                                    SiteServiceProviders
                                 WHERE 
                                    SiteId = @SiteId 
                                 AND 
                                    ServiceTypeId IN @ServiceTypeIds";

                var serviceTypeIds = serviceTypes.Select(x => (int)x).ToList();

                Stopwatch dbTimer = Stopwatch.StartNew();

                try
                {
                    var result = db.Query<int>(query,
                            new
                            {
                                SiteId = siteId,
                                ServiceTypeIds = serviceTypeIds
                            }).ToList();

                    dbTimer.Stop();

                    _logger.LogInformation("\nDatabase: SELECT Service Provider IDs | Time: {ElapsedMs} ms",
                        dbTimer.ElapsedMilliseconds);

                    return result;
                }
                catch (Exception ex)
                {
                    dbTimer.Stop();

                    _logger.LogError(ex, "\nDatabase: SELECT Service Provider IDs | Failed | Time: {ElapsedMs} ms",
                                     dbTimer.ElapsedMilliseconds);
                    throw;
                }
            }
        }

        // Approach: 1
        public void AddAndUpdateSiteServiceProviderBridge(int siteId, ServiceTypes serviceType,
                                                          List<int> serviceProviderIds)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                string deleteQuery = @"DELETE 
                                       FROM 
                                            SiteServiceProviders 
                                       WHERE 
                                            SiteId = @SiteId 
                                       AND 
                                            ServiceTypeId = @ServiceTypeId";

                string insertQuery = @"INSERT INTO 
                                            SiteServiceProviders 
                                                (SiteId, ServiceProviderId, ServiceTypeId)
                                            VALUES 
                                                (@SiteId, @ServiceProviderId, @ServiceTypeId)";

                Stopwatch dbTimer = Stopwatch.StartNew();

                try
                {
                    db.Execute(deleteQuery,
                        new
                        {
                            SiteId = siteId,
                            ServiceTypeId = (int)serviceType
                        });

                    for (int i = 0; i < serviceProviderIds.Count; i++)
                    {
                        db.Execute(insertQuery,
                            new
                            {
                                SiteId = siteId,
                                ServiceProviderId = serviceProviderIds[i],
                                ServiceTypeId = (int)serviceType
                            });
                    }

                    dbTimer.Stop();

                    _logger.LogInformation("\nDatabase: DELETE/INSERT Site Service Providers | Time: {ElapsedMs} ms",
                        dbTimer.ElapsedMilliseconds);
                }
                catch (Exception ex)
                {
                    dbTimer.Stop();

                    _logger.LogError(ex, "\nDatabase: DELETE/INSERT Site Service Providers | Failed | Time: {ElapsedMs} ms",
                                     dbTimer.ElapsedMilliseconds);
                    throw;
                }
            }
        }

        // ============================================================
        // APPROACH: 2
        // ============================================================

        //public void AddAndUpdateSiteServiceProviderBridge(
        //    int siteId,
        //    ServiceTypes serviceType,
        //    List<int> serviceProviderIds)
        //{
        //    using (IDbConnection db = new SqlConnection(_connectionString))
        //    {
        //        string csvIds = string.Join(",", serviceProviderIds);

        //        string query = @"
        //                DECLARE @NewServiceProviderIds TABLE (ServiceProviderId INT);

        //                INSERT INTO 
        //                       @NewServiceProviderIds (ServiceProviderId)
        //                SELECT 
        //                    TRY_CAST(value AS INT)
        //                FROM 
        //                    STRING_SPLIT(@ServiceProviderIdsCSV, ',')
        //                WHERE TRY_CAST(value AS INT) IS NOT NULL;

        //                DELETE FROM SiteServiceProviders
        //                WHERE 
        //                    SiteId = @SiteId
        //                  AND 
        //                    ServiceTypeId = @ServiceTypeId
        //                  AND 
        //                    ServiceProviderId NOT IN (SELECT ServiceProviderId FROM @NewServiceProviderIds);

        //                MERGE 
        //                    SiteServiceProviders AS target
        //                USING 
        //                    @NewServiceProviderIds AS source
        //                  ON 
        //                    target.SiteId = @SiteId 
        //                  AND 
        //                    target.ServiceTypeId = @ServiceTypeId
        //                  AND 
        //                    target.ServiceProviderId = source.ServiceProviderId
        //                WHEN NOT MATCHED BY TARGET THEN
        //                    INSERT 
        //                        (SiteId, ServiceTypeId, ServiceProviderId)
        //                    VALUES 
        //                        (@SiteId, @ServiceTypeId, source.ServiceProviderId);";

        //        db.Execute(query, new
        //        {
        //            SiteId = siteId,
        //            ServiceTypeId = (int)serviceType,
        //            ServiceProviderIdsCSV = csvIds
        //        });
        //    }
        //}
    }
}