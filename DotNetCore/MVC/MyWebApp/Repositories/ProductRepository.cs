using MyWebApp.Models;
using System.Data;
using Microsoft.Data.SqlClient;

namespace MyWebApp.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly string _connectionString;

        public ProductRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        
        public List<Product> Index()
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                string sqlQuery = @"
                        SELECT 
                            Products.Id, Products.ProductName, Products.BrandName, Products.Fit, 
                            Products.Fabric, Products.Category, Products.Discount,Products.Price, 
                            ProductSizes.Size, ProductColors.ColorName 
                        FROM Products
                        Inner Join ProductSizes On Products.SizeId = ProductSizes.Id
                        Inner Join ProductColors On Products.ColorId = ProductColors.Id
                        ";
                SqlDataAdapter sqlDataAdapter = new(sqlQuery, sqlConnection);
                DataTable dataTable = new();
                sqlDataAdapter.Fill(dataTable);

                List<Product> products = new();

                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    Product product = new()
                    {
                        Id = (int)dataTable.Rows[i]["Id"],
                        ProductName = (string)dataTable.Rows[i]["ProductName"],
                        BrandName = (string)dataTable.Rows[i]["BrandName"],
                        SizeName = (string)dataTable.Rows[i]["Size"],
                        ColorName = (string)dataTable.Rows[i]["ColorName"],
                        Fit = (string)dataTable.Rows[i]["Fit"],
                        Fabric = (string)dataTable.Rows[i]["Fabric"],
                        Category = (string)dataTable.Rows[i]["Category"],
                        Discount = (int)dataTable.Rows[i]["Discount"],
                        Price = (int)dataTable.Rows[i]["Price"],
                    };
                    products.Add(product);
                }
                return products;
            }
        }

        public Product Get(int id)
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                string sqlQuery = $@"
                        SELECT 
                            Products.Id, Products.ProductName, Products.BrandName, Products.Fit, 
                            Products.Fabric, Products.Category, Products.Discount,Products.Price, 
                            ProductSizes.Size, ProductColors.ColorName 
                        FROM Products
                        Inner Join ProductSizes On Products.SizeId = ProductSizes.Id
                        Inner Join ProductColors On Products.ColorId = ProductColors.Id
                        Where 
                            Products.Id = @id";
                SqlDataAdapter sqlDataAdapter = new(sqlQuery, sqlConnection);
                sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@id", id);
                DataTable dataTable = new();
                sqlDataAdapter.Fill(dataTable);
        
                Product product = new()
                {
                    Id = (int)dataTable.Rows[0]["Id"],
                    ProductName = (string)dataTable.Rows[0]["ProductName"],
                    BrandName = (string)dataTable.Rows[0]["BrandName"],
                    SizeName = (string)dataTable.Rows[0]["Size"],
                    ColorName = (string)dataTable.Rows[0]["ColorName"],
                    Fit = (string)dataTable.Rows[0]["Fit"],
                    Fabric = (string)dataTable.Rows[0]["Fabric"],
                    Category = (string)dataTable.Rows[0]["Category"],
                    Discount = (int)dataTable.Rows[0]["Discount"],
                    Price = (int)dataTable.Rows[0]["Price"],
                };
                return product;
            }
        }

        public List<ProductSizes> GetSizesDetails()
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                string sqlQuery1 = "SELECT Id, Size FROM ProductSizes";
                SqlDataAdapter sqlDataAdapter1 = new(sqlQuery1, sqlConnection);
                DataTable dataTable1 = new();
                sqlDataAdapter1.Fill(dataTable1);

                List<ProductSizes> productSizes = new();

                for (int i = 0; i < dataTable1.Rows.Count; i++)
                {
                    ProductSizes productSize = new()
                    {
                        Id = (int)dataTable1.Rows[i]["Id"],
                        Size = (string)dataTable1.Rows[i]["Size"]
                    };
                    productSizes.Add(productSize);
                }
                return productSizes;
            }
        }

        List<ProductColor> IProductRepository.GetcolorDetails()
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                string sqlQuery = "SELECT Id, ColorName FROM ProductColors";
                SqlDataAdapter sqlDataAdapter = new(sqlQuery, sqlConnection);
                DataTable dataTable = new();
                sqlDataAdapter.Fill(dataTable);

                List<ProductColor> productColors = new();

                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    ProductColor productColor = new()
                    {
                        Id = (int)dataTable.Rows[i]["Id"],
                        ColorName = (string)dataTable.Rows[i]["ColorName"]
                    };
                    productColors.Add(productColor);
                }
                return productColors;
            }
        }

        public void Delete(int id)
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                string deleteQuery = "Delete From Products Where Id = @Id";
                SqlCommand sqlCommand = new(deleteQuery, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@Id", id);
                sqlConnection.Open();
                sqlCommand.ExecuteNonQuery();
                sqlConnection.Close();
            }
        }

        public void Add(Product product)
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                string sqlQuery = @"INSERT INTO Products
                    (ProductName, BrandName, SizeId, ColorId, 
                     Fit, Fabric, Category, Discount, Price)
                     VALUES 
                    (@ProductName, @BrandName, @SizeId, @ColorId, 
                     @Fit, @Fabric, @Category, @Discount, @Price) ";
                SqlCommand sqlCommand = new(sqlQuery, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@ProductName", product.ProductName);
                sqlCommand.Parameters.AddWithValue("@BrandName", product.BrandName);
                sqlCommand.Parameters.AddWithValue("@SizeId", product.SizeId);
                sqlCommand.Parameters.AddWithValue("@ColorId", product.ColorId);
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

        public void Update(Product product)
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                string sqlQuery = @" UPDATE Products Set 
                   ProductName = @ProductName, BrandName = @BrandName,
                   SizeId = @SizeId, ColorId = @ColorId, Fit = @Fit, 
                   Fabric = @Fabric, Category = @Category,
                   Discount = @Discount, Price = @Price
                   WHERE Id = @Id ";
                SqlCommand sqlCommand = new(sqlQuery, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@Id", product.Id);
                sqlCommand.Parameters.AddWithValue("@ProductName", product.ProductName);
                sqlCommand.Parameters.AddWithValue("@BrandName", product.BrandName);
                sqlCommand.Parameters.AddWithValue("@SizeId", product.SizeId);
                sqlCommand.Parameters.AddWithValue("@ColorId", product.ColorId);
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
