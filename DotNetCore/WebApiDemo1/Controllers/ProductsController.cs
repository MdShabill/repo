using Microsoft.AspNetCore.Mvc;
using System.Data;
using Newtonsoft.Json;
using WebApiDemo1.DTO.InputDTO;
using WebApiDemo1.Repositories;
using WebApiDemo1.Enums;
using System.Reflection;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        IProductRepository _productRepository;

        public ProductsController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        [HttpGet]
        [Route("GetAllProducts")]
        public IActionResult GetAllProducts()
        {
            List<ProductDto> products = _productRepository.GetAllProductAsList();

            if (products.Count > 0)
                return Ok(products);
            else
                return NotFound();
        }

        [HttpGet]
        [Route("GetProductCount")]
        public IActionResult GetProductsCount()
        {
            int productCount = _productRepository.GetProductsCount();
            return Ok(productCount);           
        }

        [HttpGet]
        [Route("GetProductDetailById/{productId}")]
        public IActionResult GetProductDetailById(int productId)
        {
            if (productId < 1)
                return BadRequest("ProductId should be greater than 0");
            
            string brandName = _productRepository.GetProductDetailById(productId);
            return Ok(brandName);
        }

        [HttpGet]
        [Route("GetProductsDetailByBrandNameByProductName/{brandName}/{productName?}")]
        public IActionResult GetProductsDetailByBrandNameByProductName(string brandName, string? productName)
        {
            productName = productName.Trim();

            if (string.IsNullOrWhiteSpace(productName))
                return BadRequest("ProductName can not be blank");

            if (productName.Length < 3 || productName.Length > 20)
                return BadRequest("ProductName should be between 3 and 20 characters.");

            List<ProductDto> products = _productRepository.GetProductsDetailByBrandNameByProductName(brandName, productName);

            if (products.Count > 0)
                return Ok(products);
            else
                return NotFound();
        }

        [HttpGet]
        [Route("GetProductsDetailByBrandNameByPrice/{brandName}/{price}")]
        public IActionResult GetProductsDetailByBrandNameByPrice(string brandName, int price)
        {
            brandName = brandName.Trim();

            if (string.IsNullOrWhiteSpace(brandName))
                return BadRequest("BrandName can not be blank");

            if (brandName.Length < 3 || brandName.Length > 30)
                return BadRequest("BrandName should be between 3 and 30 characters.");

            if (price < 600)
                return BadRequest("Product price should be greater than 600");

            List<ProductDto> products = _productRepository.GetProductsDetailByBrandNameByPrice(brandName, price);

            if (products.Count > 0)
                return Ok(products);
            else
                return NotFound();
        }

        [HttpGet]
        [Route("GetProductsByPriceRange/{minimumPrice}/{maximumPrice}")]
        public IActionResult GetProductsByPriceRange(int minimumPrice, int maximumPrice)
        {
            if (maximumPrice < minimumPrice)
                return BadRequest("Maximum price cannot be less than minimum price");

            List<ProductDto> products = _productRepository.GetProductsByPriceRange(minimumPrice, maximumPrice);

            if (products.Count > 0)
                return Ok(products);
            else
                return NotFound();
        }

        [HttpPost]
        [Route("ProductAdd")]
        public IActionResult ProductAdd([FromBody] ProductDto product)
        {
            try
            {
                string errorMessage = validateProductAddOrUpdate(product);
                if (!string.IsNullOrEmpty(errorMessage))
                    return BadRequest(errorMessage);

                if (ModelState.IsValid)
                {
                    int id = _productRepository.ProductAdd(product);

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

        [HttpPost]
        [Route("ProductUpdate")]
        public IActionResult ProductUpdate([FromBody] ProductDto product)
        {
            try
            {
                string errorMessage = validateProductAddOrUpdate(product, true);
                if (!string.IsNullOrEmpty(errorMessage))
                    return BadRequest(errorMessage);

                if (ModelState.IsValid)
                {
                    _productRepository.Update(product);
                    return Ok("Record updated");
                }
                return BadRequest("Record not updated");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", @"Unable to save changes. 
                    Try again, and if the problem persists 
                    see your system administrator.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        private string validateProductAddOrUpdate(ProductDto product, bool isUpdate = false)
        {
            string errorMessage = "";

            product.ProductName = product.ProductName.Trim();
            product.BrandName = product.BrandName.Trim();
            product.Fabric = product.Fabric.Trim();

            if (isUpdate == true)
            {
                if (product.Id < 1)
                {
                    errorMessage = "Id can not be less than 0";
                }
            }

            if (string.IsNullOrWhiteSpace(product.ProductName))
                errorMessage = "Name can not be blank";

            else if (product.ProductName.Length < 3 || product.ProductName.Length > 15)
                errorMessage = "Brand name should be between 3 and 15 characters";

            else if (string.IsNullOrWhiteSpace(product.BrandName))
                errorMessage = "Brand name can not be blank";

            else if (product.BrandName.Length < 3 || product.BrandName.Length > 15)
                errorMessage = "Brand name should be between 3 and 15 characters";

            else if (product.Size <= 25)
                errorMessage = "Product size should be above 25";

            else if (!Enum.IsDefined(typeof(ColorType), product.Color))
                errorMessage = "Invalid Color";

            else if (product.Fit.Contains("Skinny Fit"))
                errorMessage = "This product fitting size is invalid";

            else if (product.Fabric.Contains("Polyester"))
                errorMessage = "This fabric is not accepted";

            else if (product.Category.Contains("Summer Wear"))
                errorMessage = "This product category is not accepted";

            else if (product.Price < 400 || product.Price > 5000)
                errorMessage = "Product price should be between 400 and 5000";

            return errorMessage;
        }
    }
}
