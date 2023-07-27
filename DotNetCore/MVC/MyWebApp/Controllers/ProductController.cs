using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using Microsoft.CodeAnalysis;
using Microsoft.AspNetCore.Mvc.Rendering;
using MyWebApp.Repositories;
using Microsoft.IdentityModel.Tokens;
using MyWebApp.Enums;
using System.Drawing;
using MyWebApp.DataModel;
using AutoMapper;
using MyWebApp.ViewModels.Products;

namespace MyWebApp.Controllers
{
    public class ProductController : Controller
    {
        IProductRepository _productRepository;
        IMapper _imapper;

        public ProductController(IProductRepository productRepository)
        {
            _productRepository = productRepository;

            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ProductAddVm, Product>();
                cfg.CreateMap<Product, ProductVm>();

                cfg.CreateMap<ProductFilterVm, ProductFilter>();
                cfg.CreateMap<ProductResult, ProductResultVm>();

                cfg.CreateMap<ProductFilterOptionalVm, ProductFilterOptional>();
                cfg.CreateMap<ProductResultsOptional, ProductResultsOptionalVm>();
            });

            _imapper = configuration.CreateMapper();
        }

        public IActionResult Index()
        {
            List<Product> products = _productRepository.GetAll();

            List<ProductVm> productVm = _imapper.Map<List<Product>, List<ProductVm>>(products);

            ViewBag.productCount = productVm.Count;

            return View(productVm);
        }

        public IActionResult View(int id)
        {
            //ProductRepository productRepository = new();

            //var productRepository1 = new ProductRepository();

            //productRepository1.Get()

            Product product = _productRepository.Get(id);
            return View(product);
        }

        //public IActionResult Edit(int id)
        //{
        //    Product product = _productRepository.Get(id);

        //    List<ProductColor> productColors = _productRepository.GetColors();
        //    ViewBag.ProductColors = new SelectList(productColors, "Id", "ColorName");

        //    List<ProductSize> productSizes = _productRepository.GetSizes();
        //    ViewBag.productSizes = new SelectList(productSizes, "Id", "Size");

        //    List<ProductFabric> productFabrics = _productRepository.GetFabric();
        //    ViewBag.ProductFabrics = new SelectList(productFabrics, "Id", "FabricName");

        //    List<ProductCategory> ProductCategories = _productRepository.GetCategory();
        //    ViewBag.ProductCategories = new SelectList(ProductCategories, "Id", "CategoryName");

        //    return View(product);
        //}

        public IActionResult ProductSearch()
        {
            List<ProductColor> productColors = _productRepository.GetColors();
            ViewBag.ProductColors = new SelectList(productColors, "Id", "ColorName");

            List<ProductSize> productSizes = _productRepository.GetSizes();
            ViewBag.productSizes = new SelectList(productSizes, "Id", "Size");

            List<ProductFabric> productFabrics = _productRepository.GetFabric();

            ViewBag.ProductFabrics = new SelectList(productFabrics, "Id", "FabricName");

            return View();
        }

        public IActionResult ProductSearchResult(ProductFilterVm productFilterVm)
        {
            ProductFilter productFilter = _imapper.Map<ProductFilterVm, ProductFilter>(productFilterVm);

            List<ProductResult> ProductsResult = _productRepository.GetProducts(productFilter);

            List<ProductResultVm> productResultsOutPut = _imapper.Map<List<ProductResult>, List<ProductResultVm>>(ProductsResult);

            return View("ProductSearchResult", productResultsOutPut);
        }

        public IActionResult ProductSearchOptional()
        {
            List<ProductColor> productColors = _productRepository.GetColors();
            ViewBag.ProductColors = new SelectList(productColors, "Id", "ColorName");

            List<ProductSize> productSizes = _productRepository.GetSizes();
            ViewBag.productSizes = new SelectList(productSizes, "Id", "Size");

            return View();
        }

        public IActionResult GetProductResultOptional(ProductFilterOptionalVm optionalFilterVm)
        {
            ProductFilterOptional optionalFilter = _imapper.Map<ProductFilterOptionalVm, 
                                                ProductFilterOptional>(optionalFilterVm);

            List<ProductResultsOptional> optionalResult = _productRepository.GetProductsResult(optionalFilter);

            List<ProductResultsOptionalVm> optionalResultVm = _imapper.Map<List<ProductResultsOptional>, 
                                                        List<ProductResultsOptionalVm>>(optionalResult);
            return View("GetProductResultOptional", optionalResultVm);
        }

        //List<Product> products = new List<Product>();
        //products.Add(new Product { ProductName = "Jacket", BrandName="Levis", Price = 1000 });
        //products.Add(new Product { ProductName = "Belt", BrandName = "Levis", Price =100 });
        //products.Add(new Product { ProductName = "Belt", BrandName = "Bata", Price = 200 });

        //products = products.Where(x=>x.ProductName == productName).ToList();

        public IActionResult Add()
        {
            List<ProductColor> productColors = _productRepository.GetColors();
            ViewBag.ProductColors = new SelectList(productColors, "Id", "ColorName");

            List<ProductSize> productSizes = _productRepository.GetSizes();
            ViewBag.productSizes = new SelectList(productSizes, "Id", "Size");

            List<ProductFabric> productFabrics = _productRepository.GetFabric();
            ViewBag.ProductFabrics = new SelectList(productFabrics, "Id", "FabricName");

            List<ProductCategory> ProductCategories = _productRepository.GetCategory();
            ViewBag.ProductCategories = new SelectList(ProductCategories, "Id", "CategoryName");

            return View();
        }

        [HttpPost]
        public IActionResult Add(ProductAddVm productAddVm)
        {
            try
            {
                Product product = _imapper.Map<ProductAddVm, Product>(productAddVm);

                int affectedRowCount = _productRepository.Add(product);

                if (affectedRowCount > 0)
                {
                    TempData["SuccessMessageForAdd"] = "Product Add Successful";
                }
                return RedirectToAction("Index");
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
        public IActionResult Update(ProductVm productVm)
        {
            try
            {
                Product product = _imapper.Map<ProductVm, Product>(productVm);

                int affectedRowCount = _productRepository.Update(product);

                if (affectedRowCount > 0)
                {
                    TempData["SuccessMessageForUpdate"] = "Product Update Successful";
                }
                return RedirectToAction("Index");
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
            int deleteRow = _productRepository.Delete(id);

            if (deleteRow > 0)
            {
                TempData["SuccessMessageForDelete"] = "Product Record Delete Successful";
            }
            return RedirectToAction("Index");
        }

        //private List<ProductSizesVm> GetSizes()
        //{
        //    List<ProductSizesVm> productSizes = _productRepository.GetSizes();
        //    return productSizes;
        //}

        //private List<ProductColorVm> GetColors()
        //{
        //    List<ProductColorVm> productColorsVm = _productRepository.GetColors();
        //    return productColorsVm;
        //}

        //private List<ProductFabricVm> GetFabric()
        //{                                       
        //    List<ProductFabricVm> productFabrics = _productRepository.GetFabric();
        //    return productFabrics;
        //}

        //private List<ProductCategoryVm> GetCategory()
        //{
        //    List<ProductCategoryVm> ProductCategories = _productRepository.GetCategory();
        //    return ProductCategories;
        //}

        //private void SetAllDropdownItemsInViewBag() 
        //{
        //    List<ProductColorVm> productColors = GetColors();
        //    ViewBag.ProductColors = new SelectList(productColors, "Id", "ColorName");

        //    //List<ProductSizes> productSizes = GetSizes();
        //    ViewBag.productSizes = new SelectList(GetSizes(), "Id", "Size");

        //    //List<ProductFabric> productFabrics = GetFabric();
        //    ViewBag.ProductFabrics = new SelectList(GetFabric(), "Id", "FabricName");

        //    //List<ProductCategory> ProductCategories = GetCategory();
        //    ViewBag.ProductCategories = new SelectList(GetCategory(), "Id", "CategoryName");
        //}
    }
}
