using ConstructionApplication.Core.DataModels.CostMaster;
using ConstructionApplication.Repository.Interfaces;
using Dapper;
using System.Data;
using System.Data.SqlClient;

namespace ConstructionApplication.Repository.DapperUsingSp
{
    public class CostMasterRepositoryDapperUsingSp : ICostMasterRepository
    {
        private readonly string _connectionString;

        public CostMasterRepositoryDapperUsingSp(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<CostMaster> GetByServiceType(int serviceTypeId, int siteId)
        {
            using IDbConnection connection = new SqlConnection(_connectionString);
            var parameters = new
            {
                Mode = "GET_BY_SERVICETYPE",
                ServiceTypeId = serviceTypeId,
                SiteId = siteId
            };
            return connection
                .Query<CostMaster>(
                    "Sp_CostMasterCRUD",
                    parameters,
                    commandType: CommandType.StoredProcedure)
                .ToList();
        }

        public CostMaster GetActiveCostDetail(int serviceTypeId, int siteId)
        {
            using IDbConnection connection = new SqlConnection(_connectionString);
            var parameters = new
            {
                Mode = "GET_ACTIVE_COST",
                ServiceTypeId = serviceTypeId,
                SiteId = siteId,
                CurrentDate = DateTime.Now
            };
            return connection.QueryFirstOrDefault<CostMaster>(
                       "Sp_CostMasterCRUD",
                       parameters,
                       commandType: CommandType.StoredProcedure)
                   ?? new CostMaster();
        }

        public CostMaster GetById(int id, int siteId)
        {
            using IDbConnection connection = new SqlConnection(_connectionString);
            var parameters = new
            {
                Mode = "GET_BY_ID",
                Id = id,
                SiteId = siteId
            };
            return connection.QueryFirstOrDefault<CostMaster>(
                "Sp_CostMasterCRUD",
                parameters,
                commandType: CommandType.StoredProcedure);
        }

        public int Create(CostMaster costMaster)
        {
            using IDbConnection connection = new SqlConnection(_connectionString);
            var parameters = new
            {
                Mode = "CREATE",
                ServiceTypeId = costMaster.ServiceTypeId,
                SiteId = costMaster.SiteId,
                Cost = costMaster.Cost,
                Date = costMaster.Date
            };
            return connection.ExecuteScalar<int>(
                "Sp_CostMasterCRUD",
                parameters,
                commandType: CommandType.StoredProcedure);
        }

        public int Update(CostMaster costMaster)
        {
            using IDbConnection connection = new SqlConnection(_connectionString);
            var parameters = new
            {
                Mode = "UPDATE",
                Id = costMaster.Id,
                ServiceTypeId = costMaster.ServiceTypeId,
                SiteId = costMaster.SiteId,
                Cost = costMaster.Cost,
                Date = costMaster.Date
            };
            return connection.ExecuteScalar<int>(
                "Sp_CostMasterCRUD",
                parameters,
                commandType: CommandType.StoredProcedure);
        }

        public void Delete(int id, int siteId)
        {
            using IDbConnection connection = new SqlConnection(_connectionString);
            var parameters = new
            {
                Mode = "DELETE",
                Id = id,
                SiteId = siteId
            };
            connection.Execute(
                "Sp_CostMasterCRUD",
                parameters,
                commandType: CommandType.StoredProcedure);
        }
    }
}