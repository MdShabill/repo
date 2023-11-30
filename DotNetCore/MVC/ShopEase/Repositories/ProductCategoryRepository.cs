using System.Data.SqlClient;
using System.Data;
using ShopEase.DataModels.Product;

namespace ShopEase.Repositories
{
    public class ProductCategoryRepository : IProductCategoryReopsitory
    {
        private readonly string _connectionString;

        public ProductCategoryRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<ProductCategory> GetCategories()
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                string sqlQuery = "Select Id, CategoryName From Categories";
                SqlDataAdapter sqlDataAdapter = new(sqlQuery, sqlConnection);
                DataTable dataTable = new();
                sqlDataAdapter.Fill(dataTable);

                List<ProductCategory> productCategories = new();

                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    ProductCategory productCategory = new()
                    {
                        Id = (int)dataTable.Rows[i]["Id"],
                        CategoryName = (string)dataTable.Rows[i]["CategoryName"]
                    };
                    productCategories.Add(productCategory);
                }
                return productCategories;
            }
        }
    }
}
