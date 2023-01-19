using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using System.Configuration;
using System.Data;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
//using WebApiDemo2.DTO;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using WebApiDemo1.DTO.InputDTO;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        public readonly IConfiguration _Configuration;
        SqlConnection sqlConnection;

        public ProductsController(IConfiguration configuration)
        {
            _Configuration = configuration;
            sqlConnection = new SqlConnection(_Configuration.GetConnectionString("ProductDBConnection").ToString());
        }

        [HttpGet]
        [Route("GetAllProducts")]
        public IActionResult GetAllProducts()
        {
            SqlDataAdapter sqlDataAdapter = new("SELECT * FROM Products", sqlConnection);
            DataTable dataTable = new();
            sqlDataAdapter.Fill(dataTable);

            if (dataTable.Rows.Count > 0)
            {
                return Ok(JsonConvert.SerializeObject(dataTable));
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet]
        [Route("GetProductCount")]
        public IActionResult GetProductsCount()
        {
            string sqlQuery = "SELECT COUNT(*) FROM Products";

            var sqlCommand = new SqlCommand(sqlQuery, sqlConnection);
           
            sqlConnection.Open();
            int productCount = Convert.ToInt32(sqlCommand.ExecuteScalar());
            sqlConnection.Close();

            return Ok(productCount);           
        }

        [HttpGet]
        [Route("GetProductDetail/{productId}")]
        public IActionResult GetProductDetailById(int productId)
        {
            if (productId < 1)
            {
                return BadRequest("ProductId should be greater than 0");
            }
            SqlDataAdapter sqlDataAdapter = new("SELECT * FROM Products WHERE Id = @productId", sqlConnection);

            sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@productId", productId);

            DataTable dataTable = new();
            sqlDataAdapter.Fill(dataTable);

            if (dataTable.Rows.Count > 0)
            {
                return Ok(JsonConvert.SerializeObject(dataTable));
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet]
        [Route("GetProductsDetailByBrandNameByProductName/{brandName}/{productName?}")]
        public IActionResult GetProductsDetailByBrandNameByProductName(string brandName, string? productName)
        {
            if (string.IsNullOrWhiteSpace(productName))
            {
                return BadRequest("ProductName can not be blank");
            }

            productName= productName.Trim();
            if (productName.Length < 3 || productName.Length > 20)
            {
                return BadRequest("ProductName should be between 3 and 20 characters.");
            }

            string sqlQuery = "SELECT * FROM Products WHERE BrandName = @brandName ";

            if (!string.IsNullOrWhiteSpace(productName))
            {
                sqlQuery += "AND ProductName = @productName ";
            }

            SqlDataAdapter sqlDataAdapter = new(sqlQuery, sqlConnection);
            sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@brandName", brandName);
            sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@productName", productName);

            DataTable dataTable = new();
            sqlDataAdapter.Fill(dataTable);

            if (dataTable.Rows.Count > 0)
            {
                return Ok(JsonConvert.SerializeObject(dataTable));
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet]
        [Route("GetProductsDetailByBrandNameByPriceUpto/{brandName}/{priceUpto}")]
        public IActionResult GetProductsDetailByBrandNameByPriceUpto(string brandName, int priceUpto)
        {
            if (string.IsNullOrWhiteSpace(brandName))
            {
                return BadRequest("BrandName can not be blank");
            }
            brandName = brandName.Trim();
            if (brandName.Length < 3 || brandName.Length > 30)
            {
                return BadRequest("BrandName should be between 3 and 30 characters.");
            }

            if (priceUpto < 600)
            {
                return BadRequest("priceUpto should be greater than 600");
            }
            SqlDataAdapter sqlDataAdapter = new(@"SELECT * FROM Products WHERE BrandName = @brandName AND
                                                    Price <= @priceUpto", sqlConnection);

            sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@brandName", brandName);
            sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@priceUpto", priceUpto);

            DataTable dataTable = new();
            sqlDataAdapter.Fill(dataTable);

            if (dataTable.Rows.Count > 0)
            {
                return Ok(JsonConvert.SerializeObject(dataTable));
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet]
        [Route("GetProductsByPriceRange/{minimumPrice}/{maximumPrice}")]
        public IActionResult GetProductsByPriceRange(int minimumPrice, int maximumPrice)
        {
            if (maximumPrice < minimumPrice)
            {
                return BadRequest("Maximum price cannot be smaller than minimum price");
            }

            SqlDataAdapter sqlDataAdapter = new(@" SELECT * FROM Products 
                                                    WHERE Price BETWEEN @minimumPrice AND @maximumPrice
                                                    ORDER BY Price", sqlConnection);

            sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@minimumPrice", minimumPrice);
            sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@maximumPrice", maximumPrice);

            DataTable dataTable = new();
            sqlDataAdapter.Fill(dataTable);

            if (dataTable.Rows.Count > 0)
            {
                return Ok(JsonConvert.SerializeObject(dataTable));
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost]
        [Route("ProductAdd")]
        public IActionResult ProductAdd([FromBody] ProductDto product)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(product.ProductName))
                {
                    return BadRequest("Name can not be blank");
                }

                product.ProductName = product.ProductName.Trim();
                if (product.ProductName.Length < 3 || product.ProductName.Length > 15)
                {
                    return BadRequest("Brand name should be between 3 and 15 characters");
                }

                if (string.IsNullOrWhiteSpace(product.BrandName))
                {
                    return BadRequest("Brand name can not be blank");
                }
                product.BrandName = product.BrandName.Trim();
                if(product.BrandName.Length < 3 || product.BrandName.Length > 15)
                {
                    return BadRequest("Brand name should be between 3 and 15 characters");
                }

                if(product.Size <= 25)
                {
                    return BadRequest("Invalid size product size should be above 25");
                }

                if(product.Color.Contains("Red"))
                {
                    return BadRequest("This product color is invalid");
                }

                if(product.Fit.Contains("Skinny Fit"))
                {
                    return BadRequest("This product fitting size is invalid");
                }

                if(product.Fabric.Contains("Polyester"))
                {
                    return BadRequest("This  product fabric is not accept ");
                }
                
                if(product.Category.Contains("Summer Wear"))
                {
                    return BadRequest("This product category is not accept");
                }

                if(product.Price < 400 || product.Price > 5000)
                {
                    return BadRequest("Product price should be between 400 and 5000");
                }

                if (ModelState.IsValid)
                {
                    string sqlQuery = @"INSERT INTO Products(ProductName, BrandName, Size, Color, Fit, Fabric, Category, Discount, Price)
                                        VALUES (@ProductName, @BrandName, @Size, @Color, @Fit, @Fabric, @Category, @Discount, @Price)
                                        Select Scope_Identity() ";

                    var sqlCommand = new SqlCommand(sqlQuery, sqlConnection);
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

                    return Ok(product.Id);
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", @"Unable to save changes. 
                    Try again, and if the problem persists 
                    see your system administrator.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
