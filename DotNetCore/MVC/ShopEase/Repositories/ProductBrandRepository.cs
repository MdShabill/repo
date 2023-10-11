using ShopEase.DataModels.Product;
using System.Data.SqlClient;
using System.Data;

namespace ShopEase.Repositories
{
    public class ProductBrandRepository : IProductBrandRepository
    {
        private readonly string _connectionString;
        
        public ProductBrandRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<ProductBrand> GetBrands()
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                string sqlQuery = "Select Id, BrandName From Brands";
                SqlDataAdapter sqlDataAdapter = new(sqlQuery, sqlConnection);
                DataTable dataTable = new();
                sqlDataAdapter.Fill(dataTable);

                List<ProductBrand> productBrands = new();

                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    ProductBrand productBrand = new()
                    {
                        Id = (int)dataTable.Rows[i]["Id"],
                        BrandName = (string)dataTable.Rows[i]["BrandName"]
                    };
                    productBrands.Add(productBrand);
                }
                return productBrands;
            }
        }
    }
}
