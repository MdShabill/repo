using ShopEase.DataModels.Product;
using System.Data.SqlClient;
using System.Data;

namespace ShopEase.Repositories
{
    public class SPProductRepository : ISPProductRepository
    {
        private readonly string _connectionString;

        public SPProductRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public Product SP_GetProduct(int id)
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                string sqlQuery = @"EXEC SP_GetProductById @id";

                SqlDataAdapter sqlDataAdapter = new(sqlQuery, sqlConnection);
                sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@id", id);
                DataTable dataTable = new();
                sqlDataAdapter.Fill(dataTable);

                Product product = new()
                {
                    Id = (int)dataTable.Rows[0]["Id"],
                    Title = (string)dataTable.Rows[0]["Title"],
                    CategoryName = (string)dataTable.Rows[0]["CategoryName"],
                    BrandId = (int)dataTable.Rows[0]["BrandId"],
                    BrandName = (string)dataTable.Rows[0]["BrandName"],
                    Price = Convert.ToInt32(dataTable.Rows[0]["Price"]),
                    ActualPrice = Convert.ToInt32(dataTable.Rows[0]["ActualPrice"]),
                    Discount = Convert.ToInt32(dataTable.Rows[0]["Discount"]),
                    SupplierName = (string)dataTable.Rows[0]["SupplierName"],
                    Quantity = (int)dataTable.Rows[0]["Quantity"],
                    ImageName = (string)dataTable.Rows[0]["ImageName"],
                };
                return product;
            }
        } 
    }
}
