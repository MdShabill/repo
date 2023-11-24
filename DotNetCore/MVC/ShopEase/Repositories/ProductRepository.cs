using ShopEase.DataModels;
using ShopEase.ViewModels;
using System.Data;
using System.Data.SqlClient;

namespace ShopEase.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly string _connectionString;

        public ProductRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<Product> GetSortedProducts(string? sortColumnName, string? sortOrder)
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                if (string.IsNullOrEmpty(sortColumnName))
                {
                    sortColumnName = "ProductName";
                }

                if (string.IsNullOrEmpty(sortOrder))
                {
                    sortOrder = "ASC";
                }

                string sqlQuery = @"SELECT Products.Id,
                         Products.ProductName, Brands.BrandName,
                         Products.Price, Products.Discount,
                         (Products.Price - Products.Discount) AS ActualPrice,
                         Categories.CategoryName, Suppliers.SupplierName 
                         FROM Products
                         Inner Join Brands On Products.BrandId = Brands.Id                
                         Inner Join Categories On Products.CategoryId = Categories.Id                    
                         Inner Join Suppliers On Products.SupplierId = Suppliers.Id ";

                if (!string.IsNullOrEmpty(sortColumnName) && !string.IsNullOrEmpty(sortOrder))
                {
                    sqlQuery += " ORDER BY " + @sortColumnName + " " + @sortOrder;
                }

                SqlDataAdapter sqlDataAdapter = new(sqlQuery, sqlConnection);

                sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@sortColumnName", sortColumnName);
                sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@sortOrder", sortOrder);

                DataTable dataTable = new();
                sqlDataAdapter.Fill(dataTable);

                List<Product> products = new();

                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    Product product = new()
                    {
                        Id = (int)dataTable.Rows[i]["Id"],
                        ProductName = (string)dataTable.Rows[i]["ProductName"],
                        CategoryName = (string)dataTable.Rows[i]["CategoryName"],
                        BrandName = (string)dataTable.Rows[i]["BrandName"],
                        Price = Convert.ToInt32(dataTable.Rows[i]["Price"]),
                        ActualPrice = Convert.ToInt32(dataTable.Rows[i]["ActualPrice"]),
                        Discount = Convert.ToInt32(dataTable.Rows[i]["Discount"]),
                        SupplierName = (string)dataTable.Rows[i]["SupplierName"],
                    };
                    products.Add(product);
                }
                return products;
            }
        }

        public Product GetProduct(int id)
        {
            using(SqlConnection sqlConnection = new (_connectionString))
            {
                string sqlQuery = @"SELECT Products.Id,
                         Products.ProductName, Categories.CategoryName,
                         Products.BrandId, Brands.BrandName, Products.Price, Products.Discount,
                         (Products.Price - Products.Discount) AS ActualPrice,
                         Suppliers.SupplierName, Products.ImageName
                         FROM Products
                         Inner Join Brands On Products.BrandId = Brands.Id                
                         Inner Join Categories On Products.CategoryId = Categories.Id                    
                         Inner Join Suppliers On Products.SupplierId = Suppliers.Id  
                         Where Products.Id = @id ";

                SqlDataAdapter sqlDataAdapter = new(sqlQuery, sqlConnection);
                sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@id", id);
                DataTable dataTable = new();
                sqlDataAdapter.Fill(dataTable);
                
                Product product = new()
                {
                    Id = (int)dataTable.Rows[0]["Id"],
                    ProductName = (string)dataTable.Rows[0]["ProductName"],
                    CategoryName = (string)dataTable.Rows[0]["CategoryName"],
                    BrandId = (int)dataTable.Rows[0]["BrandId"],
                    BrandName = (string)dataTable.Rows[0]["BrandName"],
                    Price = Convert.ToInt32(dataTable.Rows[0]["Price"]),
                    ActualPrice = Convert.ToInt32(dataTable.Rows[0]["ActualPrice"]),
                    Discount = Convert.ToInt32(dataTable.Rows[0]["Discount"]),
                    SupplierName = (string)dataTable.Rows[0]["SupplierName"],
                    ImageName = (string)dataTable.Rows[0]["ImageName"],
                };
                return product;
            }
        }

        public List<ProductSearchResult> GetProductsResult(ProductFilter productFilters)
        {
            using(SqlConnection sqlConnection = new(_connectionString))
            {
                string sqlQuery = @"Select Products.Id, 
                        Products.ProductName, Brands.BrandName, Products.Price,
                        (Products.Price - Products.Discount) AS ActualPrice,
                        Categories.CategoryName
                        From Products 
                        Inner Join Brands On Products.BrandId = Brands.Id                
                        Inner Join Categories On Products.CategoryId = Categories.Id
                        Where 1=1 ";

                SqlDataAdapter sqlDataAdapter = new(sqlQuery, sqlConnection);

                if (!string.IsNullOrEmpty(productFilters.ProductName))
                {
                    sqlQuery += " And Products.ProductName Like '%' + @productName + '%' ";
                    sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@productName", productFilters.ProductName);
                }

                if (productFilters.BrandId != 0)
                {
                    sqlQuery += " And Products.BrandId = @brandId ";
                    sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@brandId", productFilters.BrandId);
                }

                if (productFilters.Min != 0)
                {
                    sqlQuery += " And Products.Price >= @minPrice ";
                    sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@minPrice", productFilters.Min);
                }

                if (productFilters.Max != 0)
                {
                    sqlQuery += " And Products.Price <= @maxPrice ";
                    sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@maxPrice", productFilters.Max);
                }

                if (productFilters.CategoryId != 0)
                {
                    sqlQuery += " And Products.CategoryId = @categoryId ";
                    sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@categoryId", productFilters.CategoryId);
                }

                sqlDataAdapter.SelectCommand.CommandText = sqlQuery;

                DataTable dataTable = new();
                sqlDataAdapter.Fill(dataTable);

                List<ProductSearchResult> productSearchResults = new();

                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    ProductSearchResult productSearchResult = new()
                    {
                        Id = (int)dataTable.Rows[i]["Id"],  
                        ProductName = (string)dataTable.Rows[i]["ProductName"],
                        CategoryName = (string)dataTable.Rows[i]["CategoryName"],
                        BrandName = (string)dataTable.Rows[i]["BrandName"],
                        Price = (decimal)dataTable.Rows[i]["Price"],
                        ActualPrice = (decimal)dataTable.Rows[i]["ActualPrice"],
                    };
                    productSearchResults.Add(productSearchResult);
                }
                return productSearchResults;
            }
        }

        public int Add(ProductAdd productAdd)
        {
            using(SqlConnection sqlConnection = new(_connectionString))
            {
                string sqlQuery = @"Insert Into Products
                       (ProductName, BrandId, Price, Discount, CategoryId, SupplierId, ImageName)
                       Values
                       (@ProductName, @BrandId, @Price, @Discount, @CategoryId, @SupplierId, @imageName)";

                SqlCommand sqlCommand = new(sqlQuery, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@ProductName", productAdd.ProductName);
                sqlCommand.Parameters.AddWithValue("@BrandId", productAdd.BrandId);
                sqlCommand.Parameters.AddWithValue("@Price", productAdd.Price);
                sqlCommand.Parameters.AddWithValue("@Discount", productAdd.Discount);
                sqlCommand.Parameters.AddWithValue("@CategoryId", productAdd.CategoryId);
                sqlCommand.Parameters.AddWithValue("@SupplierId", productAdd.SupplierId);
                sqlCommand.Parameters.AddWithValue("@imageName", productAdd.ImageName);

                sqlConnection.Open();
                int affectedRowCount = sqlCommand.ExecuteNonQuery();
                sqlConnection.Close();

                return affectedRowCount;
            }
        }
    }
}
