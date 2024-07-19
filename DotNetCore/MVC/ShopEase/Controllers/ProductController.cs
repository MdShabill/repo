using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Microsoft.AspNetCore.Mvc.Rendering;
using ShopEase.Repositories;
using ShopEase.DataModels.Product;
using ShopEase.ViewModels;
using ShopEase.DataModels.Cart;
using ShopEase.ViewModels.Cart;

namespace ShopEase.Controllers
{
    public class ProductController : Controller
    {
        IProductRepository _productRepository;
        IProductRepositoryUsingSP _productRepositoryUsingSP;
        IProductBrandRepository _productBrandRepository;
        IProductCategoryReopsitory _productCategoryReopsitory;
        IProductSupplierRepository _productSupplierRepository;
        IMapper _imapper;
        private readonly IWebHostEnvironment _env;

        public ProductController(IProductRepository productRepository, 
                                 IProductBrandRepository productBrandRepository,
                                 IProductCategoryReopsitory productCategoryReopsitory,
                                 IProductSupplierRepository productSupplierRepository,
                                 IWebHostEnvironment env,
                                 IProductRepositoryUsingSP productRepositoryUsingSP)
        {
            _productRepository = productRepository;
            _productBrandRepository = productBrandRepository;
            _productCategoryReopsitory = productCategoryReopsitory;
            _productSupplierRepository = productSupplierRepository;
            _productRepositoryUsingSP = productRepositoryUsingSP;

            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Product, ProductVm>();
                cfg.CreateMap<ProductAddVm, ProductAdd>();
                cfg.CreateMap<ProductBrand, ProductBrandVm>();
                cfg.CreateMap<ProductCategory, ProductCategoryVm>();
                cfg.CreateMap<ProductSupplier, ProductSupplierVm>();
                cfg.CreateMap<ProductFilterVm, ProductFilter>();
                cfg.CreateMap<ProductSearchResult, ProductSearchResultVm>();
            });

            _imapper = configuration.CreateMapper();
            _env = env;
        }

        public IActionResult Index(string sortColumnName, string sortOrder)
        {
            if (string.IsNullOrEmpty(sortColumnName))
                sortColumnName = "Title";

            if (string.IsNullOrEmpty(sortOrder))
                sortOrder = "ASC";

            //Approach 1
            string SessionSortColumnName = HttpContext.Session.GetString("SortColumnName");
            string SessionSortOrder = HttpContext.Session.GetString("SortOrder");

            if (!string.IsNullOrEmpty(SessionSortColumnName)  && !string.IsNullOrEmpty(SessionSortOrder) 
                && SessionSortColumnName == sortColumnName)
            {
                if (HttpContext.Session.GetString("SortOrder") == "ASC")
                {
                    sortOrder = "DESC";
                    HttpContext.Session.SetString("SortOrder", sortOrder);
                }
                else if (HttpContext.Session.GetString("SortOrder") == "DESC")
                {
                    sortOrder = "ASC";
                    HttpContext.Session.SetString("SortOrder", sortOrder);
                }   
            }
            else
            {
                HttpContext.Session.SetString("SortColumnName", sortColumnName);
                HttpContext.Session.SetString("SortOrder", sortOrder);
            }


            // Approach 2
            //if (!string.IsNullOrEmpty(HttpContext.Session.GetString("SortColumnName")) && 
            //    !string.IsNullOrEmpty(HttpContext.Session.GetString("SortOrder")))
            //{
            //    if (HttpContext.Session.GetString("SortColumnName") == sortColumnName)
            //    {
            //        if (HttpContext.Session.GetString("SortOrder") == "ASC")
            //        {
            //            sortOrder = "DESC";
            //            HttpContext.Session.SetString("SortOrder", sortOrder);
            //        }
            //        else if (HttpContext.Session.GetString("SortOrder") == "DESC")
            //        {
            //            sortOrder = "ASC";
            //            HttpContext.Session.SetString("SortOrder", sortOrder);
            //        }
            //    }
            //    else
            //    {
            //        HttpContext.Session.SetString("SortColumnName", sortColumnName);
            //        HttpContext.Session.SetString("SortOrder", sortOrder);
            //    }
            //}
            //else 
            //{
            //    HttpContext.Session.SetString("SortColumnName", sortColumnName);
            //    HttpContext.Session.SetString("SortOrder", sortOrder);
            //}

            List<Product> products = _productRepository.GetSortedProducts(sortColumnName, sortOrder);

            List<ProductVm> productsVm = _imapper.Map<List<Product>, List<ProductVm>>(products);

            ViewBag.productsCount = productsVm.Count;

            return View(productsVm);
        }

        public IActionResult View(int id)
        {
            //Product product = _productRepositoryUsingSP.GetProduct(id);

            Product product = _productRepository.GetProduct(id);
            ProductVm productVm = _imapper.Map<Product, ProductVm>(product);

            HttpContext.Session.SetInt32("ProductId", productVm.Id);
            HttpContext.Session.SetString("ProductPrice", productVm.Price.ToString());
            return View(productVm);
        }

        public IActionResult ProductSearch()
        {
            ViewBag.CustomerId = HttpContext.Session.GetInt32("CustomerId");
            ViewBag.CustomerFullName = HttpContext.Session.GetString("CustomerFullName");
            ViewBag.CustomerEmail = HttpContext.Session.GetString("CustomerEmail");

            List<ProductBrand> productBrands = _productBrandRepository.GetBrands();
            ViewBag.Brands = new SelectList(productBrands, "Id", "BrandName");

            List<ProductCategory> categories = _productCategoryReopsitory.GetCategories();
            ViewBag.Categories = new SelectList(categories, "Id", "CategoryName");

            return View();
        }

        public IActionResult ProductSearchResult(ProductFilterVm productFilterVm)
        {
            if (!string.IsNullOrEmpty(productFilterVm.Title) ||
                productFilterVm.BrandId != 0 ||
                productFilterVm.Min != 0 ||
                productFilterVm.Max != 0 ||
                productFilterVm.CategoryId != 0 )
            {

                ProductFilter productFilters = _imapper.Map<ProductFilterVm, ProductFilter>(productFilterVm);

                //List<ProductSearchResult> productSearchResults = _productRepositoryUsingSP.GetProductsResult(productFilters);

                List<ProductSearchResult> productSearchResults = _productRepository.GetProductsResult(productFilters);
                List<ProductSearchResultVm> productSearchResultsVm = _imapper.Map<List<ProductSearchResult>, List<ProductSearchResultVm>>(productSearchResults);

                ViewBag.TotalCountRecord = productSearchResultsVm.Count;
                return View(productSearchResultsVm);
            }
            else
            {
                ViewBag.ErrorMessage = "Please Provide Any Input ";

                List<ProductBrand> productBrands = _productBrandRepository.GetBrands();
                ViewBag.Brands = new SelectList(productBrands, "Id", "BrandName");

                List<ProductCategory> categories = _productCategoryReopsitory.GetCategories();
                ViewBag.Categories = new SelectList(categories, "Id", "CategoryName");

                return View("ProductSearch");
            }
        }

        public IActionResult ProductSearchResultByBrand(int brandId)
        {
            ProductFilter productFilters = new();
            productFilters.BrandId = brandId;
            List<ProductSearchResult> productSearchResults = _productRepository.GetProductsResult(productFilters);

            List<ProductSearchResultVm> productSearchResultsVm = _imapper.Map<List<ProductSearchResult>, List<ProductSearchResultVm>>(productSearchResults);

            ViewBag.TotalCountRecord = productSearchResultsVm.Count;
            return View("ProductSearchResult", productSearchResultsVm);

        }

        public IActionResult Add()
        {
            DropDownSelectList();
            return View();
        }

        [HttpPost]
        public IActionResult Add(ProductAddVm productAddVm) 
        {
            DropDownSelectList();

            if (string.IsNullOrWhiteSpace(productAddVm.Title))
            {
                ViewBag.ErrorMessage = "Title should not be blank ";
                return View();
            }

            if (productAddVm.Title.Length > 50)
            {
                ViewBag.ErrorMessage = "Title should be 50 characters or less ";
                return View();
            }

            if (productAddVm.BrandId == 0)
            {
                ViewBag.ErrorMessage = "Please Select a Valid Brand Id Option ";
                return View();
            }

            if (productAddVm.Price > 300000)
            {
                ViewBag.ErrorMessage = "Product price should be 300000 or less ";
                return View();
            }

            if (productAddVm.Discount > 60)
            {
                ViewBag.ErrorMessage = "Product discount should be 60% or less ";
                return View();
            }

            if (productAddVm.CategoryId == 0)
            {
                ViewBag.ErrorMessage = "Please Select a Valid Category Id Option ";
                return View();
            }

            if (productAddVm.SupplierId == 0)
            {
                ViewBag.ErrorMessage = "Please Select a Valid Supplier Id Option ";
                return View();
            }

            string dir = Path.Combine(_env.WebRootPath, "UploadedFiles");
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            string uniqueFileName = GetUniqueFileName(productAddVm.ImageFile.FileName);
            string filePath = Path.Combine(dir, uniqueFileName);

            productAddVm.ImageFile.CopyToAsync(new FileStream(filePath, FileMode.Create));

            ProductAdd productAdd = _imapper.Map<ProductAddVm, ProductAdd>(productAddVm);
            productAdd.ImageName = uniqueFileName;
            int affrectedRowCount = _productRepository.Add(productAdd);
            if (affrectedRowCount > 0)
            {
                TempData["SuccessMessageForAdd"] = "Product Add Successful ";
            }
            return RedirectToAction("Index", productAddVm);
        }

        private string GetUniqueFileName(string fileName)
        {
            fileName = Path.GetFileName(fileName);
            return Path.GetFileNameWithoutExtension(fileName) 
                   + "_"
                   + Guid.NewGuid().ToString().Substring(0, 4)
                   + Path.GetExtension(fileName);
        }

        private void DropDownSelectList()
        {
            List<ProductBrand> productBrands = _productBrandRepository.GetBrands();
            ViewBag.Brands = new SelectList(productBrands, "Id", "BrandName");

            List<ProductCategory> categories = _productCategoryReopsitory.GetCategories();
            ViewBag.Categories = new SelectList(categories, "Id", "CategoryName");

            List<ProductSupplier> productSuppliers = _productSupplierRepository.GetSuppliers();
            ViewBag.Suppliers = new SelectList(productSuppliers, "Id", "SupplierName");
        }

        public ActionResult FirstAction()
        {
            TempData["Message"] = "Hello from FirstAction!";
            return RedirectToAction("SecondAction", "Customer");
        }
    }
}
