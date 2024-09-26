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
                string sqlQuery = @"SELECT Id, MasterMasonCost, LabourCost, Date, IsActive 
                                    FROM CostMaster
                                    Order By Date DESC";
                SqlDataAdapter sqlDataAdapter = new(sqlQuery, sqlConnection);
                DataTable dataTable = new();
                sqlDataAdapter.Fill(dataTable);

                List<CostMaster> costMasters = new();

                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    CostMaster costMaster = new()
                    {
                        Id = (int)dataTable.Rows[i]["Id"],
                        MasterMasonCost = (decimal)dataTable.Rows[i]["MasterMasonCost"],
                        LabourCost = (decimal)dataTable.Rows[i]["LabourCost"],
                        Date = (DateTime)dataTable.Rows[i]["Date"],
                        IsActive = dataTable.Rows[i]["IsActive"] != DBNull.Value && (bool)dataTable.Rows[i]["IsActive"]
                    };
                    costMasters.Add(costMaster);
                }
                return costMasters;
            }
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
