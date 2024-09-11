using ConstructionApplication.DataModels.CostMaster;
using ConstructionApplication.DataModels.MaterialPurchase;
using System.Data.SqlClient;

namespace ConstructionApplication.Repositories
{
    public class MaterialPurchaseRepository : IMaterialPurchaseRepository
    {
        private readonly string _connectionString;

        public MaterialPurchaseRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public int Create(MaterialPurchase materialPurchase)
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                string sqlQuery = @"Insert Into MaterialPurchase
                       (MaterialId, Quantity, UnitOfMeasure, Date, MaterialCost, DeliveryCharge)
                       Values
                       (@materialId, @quantity, @unitOfMeasure, @date, @materialCost, @deliveryCharge) ";

                SqlCommand sqlCommand = new(sqlQuery, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@materialId", materialPurchase.MaterialId);
                sqlCommand.Parameters.AddWithValue("@quantity", materialPurchase.Quantity);
                sqlCommand.Parameters.AddWithValue("@unitOfMeasure", materialPurchase.UnitOfMeasure);
                sqlCommand.Parameters.AddWithValue("@date", materialPurchase.Date);
                sqlCommand.Parameters.AddWithValue("@materialCost", materialPurchase.MaterialCost);
                sqlCommand.Parameters.AddWithValue("@deliveryCharge", materialPurchase.DeliveryCharge);

                sqlConnection.Open();
                int affectedRowCount = sqlCommand.ExecuteNonQuery();
                sqlConnection.Close();

                return affectedRowCount;
            }
        }
    }
}
