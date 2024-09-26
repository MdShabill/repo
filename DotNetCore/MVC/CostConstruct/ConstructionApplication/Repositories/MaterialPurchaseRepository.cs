using ConstructionApplication.DataModels.CostMaster;
using ConstructionApplication.DataModels.MaterialPurchase;
using System.Data;
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

        public List<MaterialPurchase> GetAll()
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                string sqlQuery = @"SELECT MaterialPurchase.Id, 
                       Materials.Name As MaterialName, Suppliers.Name As SupplirName, 
                       Brands.Name As BrandName, MaterialPurchase.Quantity, 
                       MaterialPurchase.UnitOfMeasure, Materials.UnitPrice, MaterialPurchase.Date,
                       MaterialPurchase.MaterialCost, MaterialPurchase.DeliveryCharge
                       FROM MaterialPurchase
                       Inner Join Materials On MaterialPurchase.MaterialId = Materials.Id                
                       Inner Join Suppliers On MaterialPurchase.SupplirId = Suppliers.Id                    
                       Inner Join Brands On MaterialPurchase.BrandId = Brands.Id 
                       Order By MaterialPurchase.Date DESC";

                SqlDataAdapter sqlDataAdapter = new(sqlQuery, sqlConnection);
                DataTable dataTable = new();
                sqlDataAdapter.Fill(dataTable);

                List<MaterialPurchase> materialPurchases = new();

                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    MaterialPurchase materialPurchase = new()
                    {
                        Id = (int)dataTable.Rows[i]["Id"],
                        MaterialName = (string)dataTable.Rows[i]["MaterialName"],
                        SupplirName = (string)dataTable.Rows[i]["SupplirName"],
                        BrandName = (string)dataTable.Rows[i]["BrandName"],
                        Quantity = (int)dataTable.Rows[i]["Quantity"],
                        UnitOfMeasure = (string)dataTable.Rows[i]["UnitOfMeasure"],
                        UnitPrice = (decimal)dataTable.Rows[i]["UnitPrice"],
                        Date = (DateTime)dataTable.Rows[i]["Date"],
                        MaterialCost = (decimal)dataTable.Rows[i]["MaterialCost"],
                        DeliveryCharge = (decimal)dataTable.Rows[i]["DeliveryCharge"],
                    };
                    materialPurchases.Add(materialPurchase);
                }
                return materialPurchases;
            }
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
