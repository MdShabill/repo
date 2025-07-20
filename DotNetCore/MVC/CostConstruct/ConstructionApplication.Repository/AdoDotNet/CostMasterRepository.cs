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

        public List<CostMaster> GetByServiceType(int serviceTypeId)
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                string sqlQuery = @"SELECT CostMaster.Id, CostMaster.ServiceTypeId, 
                            ServiceTypes.Name, CostMaster.Cost, CostMaster.Date
                            FROM CostMaster
                            Join ServiceTypes ON CostMaster.ServiceTypeId = ServiceTypes.Id
                            WHERE CostMaster.ServiceTypeId = @serviceTypeId
                            ORDER BY CostMaster.Date DESC";

                SqlDataAdapter sqlDataAdapter = new(sqlQuery, sqlConnection);
                sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@serviceTypeId", serviceTypeId);
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
                    CostMaster costMaster = new()
                    {
                        Id = (int)row["ID"],
                        ServiceTypeId = (int)row["ServiceTypeId"],
                        Name = (string)row["Name"],
                        Cost = (decimal)row["Cost"],
                        Date = (DateTime)row["Date"]
                    };
                    costMasters.Add(costMaster);
                }
                return costMasters;
            }
        }

        public CostMaster GetActiveCostDetail(int serviceTypeId)
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                string sqlQuery = @"
                        SELECT TOP 1 
                            CostMaster.ServiceTypeId, ServiceTypes.Name, 
                            CostMaster.Cost, CostMaster.Date
                        FROM CostMaster 
                        JOIN ServiceTypes ON CostMaster.ServiceTypeId = ServiceTypes.Id 
                        WHERE CostMaster.ServiceTypeId = @serviceTypeId 
                          AND CostMaster.Date <= @currentDate 
                        ORDER BY CostMaster.Date DESC";

                SqlCommand sqlCommand = new(sqlQuery, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@serviceTypeId", serviceTypeId);
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
                    };
                }

                reader.Close();
                return costMaster;
            }
        }


        public int Create(CostMaster costMaster)
        {
            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                string insertQuery = @"INSERT INTO CostMaster
                                        (ServiceTypeId, Cost, Date)
                                        VALUES
                                        (@serviceTypeId, @cost, @date)";
                SqlCommand insertCommand = new SqlCommand(insertQuery, sqlConnection);
                insertCommand.Parameters.AddWithValue("@serviceTypeId", costMaster.ServiceTypeId);
                insertCommand.Parameters.AddWithValue("@cost", costMaster.Cost);
                insertCommand.Parameters.AddWithValue("@date", costMaster.Date);
                sqlConnection.Open();
                int affectedRowCount = insertCommand.ExecuteNonQuery();
                sqlConnection.Close();

                return affectedRowCount;
            }
        }
    }
}
