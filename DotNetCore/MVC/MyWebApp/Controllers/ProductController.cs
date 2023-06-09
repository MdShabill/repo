using Microsoft.AspNetCore.Mvc;
using MyWebApp.Models;
using Microsoft.Data.SqlClient;
using System.Data;
using Microsoft.CodeAnalysis;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MyWebApp.Controllers
{
    public class ProductController : Controller
    {
        public readonly IConfiguration _Configuration;
        SqlConnection sqlConnection;

        public ProductController(IConfiguration configuration)
        {
            _Configuration = configuration;
            sqlConnection = new(_Configuration.GetConnectionString("EcommerceDBConnection").ToString());
        }

        public IActionResult GetAll()
        {
            const string sqlQuery = @"
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
            ViewBag.productCount = products.Count;
            return View(products);
        }

        public IActionResult View(int id)
        {
            string sqlQuery = $"Select * From Products Where Id = @id";
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
            return View("View", product);
        }

        public IActionResult Edit(int id)
        {
            string sqlQuery = $@"SELECT Products.Id, Products.ProductName, Products.BrandName, 
                   Products.Fit, Products.Fabric, Products.Category, Products.Discount,
                   Products.Price, ProductSizes.Size, ProductColors.ColorName 
                   FROM Products 
                   Inner Join 
                   ProductSizes On Products.SizeId = ProductSizes.Id 
                   Inner Join 
                   ProductColors On Products.ColorId = ProductColors.Id
                   Where Products.Id = @Id";
            SqlDataAdapter sqlDataAdapter = new(sqlQuery, sqlConnection);
            sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@Id", id);
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
            List<ProductSizes> productSizes = GetSizes();
            ViewBag.productSizes = new SelectList(productSizes, "Id", "Size");

            List<ProductColor> productColors = GetColors();
            ViewBag.ProductColors = new SelectList(productColors, "Id", "ColorName");

            return View(product);
        }

        public IActionResult Delete(int id)
        {
            string deleteQuery = "Delete From Products Where Id = @Id";
            SqlCommand sqlCommand = new(deleteQuery, sqlConnection);
            sqlCommand.Parameters.AddWithValue("@Id", id);
            sqlConnection.Open();
            sqlCommand.ExecuteNonQuery();
            sqlConnection.Close();

            return View("DeleteSuccess");
        }

        public IActionResult Add()
        {
            List<ProductColor> productColors = GetColors();
            ViewBag.ProductColors = new SelectList(productColors, "Id", "ColorName");

            List<ProductSizes> productSizes = GetSizes();
            ViewBag.productSizes = new SelectList(productSizes, "Id", "Size");

            return View("Add");
        }

        [HttpPost]
        public IActionResult Add(Product product)
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
            return View("AddSuccess");
        }

        public IActionResult Update()
        {
            return View("Update");
        }

        [HttpPost]
        public IActionResult Update(Product product)
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

            return View("UpdateSuccess");
        }

        private List<ProductColor> GetColors()
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
            return (productColors);
        }

        private List<ProductSizes> GetSizes()
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
            return (productSizes);
        }
    }
}
