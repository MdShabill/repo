using ConstructionApplication.DataModels.CostMaster;
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

        public CostMaster GetActiveCostDetail()
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                string sqlQuery = @"
                        Select MasterMasonCost, LabourCost  From CostMaster Where IsActive = 1 ";
                SqlDataAdapter sqlDataAdapter = new(sqlQuery, sqlConnection);
                DataTable dataTable = new();
                sqlDataAdapter.Fill(dataTable);

                if (dataTable.Rows.Count > 0)
                {
                    CostMaster costMaster = new()
                    {
                        MasterMasonCost = (decimal)dataTable.Rows[0]["MasterMasonCost"],
                        LabourCost = (decimal)dataTable.Rows[0]["LabourCost"],
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
                       (MasterMasonCost, LabourCost, Date)
                       Values
                       (@masterMasonCost, @labourCost, @date) ";

                SqlCommand sqlCommand = new(sqlQuery, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@masterMasonCost", costMaster.MasterMasonCost);
                sqlCommand.Parameters.AddWithValue("@labourCost", costMaster.LabourCost);
                sqlCommand.Parameters.AddWithValue("@date", costMaster.Date);

                sqlConnection.Open();
                int affectedRowCount = sqlCommand.ExecuteNonQuery();
                sqlConnection.Close();

                return affectedRowCount;
            }
        }
    }
}
