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
using MyWebApp.ViewModels;

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
                cfg.CreateMap<Product, ProductVm>();
                cfg.CreateMap<ProductAddVm, Product>();
                cfg.CreateMap<ProductUpdateVm, Product>();
                cfg.CreateMap<Product, ProductUpdateVm>();

                cfg.CreateMap<ProductFilterVm, ProductFilter>();
                cfg.CreateMap<ProductResult, ProductResultVm>();

                cfg.CreateMap<ProductFilterOptionalVm, ProductFilterOptional>();
                cfg.CreateMap<ProductResultsOptional, ProductResultsOptionalVm>();

                cfg.CreateMap<ProductSize, ProductSizesVm>();
                cfg.CreateMap<ProductColor, ProductColorVm>();
                cfg.CreateMap<ProductFabric, ProductFabricVm>();
                cfg.CreateMap<ProductCategory, ProductCategoryVm>();
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
            ProductVm productVm = _imapper.Map<Product, ProductVm>(product);
            return  View(productVm);
        }

        public IActionResult Edit(int id)
        {
            Product product = _productRepository.Get(id);

            ProductUpdateVm productUpdateVm = _imapper.Map<Product, ProductUpdateVm>(product);

            List<ProductColor> productColors = _productRepository.GetColors();
            ViewBag.ProductColors = new SelectList(productColors, "Id", "ColorName");

            List<ProductSize> productSizes = _productRepository.GetSizes();
            ViewBag.productSizes = new SelectList(productSizes, "Id", "Size");

            List<ProductFabric> productFabrics = _productRepository.GetFabric();
            ViewBag.ProductFabrics = new SelectList(productFabrics, "Id", "FabricName");

            List<ProductCategory> ProductCategories = _productRepository.GetCategory();
            ViewBag.ProductCategories = new SelectList(ProductCategories, "Id", "CategoryName");

            return View(productUpdateVm);
        }

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
                
                if (string.IsNullOrWhiteSpace(productAddVm.ProductName))
                {
                    ViewBag.ErrorMessage = "Product Name should not be blank ";
                    return View();
                }

                if (productAddVm.ProductName.Length > 20)
                {
                    ViewBag.ErrorMessage = "Product Name should be 20 characters or less";
                    return View();
                }

                if (string.IsNullOrWhiteSpace(productAddVm.BrandName))
                {
                    ViewBag.ErrorMessage = "Brand Name should not be blank ";
                    return View();
                }

                if (productAddVm.BrandName.Length > 20)
                {
                    ViewBag.ErrorMessage = "Brand Name should be 20 characters or less ";
                    return View();
                }

                if (productAddVm.SizeId <= 0)
                {
                    ViewBag.ErrorMessage = "Please select a valid product size ";
                    return View();
                }

                if (productAddVm.ColorId <= 0)
                {
                    ViewBag.ErrorMessage = "Please select a valid product color ";
                    return View();
                }

                if (!Enum.IsDefined(typeof(FitType), productAddVm.Fit))
                {
                    ViewBag.ErrorMessage = "Product fit invalid ";
                    return View();
                }

                if (productAddVm.FabricId <= 0)
                {
                    ViewBag.ErrorMessage = "Please select a valid product fabric ";
                    return View();
                }

                if (productAddVm.CategoryId <= 0)
                {
                    ViewBag.ErrorMessage = "Please select a valid product category ";
                    return View();
                }

                if (productAddVm.Discount > 70)
                {
                    ViewBag.ErrorMessage = "Product discount should be 70% or less ";
                    return View();
                }

                if (productAddVm.Price > 20000)
                {
                    ViewBag.ErrorMessage = "Product price should be 20000 or less ";
                    return View();
                }


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
        public IActionResult Update(ProductUpdateVm productUpdateVm)
        {
            try
            {
                if (productUpdateVm.Id < 1)
                {
                    ViewBag.ErrorMessage = "Id Can Not Be Less Than Zero";
                    return View();
                }

                if (string.IsNullOrWhiteSpace(productUpdateVm.ProductName))
                {
                    ViewBag.ErrorMessage = "Product Name should not be blank ";
                    return View();
                }

                if (productUpdateVm.ProductName.Length > 20)
                {
                    ViewBag.ErrorMessage = "Product Name should be 20 characters or less";
                    return View();
                }

                if (string.IsNullOrWhiteSpace(productUpdateVm.BrandName))
                {
                    ViewBag.ErrorMessage = "Brand Name should not be blank ";
                    return View();
                }

                if (productUpdateVm.BrandName.Length > 20)
                {
                    ViewBag.ErrorMessage = "Brand Name should be 20 characters or less ";
                    return View();
                }

                if (productUpdateVm.SizeId <= 0)
                {
                    ViewBag.ErrorMessage = "Please select a valid product size ";
                    return View();
                }

                if (productUpdateVm.ColorId <= 0)
                {
                    ViewBag.ErrorMessage = "Please select a valid product color ";
                    return View();
                }

                if (!Enum.IsDefined(typeof(FitType), productUpdateVm.Fit))
                {
                    ViewBag.ErrorMessage = "Product fit invalid ";
                    return View();
                }

                if (productUpdateVm.FabricId <= 0)
                {
                    ViewBag.ErrorMessage = "Please select a valid product fabric ";
                    return View();
                }

                if (productUpdateVm.CategoryId <= 0)
                {
                    ViewBag.ErrorMessage = "Please select a valid product category ";
                    return View();
                }

                if (productUpdateVm.Discount > 70)
                {
                    ViewBag.ErrorMessage = "Product discount should be 70% or less ";
                    return View();
                }

                if (productUpdateVm.Price > 20000)
                {
                    ViewBag.ErrorMessage = "Product price should be 20000 or less ";
                    return View();
                }

                Product product = _imapper.Map<ProductUpdateVm, Product>(productUpdateVm);

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

        private List<ProductSizesVm> GetSizes()
        {
            List<ProductSize> productSizes = _productRepository.GetSizes();

            List<ProductSizesVm> productSizesVm = _imapper.Map<List<ProductSize>, List<ProductSizesVm>>(productSizes);

            return productSizesVm;
        }

        private List<ProductColorVm> GetColors()
        {
            List<ProductColor> productColors = _productRepository.GetColors();

            List<ProductColorVm> productSizesVm = _imapper.Map<List<ProductColor>, List<ProductColorVm>>(productColors);

            return productSizesVm;
        }

        private List<ProductFabricVm> GetFabric()
        {
            List<ProductFabric> productFabrics = _productRepository.GetFabric();

            List<ProductFabricVm> productFabricsVm = _imapper.Map<List<ProductFabric>, List<ProductFabricVm>>(productFabrics);

            return productFabricsVm;
        }

        private List<ProductCategoryVm> GetCategory()
        {
            List<ProductCategory> ProductCategories = _productRepository.GetCategory();

            List<ProductCategoryVm> productCategoryVm = _imapper.Map<List<ProductCategory>, List<ProductCategoryVm>>(ProductCategories);

            return productCategoryVm;
        }

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
