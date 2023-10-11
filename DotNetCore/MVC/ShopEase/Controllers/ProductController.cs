using Microsoft.AspNetCore.Mvc;
using ShopEase.Repositories.Product;
using ShopEase.ViewModels.Product;
using AutoMapper;
using ShopEase.DataModels.Product;
using Microsoft.AspNetCore.Mvc.Rendering;
using ShopEase.Repositories;

namespace ShopEase.Controllers
{
    public class ProductController : Controller
    {
        IProductRepository _productRepository;
        IProductBrandRepository _prductBrandRepository;
        IProductCategoryReopsitory _productCategoryReopsitory;
        IProductSupplierRepository _productSupplierRepository;
        IMapper _imapper;

        public ProductController(IProductRepository productRepository, 
                                 IProductBrandRepository prductBrandRepository,
                                 IProductCategoryReopsitory productCategoryReopsitory,
                                 IProductSupplierRepository productSupplierRepository)
        {
            _productRepository = productRepository;
            _prductBrandRepository = prductBrandRepository;
            _productCategoryReopsitory = productCategoryReopsitory;
            _productSupplierRepository = productSupplierRepository;

            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Products, ProductVm>();
                cfg.CreateMap<ProductAddVm, ProductAdd>();
                cfg.CreateMap<ProductBrand, ProductBrandVm>();
                cfg.CreateMap<ProductCategory, ProductCategoryVm>();
                cfg.CreateMap<ProductSupplier, ProductSupplierVm>();
            });

            _imapper = configuration.CreateMapper();
        }

        public IActionResult Index()
        {
            List<Products> products = _productRepository.GetAll();
            List<ProductVm> productsVm = _imapper.Map<List<Products>, List<ProductVm>>(products);
            return View(productsVm);
        }

        public IActionResult Add()
        {
            List<ProductBrand> productBrands = _prductBrandRepository.GetBrands();
            ViewBag.Brands = new SelectList(productBrands, "Id", "BrandName");

            List<ProductCategory> categories = _productCategoryReopsitory.GetCategories();
            ViewBag.Categories = new SelectList(categories, "Id", "CategoryName");

            List<ProductSupplier> productSuppliers = _productSupplierRepository.GetSuppliers();
            ViewBag.Suppliers = new SelectList(productSuppliers, "Id", "SupplierName");

            return View();
        }

        [HttpPost]
        public IActionResult Add(ProductAddVm productAddVm) 
        {
            ProductAdd productAdd = _imapper.Map<ProductAddVm, ProductAdd>(productAddVm);
            int affrectedRowCount = _productRepository.Add(productAdd);
            if (affrectedRowCount > 0) 
            {
                TempData["SuccessMessageForAdd"] = "Product Add Successful ";
            }
            return RedirectToAction("Index");
        }

        private List<ProductBrandVm> GetBrands() 
        {
            List<ProductBrand> productBrands = _prductBrandRepository.GetBrands();
            List<ProductBrandVm> productBrandsVm = _imapper.Map<List<ProductBrand>, List<ProductBrandVm>>(productBrands);
            return productBrandsVm;
        }

        private List<ProductCategoryVm> GetCategories() 
        {
            List<ProductCategory> productCategories = _productCategoryReopsitory.GetCategories();
            List<ProductCategoryVm> ProductCategoriesVm = _imapper.Map<List<ProductCategory>, List<ProductCategoryVm>>(productCategories);
            return ProductCategoriesVm;
        }

        private List<ProductSupplierVm> GetSupplier() 
        {
            List<ProductSupplier> productSuppliers = _productSupplierRepository.GetSuppliers();
            List<ProductSupplierVm> productSuppliersVm = _imapper.Map<List<ProductSupplier>, List<ProductSupplierVm>>(productSuppliers);
            return productSuppliersVm;
        }
    }
}
