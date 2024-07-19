using ConstructionApplication.DataModels.CostMaster;
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
