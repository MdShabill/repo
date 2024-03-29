﻿using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using WebApiDemo1.DataModel;
using WebApiDemo1.DTO.InputDTO;
using WebApiDemo1.DTO.OutPutDTO;
using WebApiDemo1.Enums;

namespace WebApiDemo1.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly string _connectionString;

        public ProductRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<ProductInputDto> GetAllProductAsList()
        {
            List<ProductInputDto> products = new();

            using (SqlConnection sqlConnection = new(_connectionString))
            {
                SqlDataAdapter sqlDataAdapter = new("SELECT * FROM Products", sqlConnection);
                DataTable dataTable = new();
                sqlDataAdapter.Fill(dataTable);

                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    ProductInputDto productDto = new();
                    productDto.ProductName = (string)dataTable.Rows[i]["ProductName"];
                    productDto.BrandName = (string)dataTable.Rows[i]["BrandName"];
                    productDto.Size = (int)dataTable.Rows[i]["Size"];
                    productDto.Color = (ColorType)dataTable.Rows[i]["Color"];
                    productDto.Fit = (string)dataTable.Rows[i]["Fit"];
                    productDto.Fabric = (string)dataTable.Rows[i]["Fabric"];
                    productDto.Category = (string)dataTable.Rows[i]["Category"];
                    productDto.Discount = (int)dataTable.Rows[i]["Discount"];
                    productDto.Price = (int)dataTable.Rows[i]["Price"];
                    products.Add(productDto);
                }
                return products;
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

        public string GetProductDetailById(int productId)
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

        public List<ProductInputDto> GetProductsDetailByBrandNameByProductName(string brandName, string? productName)
        {
            List<ProductInputDto> products = new();

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

                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    ProductInputDto productDto = new();
                    productDto.ProductName = (string)dataTable.Rows[i]["ProductName"];
                    productDto.BrandName = (string)dataTable.Rows[i]["BrandName"];
                    productDto.Size = (int)dataTable.Rows[i]["Size"];
                    productDto.Color = (ColorType)dataTable.Rows[i]["Color"];
                    productDto.Fit = (string)dataTable.Rows[i]["Fit"];
                    productDto.Fabric = (string)dataTable.Rows[i]["Fabric"];
                    productDto.Category = (string)dataTable.Rows[i]["Category"];
                    productDto.Discount = (int)dataTable.Rows[i]["Discount"];
                    productDto.Price = (int)dataTable.Rows[i]["Price"];
                    products.Add(productDto);
                }
                return products;
            }
        }

        public List<ProductInputDto> GetProductsDetailByBrandNameByPrice(string brandName, int price)
        {
            List<ProductInputDto> products = new();

            using (SqlConnection sqlConnection = new(_connectionString))
            {
                SqlDataAdapter sqlDataAdapter = new(@"SELECT * FROM Products WHERE BrandName = @brandName AND
                           Price <= @price", sqlConnection);
                sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@brandName", brandName);
                sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@price", price);
                DataTable dataTable = new();
                sqlDataAdapter.Fill(dataTable);

                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    ProductInputDto productDto = new();
                    productDto.ProductName = (string)dataTable.Rows[i]["ProductName"];
                    productDto.BrandName = (string)dataTable.Rows[i]["BrandName"];
                    productDto.Size = (int)dataTable.Rows[i]["Size"];
                    productDto.Color = (ColorType)dataTable.Rows[i]["Color"];
                    productDto.Fit = (string)dataTable.Rows[i]["Fit"];
                    productDto.Fabric = (string)dataTable.Rows[i]["Fabric"];
                    productDto.Category = (string)dataTable.Rows[i]["Category"];
                    productDto.Discount = (int)dataTable.Rows[i]["Discount"];
                    productDto.Price = (int)dataTable.Rows[i]["Price"];
                    products.Add(productDto);
                }
                return products;
            }
        }

        public List<ProductInputDto> GetProductsByPriceRange(int minimumPrice, int maximumPrice)
        {
            List<ProductInputDto> products = new();

            using (SqlConnection sqlConnection = new(_connectionString))
            {
                SqlDataAdapter sqlDataAdapter = new(@" SELECT * FROM Products 
                           WHERE Price BETWEEN @minimumPrice AND @maximumPrice
                           ORDER BY Price", sqlConnection);
                sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@minimumPrice", minimumPrice);
                sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@maximumPrice", maximumPrice);
                DataTable dataTable = new();
                sqlDataAdapter.Fill(dataTable);
                
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    ProductInputDto productDto = new();
                    productDto.ProductName = (string)dataTable.Rows[i]["ProductName"];
                    productDto.BrandName = (string)dataTable.Rows[i]["BrandName"];
                    productDto.Size = (int)dataTable.Rows[i]["Size"];
                    productDto.Color = (ColorType)dataTable.Rows[i]["Color"];
                    productDto.Fit = (string)dataTable.Rows[i]["Fit"];
                    productDto.Fabric = (string)dataTable.Rows[i]["Fabric"];
                    productDto.Category = (string)dataTable.Rows[i]["Category"];
                    productDto.Discount = (int)dataTable.Rows[i]["Discount"];
                    productDto.Price = (int)dataTable.Rows[i]["Price"];
                    products.Add(productDto);
                }
                return products;
            }
        }

        public List<ProductOutputDto> GetFilteredProducts_1(ProductInputDto productInputDto)
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                string sqlQuery = @"
                    Select * From Products 
                    Where 
                        ProductName = @productName 
                        And BrandName = @brandName 
                        And Size = @size 
                        And Color = @color 
                        And Fit = @fit 
                        And Fabric = @fabric
                        And Category = @category 
                        And Discount = @discount 
                        And Price = @price";

                SqlDataAdapter sqlDataAdapter = new(sqlQuery, sqlConnection);
                sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@productName", productInputDto.ProductName);
                sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@brandName", productInputDto.BrandName);
                sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@size", productInputDto.Size);
                sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@color", productInputDto.Color);
                sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@fit", productInputDto.Fit);
                sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@fabric", productInputDto.Fabric);
                sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@category", productInputDto.Category);
                sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@discount", productInputDto.Discount);
                sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@price", productInputDto.Price);
                DataTable dataTable = new();
                sqlDataAdapter.Fill(dataTable);

                List<ProductOutputDto> results = new();

                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    ProductOutputDto productOutputDto = new()
                    {
                        ProductName = (string)dataTable.Rows[i]["ProductName"],
                        BrandName = (string)dataTable.Rows[i]["BrandName"],
                        Size = (int)dataTable.Rows[i]["Size"],
                        Color = (ColorType)dataTable.Rows[i]["Color"],
                        Fit = (string)dataTable.Rows[i]["Fit"],
                        Fabric = (string)dataTable.Rows[i]["Fabric"],
                        Category = (string)dataTable.Rows[i]["Category"],
                        Discount = (int)dataTable.Rows[i]["Discount"],
                        Price = (int)dataTable.Rows[i]["Price"],
                    };
                    results.Add(productOutputDto);
                }
                return results;
            }
        }

        public List<Product> GetFilteredProducts_2(Product product)
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                string sqlQuery = @"Select * From Products Where 1 = 1 ";

                if (!string.IsNullOrWhiteSpace(product.ProductName))
                    sqlQuery += "And ProductName = @productName ";

                if (!string.IsNullOrWhiteSpace(product.BrandName))
                    sqlQuery += " And BrandName = @brandName ";

                if (product.Size != 0)
                    sqlQuery += " And Size = @size ";

                if (product.Color != 0)
                    sqlQuery += " And Color = @color ";

                if (!string.IsNullOrWhiteSpace(product.Fit))
                    sqlQuery += " And Fit = @fit ";

                if (!string.IsNullOrWhiteSpace(product.Fabric))
                    sqlQuery += " And Fabric = @fabric ";

                if (!string.IsNullOrWhiteSpace(product.Category))
                    sqlQuery += " And Category = @category ";

                if (product.Discount != 0)
                    sqlQuery += " And Discount = @discount ";

                if (product.Price != 0)
                    sqlQuery += " And Price = @price ";

                SqlDataAdapter sqlDataAdapter = new(sqlQuery, sqlConnection);

                if(!string.IsNullOrWhiteSpace(product.ProductName))
                    sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@productName", product.ProductName);

                if (!string.IsNullOrWhiteSpace(product.BrandName))
                    sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@brandName", product.BrandName);

                sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@size", product.Size);

                sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@color", product.Color);

                if(!string.IsNullOrWhiteSpace(product.Fit))
                    sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@fit", product.Fit);

                if(!string.IsNullOrWhiteSpace(product.Fabric))
                    sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@fabric", product.Fabric);

                if(!string.IsNullOrWhiteSpace(product.Category))
                    sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@category", product.Category);

                sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@discount", product.Discount);

                sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@price", product.Price);

                DataTable dataTable = new();
                sqlDataAdapter.Fill(dataTable);

                List<Product> Results = new();

                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    Product productDataModel = new()
                    {
                        Id = (int)dataTable.Rows[i]["Id"],
                        ProductName = (string)dataTable.Rows[i]["ProductName"],
                        BrandName = (string)dataTable.Rows[i]["BrandName"],
                        Size = (int)dataTable.Rows[i]["Size"],
                        Color = (ColorType)dataTable.Rows[i]["Color"],
                        Fit = (string)dataTable.Rows[i]["Fit"],
                        Fabric = (string)dataTable.Rows[i]["Fabric"],
                        Category = (string)dataTable.Rows[i]["Category"],
                        Discount = (int)dataTable.Rows[i]["Discount"],
                        Price = (int)dataTable.Rows[i]["Price"],
                    };
                    Results.Add(productDataModel);
                }
                return Results;
            }
        }

        public List<Product> GetFilteredProducts_3(Product product)
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                string sqlQuery = "SELECT * FROM Products WHERE 1 = 1 ";

                SqlCommand sqlCommand = new();

                if (!string.IsNullOrWhiteSpace(product.ProductName))
                {
                    sqlQuery += " And ProductName = @productName ";
                    sqlCommand.Parameters.AddWithValue("@productName", product.ProductName);
                }

                if (!string.IsNullOrWhiteSpace(product.BrandName))
                {
                    sqlQuery += " And BrandName = @brandName ";
                    sqlCommand.Parameters.AddWithValue("@brandName", product.BrandName);
                }
                    
                if (product.Size != 0)
                {
                    sqlQuery += " And Size = @size ";
                    sqlCommand.Parameters.AddWithValue("@size", product.Size);
                }
                    
                if (product.Color != 0)
                {
                    sqlQuery += " And Color = @color ";
                    sqlCommand.Parameters.AddWithValue("@color", product.Color);
                }

                if (!string.IsNullOrWhiteSpace(product.Fit))
                {
                    sqlQuery += " And Fit = @fit ";
                    sqlCommand.Parameters.AddWithValue("@fit", product.Fit);
                }
                    
                if (!string.IsNullOrWhiteSpace(product.Fabric))
                {
                    sqlQuery += " And Fabric = @fabric ";
                    sqlCommand.Parameters.AddWithValue("@fabric", product.Fabric);
                }

                if (!string.IsNullOrWhiteSpace(product.Category))
                {
                    sqlQuery += " And Category = @category ";
                    sqlCommand.Parameters.AddWithValue("@category", product.Category);
                }

                if (product.Discount != 0)
                {
                    sqlQuery += " And Discount = @discount ";
                    sqlCommand.Parameters.AddWithValue("@discount", product.Discount);
                }

                if (product.Price != 0)
                {
                    sqlQuery += " And Price = @price ";
                    sqlCommand.Parameters.AddWithValue("@price", product.Price);
                }

                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = sqlQuery;

                SqlDataAdapter adapter = new();
                adapter.SelectCommand = sqlCommand;

                DataTable dataTable = new();
                adapter.Fill(dataTable);

                List<Product> Results = new();

                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    Product productDataModel = new()
                    {
                        Id = (int)dataTable.Rows[i]["Id"],
                        ProductName = (string)dataTable.Rows[i]["ProductName"],
                        BrandName = (string)dataTable.Rows[i]["BrandName"],
                        Size = (int)dataTable.Rows[i]["Size"],
                        Color = (ColorType)dataTable.Rows[i]["Color"],
                        Fit = (string)dataTable.Rows[i]["Fit"],
                        Fabric = (string)dataTable.Rows[i]["Fabric"],
                        Category = (string)dataTable.Rows[i]["Category"],
                        Discount = (int)dataTable.Rows[i]["Discount"],
                        Price = (int)dataTable.Rows[i]["Price"],
                    };
                    Results.Add(productDataModel);
                }
                return Results;
            }
        }

        public List<Product> GetFilteredProducts(Product product)
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                string sqlQuery = @"
                        Select * From Products 
                        Where ProductName = @productName 
                        And BrandName = @brandName 
                        And Size = @size 
                        And Color = @color 
                        And Fit = @fit 
                        And Fabric = @fabric
                        And Category = @category 
                        And Discount = @discount 
                        And Price = @price";

                SqlDataAdapter sqlDataAdapter = new(sqlQuery, sqlConnection);
                sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@productName", product.ProductName);
                sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@brandName", product.BrandName);
                sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@size", product.Size);
                sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@color", product.Color);
                sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@fit", product.Fit);
                sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@fabric", product.Fabric);
                sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@category", product.Category);
                sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@discount", product.Discount);
                sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@price", product.Price);
                DataTable dataTable = new();
                sqlDataAdapter.Fill(dataTable);

                List<Product> Results = new();

                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    Product productDataModel = new()
                    {
                        Id = (int)dataTable.Rows[i]["Id"],
                        ProductName = (string)dataTable.Rows[i]["ProductName"],
                        BrandName = (string)dataTable.Rows[i]["BrandName"],
                        Size = (int)dataTable.Rows[i]["Size"],
                        Color = (ColorType)dataTable.Rows[i]["Color"],
                        Fit = (string)dataTable.Rows[i]["Fit"],
                        Fabric = (string)dataTable.Rows[i]["Fabric"],
                        Category = (string)dataTable.Rows[i]["Category"],
                        Discount = (int)dataTable.Rows[i]["Discount"],
                        Price = (int)dataTable.Rows[i]["Price"],
                    };
                    Results.Add(productDataModel);
                }
                return Results;
            }
        }

        public int Add(ProductInputDto product)
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

        public void Update(ProductInputDto product)
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
