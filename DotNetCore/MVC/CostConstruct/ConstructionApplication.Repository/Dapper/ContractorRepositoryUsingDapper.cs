using ConstructionApplication.Core.DataModels.Contractor;
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
    public class ContractorRepositoryUsingDapper : IContractorRepository
    {
        private readonly string _connectionString;

        public ContractorRepositoryUsingDapper(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<Contractor> GetAll(int? jobCategoryId, int? id)
        {
            using (IDbConnection connection = new SqlConnection(_connectionString))
            {
                string sqlQuery = @"
                       SELECT 
                           Contractors.Id AS ContractorId, Contractors.JobCategoryId, JobCategories.Name AS JobTypes, 
                           Contractors.Name AS ContractorName, Contractors.Gender, Contractors.DOB, 
                           Contractors.MobileNumber, Contractors.ReferredBy, Addresses.AddressLine1, 
                           Addresses.AddressTypeId, AddressTypes.Name AS AddressTypes, 
                           Addresses.CountryId, Countries.Name AS CountryName, Addresses.PinCode
                       FROM 
                           Contractors
                       LEFT JOIN 
                            JobCategories ON Contractors.JobCategoryId = JobCategories.Id
                       LEFT JOIN 
                            Addresses ON Contractors.Id = Addresses.ContractorId
                       LEFT JOIN 
                            AddressTypes ON Addresses.AddressTypeId = AddressTypes.Id
                       LEFT JOIN 
                            Countries ON Addresses.CountryId = Countries.Id
                       WHERE 
                           (@jobCategoryId IS NULL OR Contractors.JobCategoryId = @jobCategoryId)
                       AND (@id IS NULL OR Contractors.Id = @id);";
                // Execute query and return mapped list
                return connection.Query<Contractor>(sqlQuery, new { jobCategoryId, id }).ToList();
            }
        }

        public int Add(Contractor contractor)
        {
            using (IDbConnection connection = new SqlConnection(_connectionString))
            {
                string sqlQuery = @"
                        INSERT INTO Contractors 
                               (JobCategoryId, Name, Gender, DOB, ImageName, MobileNumber, ReferredBy)
                        VALUES 
                               (@JobCategoryId, @ContractorName, @Gender, @DOB, @ImageName, @MobileNumber, @ReferredBy);
                        SELECT CAST(SCOPE_IDENTITY() AS INT);";

                // Executes the SQL query and returns the newly inserted ContractorId
                return connection.ExecuteScalar<int>(sqlQuery, contractor);
            }
        }

        public void Delete(int contractorId)
        {
            using (IDbConnection connection = new SqlConnection(_connectionString))
            {
                string sqlQuery = "DELETE FROM Contractors WHERE Id = @ContractorId";
                // Executes the delete query
                connection.Execute(sqlQuery, new { ContractorId = contractorId });
            }
        }


        public int Update(Contractor contractor)
        {
            using (IDbConnection connection = new SqlConnection(_connectionString))
            {
                string sqlQuery = @"
                       UPDATE Contractors SET
                              JobCategoryId = @JobCategoryId,
                              Name = @ContractorName,
                              Gender = @Gender,
                              DOB = @DOB,
                              MobileNumber = @MobileNumber,
                              ReferredBy = @ReferredBy
                       WHERE Id = @ContractorId";
                // Executes and returns affected rows
                return connection.Execute(sqlQuery, contractor);
            }
        }
    }
}
