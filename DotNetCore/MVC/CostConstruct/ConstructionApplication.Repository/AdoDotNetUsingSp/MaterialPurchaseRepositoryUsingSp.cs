using ConstructionApplication.Core.DataModels.MaterialPurchase;
using System.Data.SqlClient;
using System.Data;
using ConstructionApplication.Repository.Interfaces;

namespace ConstructionApplication.Repository.AdoDotNetUsingSp
{
    public class MaterialPurchaseRepositoryUsingSp : IMaterialPurchaseRepository
    {
        private readonly string _connectionString;

        public MaterialPurchaseRepositoryUsingSp(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<MaterialPurchase> GetAll(int siteId, DateTime? DateFrom, DateTime? DateTo, int? MaterialId, int? SupplierId, int? BrandId)
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                SqlCommand sqlCommand = new("Sp_MaterialPurchaseCRUD", sqlConnection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                sqlCommand.Parameters.AddWithValue("@Mode", "Get_All");
                sqlCommand.Parameters.AddWithValue("@DateFrom", (object)DateFrom ?? DBNull.Value);
                sqlCommand.Parameters.AddWithValue("@DateTo", (object)DateTo ?? DBNull.Value);
                sqlCommand.Parameters.AddWithValue("@MaterialId", (object)MaterialId ?? DBNull.Value);
                sqlCommand.Parameters.AddWithValue("@SupplierId", (object)SupplierId ?? DBNull.Value);
                sqlCommand.Parameters.AddWithValue("@BrandId", (object)BrandId ?? DBNull.Value);

                SqlDataAdapter sqlDataAdapter = new(sqlCommand);
                DataTable dataTable = new();
                sqlDataAdapter.Fill(dataTable);

                List<MaterialPurchase> materialPurchases = new();

                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    MaterialPurchase materialPurchase = new()
                    {
                        Id = (int)dataTable.Rows[i]["Id"],
                        MaterialName = (string)dataTable.Rows[i]["MaterialName"],
                        SupplierName = (string)dataTable.Rows[i]["SupplierName"],
                        PhoneNumber = (string)dataTable.Rows[i]["PhoneNumber"],
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
                SqlCommand sqlCommand = new("Sp_MaterialPurchaseCRUD", sqlConnection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                sqlCommand.Parameters.AddWithValue("@Mode", "Create");
                sqlCommand.Parameters.AddWithValue("@materialId", materialPurchase.MaterialId);
                sqlCommand.Parameters.AddWithValue("@supplierId", materialPurchase.SupplierId);
                sqlCommand.Parameters.AddWithValue("@phoneNumber", materialPurchase.PhoneNumber);
                sqlCommand.Parameters.AddWithValue("@brandId", materialPurchase.BrandId);
                sqlCommand.Parameters.AddWithValue("@quantity", materialPurchase.Quantity);
                sqlCommand.Parameters.AddWithValue("@unitOfMeasure", materialPurchase.UnitOfMeasure);
                sqlCommand.Parameters.AddWithValue("@date", materialPurchase.Date);
                sqlCommand.Parameters.AddWithValue("@materialCost", materialPurchase.MaterialCost);
                sqlCommand.Parameters.AddWithValue("@deliveryCharge", materialPurchase.DeliveryCharge);

                sqlConnection.Open();
                object result = sqlCommand.ExecuteScalar();
                sqlConnection.Close();
                int affectedRowCount = result != null ? Convert.ToInt32(result) : 0;

                return affectedRowCount;
            }
        }
    }
}
