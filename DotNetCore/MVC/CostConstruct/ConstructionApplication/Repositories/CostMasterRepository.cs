using ConstructionApplication.DataModels.CostMaster;
using ConstructionApplication.DataModels.JobCategory;
using ConstructionApplication.DataModels.Material;
using System.Data;
using System.Data.SqlClient;

namespace ConstructionApplication.Repositories
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
                string sqlQuery = @"SELECT CostMaster.Id, CostMaster.JobCategoryId, JobCategories.Name, 
                            CostMaster.Cost, CostMaster.Date, CostMaster.IsActive
                            FROM CostMaster
                            Join JobCategories ON CostMaster.JobCategoryId = JobCategories.Id
                            WHERE CostMaster.JobCategoryId = @jobCategoryId
                            ORDER BY CostMaster.Date DESC";

                SqlDataAdapter sqlDataAdapter = new(sqlQuery, sqlConnection);
                sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@jobCategoryId", jobCategoryId);
                DataTable dataTable = new();
                sqlDataAdapter.Fill(dataTable);

                List<CostMaster> costMasters = new();
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    CostMaster costMaster = new()
                    {
                        Id = (int)dataTable.Rows[i]["Id"],
                        JobCategoryId = (int)dataTable.Rows[i]["JobCategoryId"],
                        Name = (string)dataTable.Rows[i]["Name"],
                        Cost = (decimal)dataTable.Rows[i]["Cost"],
                        Date = (DateTime)dataTable.Rows[i]["Date"],
                        IsActive = dataTable.Rows[i]["IsActive"] != DBNull.Value && (bool)dataTable.Rows[i]["IsActive"]
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
                        Select CostMaster.JobCategoryId, JobCategories.Name, CostMaster.Cost
                        From CostMaster 
                        Join JobCategories ON CostMaster.JobCategoryId = JobCategories.Id 
                        Where CostMaster.IsActive = 1 And CostMaster.JobCategoryId = @jobCategoryId ";
                SqlDataAdapter sqlDataAdapter = new(sqlQuery, sqlConnection);
                sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@jobCategoryId", JobCategoryId);
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
                sqlConnection.Open();
                using (SqlTransaction transaction = sqlConnection.BeginTransaction())
                {
                    try
                    {
                        string updateQuery = @"UPDATE CostMaster 
                                       SET IsActive = 0 
                                       WHERE JobCategoryId = @jobCategoryId 
                                       AND IsActive = 1";
                        SqlCommand updateCommand = new SqlCommand(updateQuery, sqlConnection, transaction);
                        updateCommand.Parameters.AddWithValue("@jobCategoryId", costMaster.JobCategoryId);
                        updateCommand.ExecuteNonQuery();

                        string insertQuery = @"INSERT INTO CostMaster
                                       (JobCategoryId, Cost, Date, IsActive)
                                       VALUES
                                       (@jobCategoryId, @cost, @date, 1)";
                        SqlCommand insertCommand = new SqlCommand(insertQuery, sqlConnection, transaction);
                        insertCommand.Parameters.AddWithValue("@jobCategoryId", costMaster.JobCategoryId);
                        insertCommand.Parameters.AddWithValue("@cost", costMaster.Cost);
                        insertCommand.Parameters.AddWithValue("@date", costMaster.Date);
                        int affectedRowCount = insertCommand.ExecuteNonQuery();

                        transaction.Commit();
                        return affectedRowCount;
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                    finally
                    {
                        sqlConnection.Close();
                    }
                }
            }
        }

    }
}
