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

        public List<Product> GetAll()
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
    }
}
