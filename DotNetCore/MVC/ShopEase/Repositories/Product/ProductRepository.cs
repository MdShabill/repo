﻿using ShopEase.DataModels.Product;
using System.Data;
using System.Data.SqlClient;

namespace ShopEase.Repositories.Product
{
    public class ProductRepository : IProductRepository
    {
        private readonly string _connectionString;

        public ProductRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<Products> GetAll()
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                string sqlQuery = @"
                        SELECT 
                            Products.Id, Products.ProductName, Brands.BrandName, Products.Price, 
                            Products.Discount, Categories.CategoryName, Suppliers.SupplierName 
                        FROM Products
                        Inner Join Brands On Products.BrandId = Brands.Id
                        Inner Join Categories On Products.CategoryId = Categories.Id
                        Inner Join Suppliers On Products.SupplierId = Suppliers.Id
                        ";
                SqlDataAdapter sqlDataAdapter = new(sqlQuery, sqlConnection);
                DataTable dataTable = new();
                sqlDataAdapter.Fill(dataTable);

                List<Products> products = new();

                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    Products product = new()
                    {
                        Id = (int)dataTable.Rows[i]["Id"],
                        ProductName = (string)dataTable.Rows[i]["ProductName"],
                        BrandName = (string)dataTable.Rows[i]["BrandName"],
                        Discount = Convert.ToInt32(dataTable.Rows[i]["Discount"]),
                        Price = Convert.ToInt32(dataTable.Rows[i]["Price"]),
                        CategoryName = (string)dataTable.Rows[i]["CategoryName"],
                        SupplierName = (string)dataTable.Rows[i]["SupplierName"],
                    };
                    products.Add(product);
                }
                return products;
            }
        }

        public List<Products> GetSortedProducts(string? sortColumnName, string? sortOrder)
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                if (string.IsNullOrEmpty(sortColumnName))
                    sortColumnName = "ProductName";

                if (string.IsNullOrEmpty(sortOrder))
                    sortOrder = "ASC";

                string sqlQuery = @"SELECT Products.Id,
                         Products.ProductName, Brands.BrandName,
                         Products.Price, Products.Discount,
                         Categories.CategoryName, Suppliers.SupplierName 
                         FROM Products
                         Inner Join Brands On Products.BrandId = Brands.Id                
                         Inner Join Categories On Products.CategoryId = Categories.Id                    
                         Inner Join Suppliers On Products.SupplierId = Suppliers.Id ";

                if (!string.IsNullOrEmpty(sortColumnName) && !string.IsNullOrEmpty(sortOrder))
                {
                    switch (sortColumnName)
                    {
                        case "ProductName":
                            sqlQuery += $" ORDER BY ProductName {sortOrder}";
                            break;
                        case "BrandName":
                            sqlQuery += $" ORDER BY BrandName {sortOrder}";
                            break;
                        case "Price":
                            sqlQuery += $" ORDER BY Price {sortOrder}";
                            break;
                        case "Discount":
                            sqlQuery += $" ORDER BY Discount {sortOrder}";
                            break;
                        case "CategoryName":
                            sqlQuery += $" ORDER BY CategoryName {sortOrder}";
                            break;
                        case "SupplierName":
                            sqlQuery += $" ORDER BY SupplierName {sortOrder}";
                            break;
                        default:
                            sqlQuery += $" ORDER BY ProductName {sortOrder}";
                            break;
                    }
                }

                SqlDataAdapter sqlDataAdapter = new(sqlQuery, sqlConnection);
                DataTable dataTable = new();
                sqlDataAdapter.Fill(dataTable);

                List<Products> products = new();

                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    Products product = new()
                    {
                        Id = (int)dataTable.Rows[i]["Id"],
                        ProductName = (string)dataTable.Rows[i]["ProductName"],
                        BrandName = (string)dataTable.Rows[i]["BrandName"],
                        Discount = Convert.ToInt32(dataTable.Rows[i]["Discount"]),
                        Price = Convert.ToInt32(dataTable.Rows[i]["Price"]),
                        CategoryName = (string)dataTable.Rows[i]["CategoryName"],
                        SupplierName = (string)dataTable.Rows[i]["SupplierName"],
                    };
                    products.Add(product);
                }
                return products;
            }
        }

        public int Add(ProductAdd productAdd)
        {
            using(SqlConnection sqlConnection = new(_connectionString))
            {
                string sqlQuery = @"Insert Into Products
                       (ProductName, BrandId, Price, Discount, CategoryId, SupplierId)
                       Values
                       (@ProductName, @BrandId, @Price, @Discount, @CategoryId, @SupplierId)";

                SqlCommand sqlCommand = new(sqlQuery, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@ProductName", productAdd.ProductName);
                sqlCommand.Parameters.AddWithValue("@BrandId", productAdd.BrandId);
                sqlCommand.Parameters.AddWithValue("@Price", productAdd.Price);
                sqlCommand.Parameters.AddWithValue("@Discount", productAdd.Discount);
                sqlCommand.Parameters.AddWithValue("@CategoryId", productAdd.CategoryId);
                sqlCommand.Parameters.AddWithValue("@SupplierId", productAdd.SupplierId);

                sqlConnection.Open();
                int affectedRowCount = sqlCommand.ExecuteNonQuery();
                sqlConnection.Close();

                return affectedRowCount;
            }
        }
    }
}