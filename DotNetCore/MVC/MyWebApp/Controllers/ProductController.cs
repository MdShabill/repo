using Microsoft.AspNetCore.Mvc;
using MyWebApp.Models;
using Microsoft.Data.SqlClient;
using System.Data;
using Microsoft.CodeAnalysis;
using Microsoft.AspNetCore.Mvc.Rendering;
using MyWebApp.Repositories;
using Microsoft.IdentityModel.Tokens;
using MyWebApp.Enums;
using System.Drawing;

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
            List<Product> products = _productRepository.GetAll();

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

            string successMessageForDelete = ViewBag.SuccessMessageForDelete;
            if (!string.IsNullOrEmpty(successMessageForDelete))
            {
                ViewBag.SuccessMessageForDelete = successMessageForDelete;
            }

            ViewBag.productCount = products.Count;

            return View("Index", products);
        }

        public IActionResult View(int id)
        {
            //ProductRepository productRepository = new();

            //var productRepository1 = new ProductRepository();

            //productRepository1.Get()

            Product product = _productRepository.Get(id);
            return View(product);
        }

        public IActionResult Edit(int id)
        {
            Product product = _productRepository.Get(id);

            //List<ProductSizes> productSizes = GetSizes();
            ViewBag.productSizes = new SelectList(GetSizes(), "Id", "Size");

            //List<ProductColor> productColors = GetColors();
            ViewBag.ProductColors = new SelectList(GetColors(), "Id", "ColorName");

            //List<ProductFabric> productFabrics = GetFabric();
            ViewBag.ProductFabrics = new SelectList(GetFabric(), "Id", "FabricName");

            //List<ProductCategory> ProductCategories = GetCategory();
            ViewBag.ProductCategories = new SelectList(GetCategory(), "Id", "CategoryName");

            return View(product);
        }

        public IActionResult ProductSearch()
        {
            ViewBag.ProductColors = new SelectList(GetColors(), "Id", "ColorName");

            ViewBag.ProductSizes = new SelectList(GetSizes(), "Id", "Size");

            ViewBag.ProductFabrics = new SelectList(GetFabric(), "Id", "FabricName");

            return View();
        }

        public IActionResult ProductSearchResult(Product productFilter)
        {
            List<Product> products = _productRepository.GetProducts(productFilter.ProductName, productFilter.ColorId, productFilter.SizeId, productFilter.Price);
            return View("ProductSearchResult", products);
        }

        //List<Product> products = new List<Product>();
        //products.Add(new Product { ProductName = "Jacket", BrandName="Levis", Price = 1000 });
        //products.Add(new Product { ProductName = "Belt", BrandName = "Levis", Price =100 });
        //products.Add(new Product { ProductName = "Belt", BrandName = "Bata", Price = 200 });

        //products = products.Where(x=>x.ProductName == productName).ToList();

        public IActionResult Add()
        {
            SetAllDropdownItemsInViewBag();
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
                    SetAllDropdownItemsInViewBag();                    
                    return View();
                }

                product.ProductName = product.ProductName.Trim();
                product.BrandName = product.BrandName.Trim();

                _productRepository.Add(product);

                ViewBag.SuccessMessageForAdd = "Product Add Successful";
                List<Product> products = _productRepository.GetAll();
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

        [HttpPost]
        public IActionResult Update(Product product)
        {
            try
            {
                string errorMessage = validateProductAddOrUpdate(product, true);

                if (!string.IsNullOrEmpty(errorMessage))
                {
                    ViewBag.errorMssage = errorMessage;
                    SetAllDropdownItemsInViewBag();
                    return View();
                }

                product.ProductName = product.ProductName.Trim();
                product.BrandName = product.BrandName.Trim();
                
                _productRepository.Update(product);

                ViewBag.SuccessMessageForUpdate = "Product Update SuccessFul";
                List<Product> products = _productRepository.GetAll();
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

        public IActionResult Delete(int id)
        {
            _productRepository.Delete(id);

            ViewBag.SuccessMessageForDelete = "Product Delete Successful";
            List<Product> products = _productRepository.GetAll();
            return View("Index", products);
        }

        private List<ProductSizes> GetSizes()
        {
            List<ProductSizes> productSizes = _productRepository.GetSizes();
            return (productSizes);
        }

        private List<ProductColor> GetColors()
        {
            List<ProductColor> productColors = _productRepository.GetColor();
            return (productColors);
        }

        private List<ProductFabric> GetFabric()
        {
            List<ProductFabric> productFabrics = _productRepository.GetFabric();
            return (productFabrics);
        }

        private List<ProductCategory> GetCategory()
        {
            List<ProductCategory> ProductCategories = _productRepository.GetCategory();
            return (ProductCategories);
        }

        private void SetAllDropdownItemsInViewBag() 
        {
            List<ProductColor> productColors = GetColors();
            ViewBag.ProductColors = new SelectList(productColors, "Id", "ColorName");

            //List<ProductSizes> productSizes = GetSizes();
            ViewBag.productSizes = new SelectList(GetSizes(), "Id", "Size");

            //List<ProductFabric> productFabrics = GetFabric();
            ViewBag.ProductFabrics = new SelectList(GetFabric(), "Id", "FabricName");

            //List<ProductCategory> ProductCategories = GetCategory();
            ViewBag.ProductCategories = new SelectList(GetCategory(), "Id", "CategoryName");
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

            else if (!Enum.IsDefined(typeof(FitType), product.Fit))
                errorMessage = "Invalid Product Fiting";

            return errorMessage;
        }
    }
}
