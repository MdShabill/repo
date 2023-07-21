using System.Data;
using Microsoft.Data.SqlClient;
using MyWebApp.Enums;
using MyWebApp.DataModel;
using MyWebApp.ViewModels.Products;

namespace MyWebApp.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly string _connectionString;

        public ProductRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<Product> GetAll()
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                string sqlQuery = @"
                        SELECT 
                            Products.Id, Products.ProductName, Products.BrandName, Products.Fit, 
                            Productfabrics.FabricName, ProductCategories.CategoryName, Products.Discount, Products.Price, 
                            ProductSizes.Size, ProductColors.ColorName 
                        FROM Products
                        Inner Join ProductSizes On Products.SizeId = ProductSizes.Id
                        Inner Join ProductColors On Products.ColorId = ProductColors.Id
                        Inner Join ProductFabrics On Products.FabricId = ProductFabrics.Id
                        Inner Join ProductCategories On Products.CategoryId = ProductCategories.Id
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
                        Fit = (FitType)dataTable.Rows[i]["Fit"],
                        FabricName = (string)dataTable.Rows[i]["FabricName"],
                        CategoryName = (string)dataTable.Rows[i]["CategoryName"],
                        Discount = (int)dataTable.Rows[i]["Discount"],
                        Price = (int)dataTable.Rows[i]["Price"],
                    };
                    products.Add(product);
                }
                return products;
            }
        }

        public ProductVm Get(int id)
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                string sqlQuery = $@"
                        SELECT 
                            Products.Id, Products.ProductName, Products.BrandName, Products.Fit, 
                            Productfabrics.FabricName, ProductCategories.CategoryName, Products.Discount, 
                            Products.Price, ProductSizes.Size, ProductColors.ColorName 
                        FROM Products
                        Inner Join ProductSizes On Products.SizeId = ProductSizes.Id
                        Inner Join ProductColors On Products.ColorId = ProductColors.Id
                        Inner Join ProductFabrics On Products.FabricId = ProductFabrics.Id
                        Inner Join ProductCategories On Products.CategoryId = ProductCategories.Id
                        Where 
                            Products.Id = @id";
                SqlDataAdapter sqlDataAdapter = new(sqlQuery, sqlConnection);
                sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@id", id);
                DataTable dataTable = new();
                sqlDataAdapter.Fill(dataTable);
        
                ProductVm product = new()
                {
                    Id = (int)dataTable.Rows[0]["Id"],
                    ProductName = (string)dataTable.Rows[0]["ProductName"],
                    BrandName = (string)dataTable.Rows[0]["BrandName"],
                    SizeName = (string)dataTable.Rows[0]["Size"],
                    ColorName = (string)dataTable.Rows[0]["ColorName"],
                    Fit = (FitType)dataTable.Rows[0]["Fit"],
                    FabricName = (string)dataTable.Rows[0]["FabricName"],
                    CategoryName = (string)dataTable.Rows[0]["CategoryName"],
                    Discount = (int)dataTable.Rows[0]["Discount"],
                    Price = (int)dataTable.Rows[0]["Price"],
                };
                return product;
            }
        }

        public List<ProductVm> GetProducts(string productName, int colorId, int sizeId, int minPrice, int maxPrice)
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                string sqlQuery = @"
                        Select 
                        Products.Id, Products.ProductName, 
                        Products.BrandName, ProductColors.ColorName,
                        ProductSizes.Size
                        From Products 
                        Inner Join ProductColors On Products.ColorId = ProductColors.Id
                        Inner Join ProductSizes On Products.SizeId = ProductSizes.Id
                        Where 
                        Products.ProductName LIKE '%' + @productName + '%' And 
                        Products.ColorId = @myColorId And 
                        Products.SizeId = @sizeId And
                        Products.Price Between @minPrice And @maxPrice ";

                SqlDataAdapter sqlDataAdapter = new(sqlQuery, sqlConnection);
                
                sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@productName", productName);
                sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@myColorId", colorId);
                sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@sizeId", sizeId);
                sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@minPrice", minPrice);
                sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@maxPrice", maxPrice);
                
                DataTable dataTable = new();
                sqlDataAdapter.Fill(dataTable);

                List<ProductVm> products = new();

                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    ProductVm product = new()
                    {
                        Id = (int)dataTable.Rows[i]["Id"],
                        ProductName = (string)dataTable.Rows[i]["ProductName"],
                        BrandName = (string)dataTable.Rows[i]["BrandName"],
                        ColorName = (string)dataTable.Rows[i]["ColorName"],
                        SizeName = (string)dataTable.Rows[i]["Size"]
                    };
                    products.Add(product);
                }
                return products;
            }
        }

        public List<ProductSizesVm> GetSizes()
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                string sqlQuery1 = "SELECT Id, Size FROM ProductSizes";
                SqlDataAdapter sqlDataAdapter1 = new(sqlQuery1, sqlConnection);
                DataTable dataTable1 = new();
                sqlDataAdapter1.Fill(dataTable1);

                List<ProductSizesVm> productSizes = new();

                for (int i = 0; i < dataTable1.Rows.Count; i++)
                {
                    ProductSizesVm productSize = new()
                    {
                        Id = (int)dataTable1.Rows[i]["Id"],
                        Size = (string)dataTable1.Rows[i]["Size"]
                    };
                    productSizes.Add(productSize);
                }
                return productSizes;
            }
        }

        public List<ProductColorVm> GetColors()
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                string sqlQuery = "SELECT Id, ColorName FROM ProductColors";
                SqlDataAdapter sqlDataAdapter = new(sqlQuery, sqlConnection);
                DataTable dataTable = new();
                sqlDataAdapter.Fill(dataTable);

                List<ProductColorVm> productColors = new();

                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    ProductColorVm productColor = new()
                    {
                        Id = (int)dataTable.Rows[i]["Id"],
                        ColorName = (string)dataTable.Rows[i]["ColorName"]
                    };
                    productColors.Add(productColor);
                }
                return productColors;
            }
        }

        public List<ProductFabricVm> GetFabric()
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                string sqlQuery = "SELECT Id, FabricName FROM ProductFabrics";
                SqlDataAdapter sqlDataAdapter = new(sqlQuery, sqlConnection);
                DataTable dataTable = new();
                sqlDataAdapter.Fill(dataTable);

                List<ProductFabricVm> productFabrics = new();

                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    ProductFabricVm productFabric = new()
                    {
                        Id = (int)dataTable.Rows[i]["Id"],
                        FabricName = (string)dataTable.Rows[i]["FabricName"]
                    };
                    productFabrics.Add(productFabric);
                }
                return productFabrics;
            }
        }

        public List<ProductCategoryVm> GetCategory()
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                string sqlQuery = "SELECT Id, CategoryName FROM ProductCategories";
                SqlDataAdapter sqlDataAdapter = new(sqlQuery, sqlConnection);
                DataTable dataTable = new();
                sqlDataAdapter.Fill(dataTable);

                List<ProductCategoryVm> ProductCategories = new();

                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    ProductCategoryVm ProductCategory = new()
                    {
                        Id = (int)dataTable.Rows[i]["Id"],
                        CategoryName = (string)dataTable.Rows[i]["CategoryName"]
                    };
                    ProductCategories.Add(ProductCategory);
                }
                return ProductCategories;
            }
        }

        public int Delete(int id)
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                string deleteQuery = "Delete From Products Where Id = @Id";
                SqlCommand sqlCommand = new(deleteQuery, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@Id", id);
                sqlConnection.Open();
                int affectedRowCount = sqlCommand.ExecuteNonQuery();
                sqlConnection.Close();
                return affectedRowCount;
            }
        }

        public int Add(Product product)
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                string sqlQuery = @"INSERT INTO Products
                    (ProductName, BrandName, SizeId, ColorId, 
                     Fit, FabricId, CategoryId, Discount, Price)
                     VALUES 
                    (@ProductName, @BrandName, @SizeId, @ColorId, 
                     @Fit, @FabricId, @CategoryId, @Discount, @Price) ";
                SqlCommand sqlCommand = new(sqlQuery, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@ProductName", product.ProductName);
                sqlCommand.Parameters.AddWithValue("@BrandName", product.BrandName);
                sqlCommand.Parameters.AddWithValue("@SizeId", product.SizeId);
                sqlCommand.Parameters.AddWithValue("@ColorId", product.ColorId);
                sqlCommand.Parameters.AddWithValue("@Fit", product.Fit);
                sqlCommand.Parameters.AddWithValue("@FabricId", product.FabricId);
                sqlCommand.Parameters.AddWithValue("@CategoryId", product.CategoryId);
                sqlCommand.Parameters.AddWithValue("@Discount", product.Discount);
                sqlCommand.Parameters.AddWithValue("@Price", product.Price);
                sqlConnection.Open();
                int affectedRowCount = sqlCommand.ExecuteNonQuery();
                sqlConnection.Close();
                return affectedRowCount;
            }
        }

        public int Update(Product product)
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                string sqlQuery = @" UPDATE Products Set 
                   ProductName = @ProductName, BrandName = @BrandName,
                   SizeId = @SizeId, ColorId = @ColorId, Fit = @Fit, 
                   FabricId = @FabricId, CategoryId = @CategoryId,
                   Discount = @Discount, Price = @Price
                   WHERE Id = @Id ";
                SqlCommand sqlCommand = new(sqlQuery, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@Id", product.Id);
                sqlCommand.Parameters.AddWithValue("@ProductName", product.ProductName);
                sqlCommand.Parameters.AddWithValue("@BrandName", product.BrandName);
                sqlCommand.Parameters.AddWithValue("@SizeId", product.SizeId);
                sqlCommand.Parameters.AddWithValue("@ColorId", product.ColorId);
                sqlCommand.Parameters.AddWithValue("@Fit", product.Fit);
                sqlCommand.Parameters.AddWithValue("@FabricId", product.FabricId);
                sqlCommand.Parameters.AddWithValue("@CategoryId", product.CategoryId);
                sqlCommand.Parameters.AddWithValue("@Discount", product.Discount);
                sqlCommand.Parameters.AddWithValue("@Price", product.Price);
                sqlConnection.Open();
                int affectedRowCount = sqlCommand.ExecuteNonQuery();
                sqlConnection.Close();
                return affectedRowCount;
            }
        }

    }
}
