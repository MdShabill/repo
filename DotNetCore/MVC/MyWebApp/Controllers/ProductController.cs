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

        public IActionResult Index()
        {
            List<Product> products = _productRepository.Index();

            string successMessageForAdd = ViewBag.SuccessMessageForAdd;
            if(!string.IsNullOrEmpty(successMessageForAdd))
            {
                ViewBag.SuccessMessageForAdd = successMessageForAdd;
            }

            string successMessageForUpdate = ViewBag.SuccessMessageForUpdate;
            if (!string.IsNullOrEmpty(successMessageForUpdate))
            {
                ViewBag.SuccessMessageForUpdate = successMessageForUpdate;
            }

            ViewBag.productCount = products.Count;

            return View("Index", products);
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
            SetSelectListColorAndSizeinViewBag();
            return View("Add");
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
                    SetSelectListColorAndSizeinViewBag();                    
                    return View();
                }

                product.ProductName = product.ProductName.Trim();
                product.BrandName = product.BrandName.Trim();
                product.Fabric = product.Fabric.Trim();
                product.Category = product.Category.Trim();

                _productRepository.Add(product);

                ViewBag.SuccessMessageForAdd = "Product Add SuccessFul";
                List<Product> products = _productRepository.Index();
                return View("Index", products);
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
            SetSelectListColorAndSizeinViewBag();
            return View();
        }

        [HttpPost]
        public IActionResult Update(Product product)
        {
            try
            {
                string errorMessage = validateProductAddOrUpdate(product, true);

                if (!string.IsNullOrEmpty(errorMessage))
                {
                    ViewBag.errorMssage = errorMessage;
                    SetSelectListColorAndSizeinViewBag();
                    return View();
                }

                product.ProductName = product.ProductName.Trim();
                product.BrandName = product.BrandName.Trim();
                product.Fabric = product.Fabric.Trim();
                product.Category = product.Category.Trim();
 
                _productRepository.Update(product);

                ViewBag.SuccessMessageForUpdate = "Product Update SuccessFul";
                List<Product> products = _productRepository.Index();
                return View("Index", products);
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

        private void SetSelectListColorAndSizeinViewBag() 
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
