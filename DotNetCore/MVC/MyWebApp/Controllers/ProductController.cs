using Microsoft.AspNetCore.Mvc;
using MyWebApp.Models;
using Microsoft.Data.SqlClient;
using System.Data;
using Microsoft.CodeAnalysis;
using Microsoft.AspNetCore.Mvc.Rendering;
using MyWebApp.Repositories;
using Microsoft.IdentityModel.Tokens;

namespace MyWebApp.Controllers
{
    public class ProductController : Controller
    {
        IProductRepository _productRepository;
        
        public ProductController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public IActionResult GetAll()
        {
            List<Product> products = _productRepository.GetAll();

            ViewBag.productCount = products.Count;
            return View(products);
        }

        public IActionResult View(int id)
        {
            Product product = _productRepository.Get(id);
            return View(product);
        }

        public IActionResult Edit(int id)
        {
            Product product = _productRepository.Get(id);

            List<ProductSizes> productSizes = GetSizes();
            ViewBag.productSizes = new SelectList(productSizes, "Id", "Size");

            List<ProductColor> productColors = GetColors();
            ViewBag.ProductColors = new SelectList(productColors, "Id", "ColorName");

            return View(product);
        }

        public IActionResult Delete(int id)
        {
            _productRepository.Delete(id);
            return View("DeleteSuccess");
        }

        public IActionResult Add()
        {

            SetSelectListColorAndSize();

            return View();
        }

        [HttpPost]
        public IActionResult Add(Product product)
        {
            try
            {
                string errormessage = validateProductAddOrUpdate(product);
                
                if (!string.IsNullOrEmpty(errormessage))
                {
                    ViewBag.errormessage = errormessage;

                    SetSelectListColorAndSize();                    
                    return View();
                }

                product.ProductName = product.ProductName.Trim();
                product.BrandName = product.BrandName.Trim();
                product.Fabric = product.Fabric.Trim();
                product.Category = product.Category.Trim();

                if (ModelState.IsValid)
                {
                    _productRepository.Add(product);
                    return View("AddSuccess");
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

        public IActionResult Update()
        {
            return View("Update");
        }

        [HttpPost]
        public IActionResult Update(Product product)
        {
            try
            {
                string errormessage = validateProductAddOrUpdate(product, true);
                if (!string.IsNullOrEmpty(errormessage))
                    return BadRequest(errormessage);

                if (ModelState.IsValid)
                {
                    _productRepository.Update(product);
                }
                return View("UpdateSuccess");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", @"Unable to save changes. 
                    Try again, and if the problem persists 
                    see your system administrator.");

                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        private List<ProductSizes> GetSizes()
        {
            List<ProductSizes> productSizes = _productRepository.GetSizesDetails();
            return (productSizes);
        }

        private List<ProductColor> GetColors()
        {
            List<ProductColor> productColors = _productRepository.GetcolorDetails();
            return (productColors);
        }

        private void SetSelectListColorAndSize() 
        {
            List<ProductColor> productColors = GetColors();
            ViewBag.ProductColors = new SelectList(productColors, "Id", "ColorName");

            List<ProductSizes> productSizes = GetSizes();
            ViewBag.productSizes = new SelectList(productSizes, "Id", "Size");

        }

        private string validateProductAddOrUpdate(Product product, bool IsUpdate = false)
        {
            string errorMessage = "";

            if (IsUpdate == true)
            {
                if (product.Id < 1)
                    errorMessage = "Product Id Can Not Be Less Then Zero";
            }

            if (string.IsNullOrEmpty(product.ProductName))
                errorMessage = "Product Name Can Not Be Blank";

            else if (product.ProductName.Length >= 15)
                errorMessage = "Product Name Should Be Under 15 Characters";

            else if (string.IsNullOrEmpty(product.BrandName))
                errorMessage = "Brand Name Can Not Be Blank";

            else if (product.BrandName.Length >= 15)
                errorMessage = "Brand Name Should Be Under 15 Characters";

            else if (string.IsNullOrEmpty(product.Fit))
                errorMessage = "Product Fit Can Not Be Blank";

            else if (string.IsNullOrEmpty(product.Fabric))
                errorMessage = "Product Fabric Can Not Be Blank";

            else if (string.IsNullOrEmpty(product.Category))
                errorMessage = "Product Category Can Not Be Blank";

            return errorMessage;
        }
    }
}
