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

        public object ProductName { get; private set; }

        public ProductsController(IConfiguration configuration)
        {
            _Configuration = configuration;
            sqlConnection = new SqlConnection(_Configuration.GetConnectionString("ProductDBCSonnection").ToString());
        }

        [HttpGet]
        [Route("GetAllProducts")]
        public IActionResult GetAllProducts()
        {
            SqlDataAdapter sqlDataAdapter = new("SELECT * FROM Products", sqlConnection);
            var dataTable = new DataTable();
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
        [Route("GetBrandNameById/{productId}")]
        public IActionResult GetBrandNameById(int productId)
        {
            string sqlQuery = @"SELECT BrandName FROM Products where id = @productId";

            var sqlCommand = new SqlCommand(sqlQuery, sqlConnection);
            sqlCommand.Parameters.AddWithValue("@productId", productId);

            sqlConnection.Open();
            string productBrandNane = Convert.ToString(sqlCommand.ExecuteScalar());
            sqlConnection.Close();

            return Ok(productBrandNane);
        }

        [HttpGet]
        [Route("GetProductsDetail/{brandName}/{productName}")]
        public IActionResult GetProductsDetailByProductNameBrandName(string brandName, string productName)
        {
            string sqlQuery = $"SELECT * FROM Products WHERE BrandName Like '%{brandName}%' ";
            if (!string.IsNullOrEmpty(productName))
            {
                sqlQuery = sqlQuery + "AND ProductName Like '%{productName}%' ";
            }

            SqlDataAdapter sqlDataAdapter = new(sqlQuery, sqlConnection);
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
        [Route("GetProductByPriceRange/{minimumPrice}/{maximumPrice}")]
        public IActionResult GetProductByPriceRange(int minimumPrice, int maximumPrice)
        {
            if(minimumPrice < 900 || maximumPrice > 15000)
            {
                return BadRequest("Product minimum and maximum price  Should be between 900 and 15000");
            }

            string sqlQuery = $@" SELECT * FROM Products 
                                    WHERE Price BETWEEN {minimumPrice} AND {maximumPrice}
                                    ORDER BY Price ";
            SqlDataAdapter sqlDataAdapter = new(sqlQuery, sqlConnection);
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

                if(product.BrandName.Length < 3 || product.BrandName.Length > 20)
                {
                    return BadRequest("Brand name should be between 3 and 20 characters");
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
                    string sqlQuery = $@"
                    INSERT INTO Products(ProductName, BrandName, Size, Color, Fit, Fabric, Category, Discount, Price)
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
