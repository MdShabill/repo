using ConstructionApplication.Core.DataModels.CostMaster;
using ConstructionApplication.Core.DataModels.ServiceTypes;
using ConstructionApplication.Core.DataModels.Material;
using ConstructionApplication.Repository.Interfaces;
using Microsoft.VisualBasic;
using System.Data;
using System.Data.SqlClient;

namespace ConstructionApplication.Repository.AdoDotNet
{
    public class CostMasterRepository : ICostMasterRepository
    {
        private readonly string _connectionString;

        public CostMasterRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<CostMaster> GetByServiceType(int serviceTypeId, int siteId)
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                string sqlQuery = @"
                       SELECT 
                           CostMaster.Id, CostMaster.ServiceTypeId, 
                           ServiceTypes.Name, CostMaster.Cost, CostMaster.Date, CostMaster.SiteId
                       FROM 
                           CostMaster
                       JOIN 
                           ServiceTypes ON CostMaster.ServiceTypeId = ServiceTypes.Id
                       WHERE 
                           CostMaster.ServiceTypeId = @serviceTypeId
                         AND 
                           CostMaster.SiteId = @siteId
                       ORDER BY 
                           CostMaster.Date DESC";

                SqlDataAdapter sqlDataAdapter = new(sqlQuery, sqlConnection);
                sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@serviceTypeId", serviceTypeId);
                sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@siteId", siteId);

                DataTable dataTable = new();
                sqlDataAdapter.Fill(dataTable);

                List<CostMaster> costMasters = new();

                //Approach: 1 = With For Loop
                //for (int i = 0; i < dataTable.Rows.Count; i++)
                //{
                //    CostMaster costMaster = new()
                //    {
                //        Id = (int)dataTable.Rows[i]["Id"],
                //        ServiceTypeId = (int)dataTable.Rows[i]["ServiceTypeId"],
                //        Name = (string)dataTable.Rows[i]["Name"],
                //        Cost = (decimal)dataTable.Rows[i]["Cost"],
                //        Date = (DateTime)dataTable.Rows[i]["Date"]
                //    };
                //    costMasters.Add(costMaster);
                //}

                //Approach: 2 = With For Each Loop
                foreach (DataRow row in dataTable.Rows)
                {
                    costMasters.Add(new CostMaster
                    {
                        Id = (int)row["Id"],
                        ServiceTypeId = (int)row["ServiceTypeId"],
                        Name = (string)row["Name"],
                        Cost = (decimal)row["Cost"],
                        Date = (DateTime)row["Date"],
                        SiteId = row["SiteId"] != DBNull.Value ? (int?)row["SiteId"] : null
                    });
                }
                return costMasters;
            }
        }

        public CostMaster GetActiveCostDetail(int serviceTypeId, int siteId)
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                string sqlQuery = @"
                       SELECT TOP 1 
                           CostMaster.ServiceTypeId, ServiceTypes.Name, 
                           CostMaster.Cost, CostMaster.Date, CostMaster.SiteId
                       FROM 
                           CostMaster 
                       JOIN 
                           ServiceTypes ON CostMaster.ServiceTypeId = ServiceTypes.Id 
                       WHERE 
                           CostMaster.ServiceTypeId = @serviceTypeId 
                         AND 
                           CostMaster.SiteId = @siteId
                         AND 
                           CostMaster.Date <= @currentDate 
                       ORDER BY 
                           CostMaster.Date DESC";

                SqlCommand sqlCommand = new(sqlQuery, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@serviceTypeId", serviceTypeId);
                sqlCommand.Parameters.AddWithValue("@siteId", siteId);
                sqlCommand.Parameters.AddWithValue("@currentDate", DateTime.Now);

                sqlConnection.Open();
                SqlDataReader reader = sqlCommand.ExecuteReader();

                CostMaster costMaster = null;
                if (reader.Read())
                {
                    costMaster = new CostMaster
                    {
                        ServiceTypeId = (int)reader["ServiceTypeId"],
                        Name = (string)reader["Name"],
                        Cost = (decimal)reader["Cost"],
                        SiteId = reader["SiteId"] != DBNull.Value ? (int?)reader["SiteId"] : null
                    };
                }

                reader.Close();
                return costMaster;
            }
        }

        public CostMaster GetById(int id, int siteId)
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                string sqlQuery = @"
                       SELECT 
                            CostMaster.Id, CostMaster.ServiceTypeId, 
                            ServiceTypes.Name, CostMaster.Cost, CostMaster.Date, CostMaster.SiteId
                       FROM 
                           CostMaster
                       JOIN 
                           ServiceTypes ON CostMaster.ServiceTypeId = ServiceTypes.Id
                       WHERE 
                           CostMaster.Id = @id
                         AND 
                           CostMaster.SiteId = @siteId";

                SqlCommand sqlCommand = new(sqlQuery, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@id", id);
                sqlCommand.Parameters.AddWithValue("@siteId", siteId);

                sqlConnection.Open();
                SqlDataReader reader = sqlCommand.ExecuteReader();

                CostMaster costMaster = null;
                if (reader.Read())
                {
                    costMaster = new CostMaster
                    {
                        Id = (int)reader["Id"],
                        ServiceTypeId = (int)reader["ServiceTypeId"],
                        Name = (string)reader["Name"],
                        Cost = (decimal)reader["Cost"],
                        Date = (DateTime)reader["Date"],
                        SiteId = reader["SiteId"] != DBNull.Value ? (int?)reader["SiteId"] : null
                    };
                }

                reader.Close();
                return costMaster;
            }
        }

        public int Create(CostMaster costMaster)
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                string insertQuery = @"
                       INSERT INTO CostMaster (ServiceTypeId, Cost, Date, SiteId)
                       VALUES (@serviceTypeId, @cost, @date, @siteId)";

                SqlCommand insertCommand = new(insertQuery, sqlConnection);
                insertCommand.Parameters.AddWithValue("@serviceTypeId", costMaster.ServiceTypeId);
                insertCommand.Parameters.AddWithValue("@cost", costMaster.Cost);
                insertCommand.Parameters.AddWithValue("@date", costMaster.Date);
                insertCommand.Parameters.AddWithValue("@siteId",
                    costMaster.SiteId.HasValue ? costMaster.SiteId : DBNull.Value);

                sqlConnection.Open();
                int affectedRowCount = insertCommand.ExecuteNonQuery();
                sqlConnection.Close();
                return affectedRowCount;
            }
        }

        public int Update(CostMaster costMaster)
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                string updateQuery = @"
                       UPDATE CostMaster SET
                           ServiceTypeId = @serviceTypeId,
                           Cost = @cost,
                           Date = @date
                       WHERE Id = @id
                         AND SiteId = @siteId";

                SqlCommand updateCommand = new(updateQuery, sqlConnection);
                updateCommand.Parameters.AddWithValue("@id", costMaster.Id);
                updateCommand.Parameters.AddWithValue("@serviceTypeId", costMaster.ServiceTypeId);
                updateCommand.Parameters.AddWithValue("@cost", costMaster.Cost);
                updateCommand.Parameters.AddWithValue("@date", costMaster.Date);
                updateCommand.Parameters.AddWithValue("@siteId",
                    costMaster.SiteId.HasValue ? costMaster.SiteId : DBNull.Value);

                sqlConnection.Open();
                int affectedRowCount = updateCommand.ExecuteNonQuery();
                sqlConnection.Close();
                return affectedRowCount;
            }
        }

        public void Delete(int id, int siteId)
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                string deleteQuery = @"DELETE FROM CostMaster WHERE Id = @id AND SiteId = @siteId";

                SqlCommand deleteCommand = new(deleteQuery, sqlConnection);
                deleteCommand.Parameters.AddWithValue("@id", id);
                deleteCommand.Parameters.AddWithValue("@siteId", siteId);

                sqlConnection.Open();
                deleteCommand.ExecuteNonQuery();
                sqlConnection.Close();
            }
        }
    }
}
