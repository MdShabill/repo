using ConstructionApplication.Core.DataModels.CostMaster;
using ConstructionApplication.Core.DataModels.JobCategory;
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

        public List<CostMaster> GetByJobCategory(int jobCategoryId)
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                string sqlQuery = @"SELECT CostMaster.Id, CostMaster.JobCategoryId, 
                            JobCategories.Name, CostMaster.Cost, CostMaster.Date
                            FROM CostMaster
                            Join JobCategories ON CostMaster.JobCategoryId = JobCategories.Id
                            WHERE CostMaster.JobCategoryId = @jobCategoryId
                            ORDER BY CostMaster.Date DESC";

                SqlDataAdapter sqlDataAdapter = new(sqlQuery, sqlConnection);
                sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@jobCategoryId", jobCategoryId);
                DataTable dataTable = new();
                sqlDataAdapter.Fill(dataTable);

                List<CostMaster> costMasters = new();
                //Approach: 1 = With For Loop
                //for (int i = 0; i < dataTable.Rows.Count; i++)
                //{
                //    CostMaster costMaster = new()
                //    {
                //        Id = (int)dataTable.Rows[i]["Id"],
                //        JobCategoryId = (int)dataTable.Rows[i]["JobCategoryId"],
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
                        JobCategoryId = (int)row["JobCategoryId"],
                        Name = (string)row["Name"],
                        Cost = (decimal)row["Cost"],
                        Date = (DateTime)row["Date"]
                    };
                    costMasters.Add(costMaster);
                }
                return costMasters;
            }
        }

        public CostMaster GetActiveCostDetail(int JobCategoryId)
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                string sqlQuery = @"
                        Select Top 1 
                        CostMaster.JobCategoryId, JobCategories.Name, 
                        CostMaster.Cost, CostMaster.Date
                        From CostMaster 
                        Join JobCategories ON CostMaster.JobCategoryId = JobCategories.Id 
                        Where CostMaster.JobCategoryId = @jobCategoryId 
                        And CostMaster.Date <= @currentDate 
                        ORDER BY CostMaster.Date DESC";

                SqlDataAdapter sqlDataAdapter = new(sqlQuery, sqlConnection);
                sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@jobCategoryId", JobCategoryId);
                sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@currentDate", DateTime.Now);
                DataTable dataTable = new();
                sqlDataAdapter.Fill(dataTable);

                if (dataTable.Rows.Count > 0)
                {
                    CostMaster costMaster = new()
                    {
                        JobCategoryId = (int)dataTable.Rows[0]["JobCategoryId"],
                        Name = (string)dataTable.Rows[0]["Name"],
                        Cost = (decimal)dataTable.Rows[0]["Cost"],
                    };
                    return costMaster;
                }
                return null;
            }
        }

        public int Create(CostMaster costMaster)
        {
            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                string insertQuery = @"INSERT INTO CostMaster
                                        (JobCategoryId, Cost, Date)
                                        VALUES
                                        (@jobCategoryId, @cost, @date)";
                SqlCommand insertCommand = new SqlCommand(insertQuery, sqlConnection);
                insertCommand.Parameters.AddWithValue("@jobCategoryId", costMaster.JobCategoryId);
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
