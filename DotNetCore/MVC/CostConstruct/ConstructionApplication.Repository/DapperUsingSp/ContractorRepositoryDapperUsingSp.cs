using ConstructionApplication.Core.DataModels.Contractor;
using ConstructionApplication.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace ConstructionApplication.Repository.DapperUsingSp
{
    public class ContractorRepositoryDapperUsingSp : IContractorRepository
    {
        private readonly string _connectionString;

        public ContractorRepositoryDapperUsingSp(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<Contractor> GetAll(int? jobCategoryId, int? id)
        {
            using (IDbConnection connection = new SqlConnection(_connectionString))
            {
                string sqlQuery = "SP_ConreactorCRUD";
                var parameters = new
                {
                    Mode = 2,
                    ContractorId = id ?? 0,
                    FilterJobCategoryId = jobCategoryId ?? 0
                };
                // Execute the stored procedure and return mapped list of contractors
                return connection.Query<Contractor>(sqlQuery, parameters, commandType: CommandType.StoredProcedure).ToList();
            }
        }

        public int Add(Contractor contractor)
        {
            using (IDbConnection connection = new SqlConnection(_connectionString))
            {
                string sqlQuery = "SP_ConreactorCRUD";
                var parameters = new
                {
                    Mode = 1,
                    ContractorId = 0,
                    contractor.JobCategoryId,
                    contractor.ContractorName,
                    contractor.Gender,
                    contractor.DOB,
                    contractor.ImageName,
                    contractor.MobileNumber,
                    contractor.ReferredBy
                };
                // Execute the stored procedure and return the newly inserted ContractorId
                return connection.ExecuteScalar<int>(sqlQuery, parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public void Delete(int contractorId)
        {
            using (IDbConnection connection = new SqlConnection(_connectionString))
            {
                string sqlQuery = "SP_ConreactorCRUD";
                var parameters = new
                {
                    Mode = 4,
                    ContractorId = contractorId
                };
                // Execute the stored procedure for deleting a contractor
                connection.Execute(sqlQuery, parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public int Update(Contractor contractor)
        {
            using (IDbConnection connection = new SqlConnection(_connectionString))
            {
                string sqlQuery = "SP_ConreactorCRUD";
                var parameters = new
                {
                    Mode = 3,
                    contractor.ContractorId,
                    contractor.JobCategoryId,
                    contractor.ContractorName,
                    contractor.Gender,
                    contractor.DOB,
                    contractor.MobileNumber,
                    contractor.ReferredBy
                };
                // Execute the stored procedure and return affected rows
                return connection.Execute(sqlQuery, parameters, commandType: CommandType.StoredProcedure);
            }
        }
    }
}
