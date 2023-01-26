using Microsoft.Data.SqlClient;
using System.Data;
using WebApiDemo1.DTO.InputDTO;

namespace WebApiDemo1.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly string _connectionString;

        public ProductRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public DataTable GetAllProducts()
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                SqlDataAdapter sqlDataAdapter = new("SELECT * FROM Products", sqlConnection);
                DataTable dataTable = new();
                sqlDataAdapter.Fill(dataTable);
                return dataTable;
            }
        }

        public int GetProductsCount()
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                string sqlQuery = "SELECT COUNT(*) FROM Products";
                SqlCommand sqlCommand = new(sqlQuery, sqlConnection);
                sqlConnection.Open();
                int productCount = Convert.ToInt32(sqlCommand.ExecuteScalar());
                sqlConnection.Close();
                return productCount;
            }          
        }

        public string GetProductDetailByBaradNameById(int productId)
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                string sqlQuery = "Select BrandName From Products where id = @productId";
                SqlCommand sqlCommand = new(sqlQuery, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@productId", productId);
                sqlConnection.Open();
                string brandName = Convert.ToString(sqlCommand.ExecuteScalar());
                sqlConnection.Close();
                return brandName;
            }
        }

        public DataTable GetProductsDetailByBrandNameByProductName(string brandName, string? productName)
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                string sqlQuery = "SELECT * FROM Products WHERE BrandName = @brandName ";

                if (!string.IsNullOrWhiteSpace(productName))
                    sqlQuery += "AND ProductName = @productName ";

                SqlDataAdapter sqlDataAdapter = new(sqlQuery, sqlConnection);
                sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@brandName", brandName);
                sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@productName", productName);
                DataTable dataTable = new();
                sqlDataAdapter.Fill(dataTable);
                return dataTable;
            }
        }

        public DataTable GetProductsDetailByBrandNameByPriceUpto(string brandName, int priceUpto)
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                SqlDataAdapter sqlDataAdapter = new(@"SELECT * FROM Products WHERE BrandName = @brandName AND
                           Price <= @priceUpto", sqlConnection);
                sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@brandName", brandName);
                sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@priceUpto", priceUpto);
                DataTable dataTable = new();
                sqlDataAdapter.Fill(dataTable);
                return dataTable;
            }
        }

        public DataTable GetProductsByPriceRange(int minimumPrice, int maximumPrice)
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                SqlDataAdapter sqlDataAdapter = new(@" SELECT * FROM Products 
                           WHERE Price BETWEEN @minimumPrice AND @maximumPrice
                           ORDER BY Price", sqlConnection);
                sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@minimumPrice", minimumPrice);
                sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@maximumPrice", maximumPrice);
                DataTable dataTable = new();
                sqlDataAdapter.Fill(dataTable);
                return dataTable;
            }
        }

        public int ProductAdd(ProductDto product)
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                string sqlQuery = @"INSERT INTO Products(ProductName, BrandName, Size, Color, Fit, 
                            Fabric, Category, Discount, Price)
                            VALUES (@ProductName, @BrandName, @Size, @Color, @Fit, @Fabric, 
                            @Category, @Discount, @Price)
                            Select Scope_Identity() ";
                SqlCommand sqlCommand = new(sqlQuery, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@ProductName", product.ProductName);
                sqlCommand.Parameters.AddWithValue("@BrandName", product.BrandName);
                sqlCommand.Parameters.AddWithValue("@Size", product.Size);
                sqlCommand.Parameters.AddWithValue("@Color", product.Color);
                sqlCommand.Parameters.AddWithValue("@Fit", product.Fit);
                sqlCommand.Parameters.AddWithValue("@Fabric", product.Fabric);
                sqlCommand.Parameters.AddWithValue("@Category", product.Category);
                sqlCommand.Parameters.AddWithValue("@Discount", product.Discount);
                sqlCommand.Parameters.AddWithValue("@Price", product.Price);
                sqlConnection.Open();
                product.Id = Convert.ToInt32(sqlCommand.ExecuteScalar());
                sqlConnection.Close();
                return product.Id;
            }
        }

        public void Update(ProductDto product)
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                string sqlQuery = @" UPDATE Products Set ProductName = @ProductName, BrandName = @BrandName,
                    Size = @Size, Color = @Color, Fit = @Fit, Fabric = @Fabric, Category = @Category,
                    Discount = @Discount, Price = @Price
                    WHERE Id = @Id ";
                SqlCommand sqlCommand = new(sqlQuery, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@Id", product.Id);
                sqlCommand.Parameters.AddWithValue("@ProductName", product.ProductName);
                sqlCommand.Parameters.AddWithValue("@BrandName", product.BrandName);
                sqlCommand.Parameters.AddWithValue("@Size", product.Size);
                sqlCommand.Parameters.AddWithValue("@Color", product.Color);
                sqlCommand.Parameters.AddWithValue("@Fit", product.Fit);
                sqlCommand.Parameters.AddWithValue("@Fabric", product.Fabric);
                sqlCommand.Parameters.AddWithValue("@Category", product.Category);
                sqlCommand.Parameters.AddWithValue("@Discount", product.Discount);
                sqlCommand.Parameters.AddWithValue("@Price", product.Price);
                sqlConnection.Open();
                sqlCommand.ExecuteNonQuery();
                sqlConnection.Close();
            }
        }
    }
}
