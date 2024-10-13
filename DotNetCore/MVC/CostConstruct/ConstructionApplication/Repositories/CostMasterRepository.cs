using ConstructionApplication.DataModels.CostMaster;
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

        public List<CostMaster> GetAll()
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                string sqlQuery = @"SELECT CostMaster.Id, JobCategories.Name, 
                                    CostMaster.Cost, CostMaster.Date, CostMaster.IsActive
                                    FROM CostMaster
                                    Join JobCategories ON CostMaster.JobCategoryId = JobCategories.Id
                                    Order By CostMaster.Date DESC";
                SqlDataAdapter sqlDataAdapter = new(sqlQuery, sqlConnection);
                DataTable dataTable = new();
                sqlDataAdapter.Fill(dataTable);

                List<CostMaster> costMasters = new();

                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    CostMaster costMaster = new()
                    {
                        Id = (int)dataTable.Rows[i]["Id"],
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
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                string sqlQuery = @"Insert Into CostMaster
                                 (JobCategoryId, Cost, Date)
                                 Values
                                 (@jobCategoryId, @cost, @date) ";

                SqlCommand sqlCommand = new(sqlQuery, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@jobCategoryId", costMaster.JobCategoryId);
                sqlCommand.Parameters.AddWithValue("@cost", costMaster.Cost);
                sqlCommand.Parameters.AddWithValue("@date", costMaster.Date);

                sqlConnection.Open();
                int affectedRowCount = sqlCommand.ExecuteNonQuery();
                sqlConnection.Close();

                return affectedRowCount;
            }
        }
    }
}
