using ConstructionApplication.Core.DataModels.ServiceProviders;
using ConstructionApplication.Core.Enums;
using ConstructionApplication.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace ConstructionApplication.Repository.Dapper
{
    public class ServiceProviderRepositoryUsingDapper : IServiceProviderRepository
    {
        private readonly string _connectionString;

        public ServiceProviderRepositoryUsingDapper(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<ServiceProvider> GetAll(int? jobCategoryId, int? id)
        {
            using (IDbConnection connection = new SqlConnection(_connectionString))
            {
                string sqlQuery = @"
                       SELECT 
                           ServiceProviders.Id AS ServiceProviderId, ServiceProviders.JobCategoryId, JobCategories.Name AS JobTypes, 
                           ServiceProviders.Name AS ServiceProviderName, ServiceProviders.Gender, ServiceProviders.DOB, 
                           ServiceProviders.MobileNumber, ServiceProviders.ReferredBy, Addresses.AddressLine1, 
                           Addresses.AddressTypeId, AddressTypes.Name AS AddressTypes, 
                           Addresses.CountryId, Countries.Name AS CountryName, Addresses.PinCode
                       FROM 
                           ServiceProviders
                       LEFT JOIN 
                            JobCategories ON ServiceProviders.JobCategoryId = JobCategories.Id
                       LEFT JOIN 
                            Addresses ON ServiceProviders.Id = Addresses.ServiceProviderId
                       LEFT JOIN 
                            AddressTypes ON Addresses.AddressTypeId = AddressTypes.Id
                       LEFT JOIN 
                            Countries ON Addresses.CountryId = Countries.Id
                       WHERE 
                           (@jobCategoryId IS NULL OR ServiceProviders.JobCategoryId = @jobCategoryId)
                       AND (@id IS NULL OR ServiceProviders.Id = @id);";
                // Execute query and return mapped list
                return connection.Query<ServiceProvider>(sqlQuery, new { jobCategoryId, id }).ToList();
            }
        }

        public int Add(ServiceProvider serviceProvider)
        {
            using (IDbConnection connection = new SqlConnection(_connectionString))
            {
                string sqlQuery = @"
                        INSERT INTO ServiceProviders 
                               (JobCategoryId, Name, Gender, DOB, ImageName, MobileNumber, ReferredBy)
                        VALUES 
                               (@JobCategoryId, @ServiceProviderName, @Gender, @DOB, @ImageName, @MobileNumber, @ReferredBy);
                        SELECT CAST(SCOPE_IDENTITY() AS INT);";

                // Executes the SQL query and returns the newly inserted ServiceProviderId
                return connection.ExecuteScalar<int>(sqlQuery, serviceProvider);
            }
        }

        public void Delete(int serviceProviderId)
        {
            using (IDbConnection connection = new SqlConnection(_connectionString))
            {
                string sqlQuery = "DELETE FROM ServiceProviders WHERE Id = @ServiceProviderId";
                // Executes the delete query
                connection.Execute(sqlQuery, new { ServiceProviderId = serviceProviderId });
            }
        }


        public int Update(ServiceProvider serviceProvider)
        {
            using (IDbConnection connection = new SqlConnection(_connectionString))
            {
                string sqlQuery = @"
                       UPDATE ServiceProviders SET
                              JobCategoryId = @JobCategoryId,
                              Name = @ServiceProviderName,
                              Gender = @Gender,
                              DOB = @DOB,
                              MobileNumber = @MobileNumber,
                              ReferredBy = @ReferredBy
                       WHERE Id = @ServiceProviderId";
                // Executes and returns affected rows
                return connection.Execute(sqlQuery, serviceProvider);
            }
        }
    }
}
