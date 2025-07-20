using ConstructionApplication.Core.DataModels.CostMaster;
using ConstructionApplication.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using ConstructionApplication.Core.DataModels.ServiceTypes;

namespace ConstructionApplication.Repository.Dapper
{
    public class CostMasterRepositoryUsingDapper : ICostMasterRepository
    {
        private readonly string _connectionString;

        public CostMasterRepositoryUsingDapper(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<CostMaster> GetByServiceType(int serviceTypeId)
        {
            using (IDbConnection connection = new SqlConnection(_connectionString))
            {
                string sqlQuery = @"
                        SELECT 
                            CostMaster.Id, CostMaster.ServiceTypeId, 
                            ServiceTypes.Name, CostMaster.Cost, CostMaster.Date
                        FROM 
                            CostMaster
                        JOIN 
                            ServiceTypes ON CostMaster.ServiceTypeId = ServiceTypes.Id
                        WHERE 
                            CostMaster.ServiceTypeId = @serviceTypeId
                        ORDER BY CostMaster.Date DESC";

                return connection.Query<CostMaster>(sqlQuery, new { serviceTypeId }).ToList();
            }
        }

        public CostMaster GetActiveCostDetail(int serviceTypeId)
        {
            using (IDbConnection connection = new SqlConnection(_connectionString))
            {
                string sqlQuery = @"
                        SELECT 
                            TOP 1 CostMaster.ServiceTypeId, ServiceTypes.Name, 
                                  CostMaster.Cost, CostMaster.Date
                        FROM 
                            CostMaster 
                        JOIN 
                            ServiceTypes ON CostMaster.ServiceTypeId = ServiceTypes.Id 
                        WHERE 
                            CostMaster.ServiceTypeId = @serviceTypeId 
                            AND CostMaster.Date <= @currentDate 
                        ORDER BY CostMaster.Date DESC";

                return connection.QueryFirstOrDefault<CostMaster>(sqlQuery,
                    new { serviceTypeId, currentDate = DateTime.Now }) ?? new CostMaster();
            }
        }

        public int Create(CostMaster costMaster)
        {
            using (IDbConnection connection = new SqlConnection(_connectionString))
            {
                string insertQuery = @"
                       INSERT INTO CostMaster 
                             (ServiceTypeId, Cost, Date)
                       VALUES 
                             (@serviceTypeId, @Cost, @Date);
                       SELECT CAST(SCOPE_IDENTITY() AS INT);";

                return connection.ExecuteScalar<int>(insertQuery, costMaster);
            }
        }
    }
}
