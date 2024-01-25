using ShopEase.DataModels.Product;
using System.Data.SqlClient;
using System.Data;

namespace ShopEase.Repositories
{
    public class ProductRepositoryUsingSP : IProductRepositoryUsingSP
    {
        private readonly string _connectionString;

        public ProductRepositoryUsingSP(string connectionString)
        {
            _connectionString = connectionString;
        }

        public Product GetProduct(int id)
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                string sqlQuery = @"EXEC SP_GetProductResult @id";

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

        public List<ProductSearchResult> GetProductsResult(ProductFilter productFilters)
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                string sqlQuery = @"EXEC SP_GetProductResult @title, @brandId, @minPrice, @maxPrice, @categoryId ";

                SqlDataAdapter sqlDataAdapter = new(sqlQuery, sqlConnection);
                sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@title", (object)productFilters.Title ?? DBNull.Value);
                sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@brandId", (object)productFilters.BrandId ?? DBNull.Value);
                sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@minPrice", (object)productFilters.Min ?? DBNull.Value);
                sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@maxPrice", (object)productFilters.Max ?? DBNull.Value);
                sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@categoryId", (object)productFilters.CategoryId ?? DBNull.Value);

                DataTable dataTable = new();
                sqlDataAdapter.Fill(dataTable);

                List<ProductSearchResult> productSearchResults = new();

                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    ProductSearchResult productSearchResult = new()
                    {
                        Id = (int)dataTable.Rows[i]["Id"],
                        Title = (string)dataTable.Rows[i]["Title"],
                        ImageName = (string)dataTable.Rows[i]["ImageName"],
                        CategoryName = (string)dataTable.Rows[i]["CategoryName"],
                        BrandId = (int)dataTable.Rows[i]["BrandId"],
                        BrandName = (string)dataTable.Rows[i]["BrandName"],
                        Price = (decimal)dataTable.Rows[i]["Price"],
                        ActualPrice = (decimal)dataTable.Rows[i]["ActualPrice"],
                        Quantity = (int)dataTable.Rows[i]["Quantity"]
                    };
                    productSearchResults.Add(productSearchResult);
                }
                return productSearchResults;
            }
        }
    }
}
