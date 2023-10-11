using Microsoft.AspNetCore.Mvc;
using ShopEase.Repositories.Product;
using ShopEase.ViewModels.Product;
using AutoMapper;
using ShopEase.DataModels.Product;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ShopEase.Controllers
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
                cfg.CreateMap<Products, ProductVm>();
                cfg.CreateMap<ProductAddVm, ProductAdd>();
                cfg.CreateMap<Brand, BrandVm>();
                cfg.CreateMap<Category, CategoryVm>();
                cfg.CreateMap<Supplier, SupplierVm>();
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
            List<Brand> productBrands = _productRepository.GetBrands();
            ViewBag.Brands = new SelectList(productBrands, "Id", "BrandName");

            List<Category> categories = _productRepository.GetCategories();
            ViewBag.Categories = new SelectList(categories, "Id", "CategoryName");

            List<Supplier> productSuppliers = _productRepository.GetSuppliers();
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

        private List<BrandVm> GetBrands() 
        {
            List<Brand> productBrands = _productRepository.GetBrands();

            List<BrandVm> productBrandsVm = _imapper.Map<List<Brand>, List<BrandVm>>(productBrands);

            return productBrandsVm;
        }

        private List<CategoryVm> GetCategories() 
        {
            List<Category> categories = _productRepository.GetCategories();

            List<CategoryVm> ProductCategoriesVm = _imapper.Map<List<Category>, List<CategoryVm>>(categories);

            return ProductCategoriesVm;
        }

        private List<SupplierVm> GetSupplier() 
        {
            List<Supplier> productSuppliers = _productRepository.GetSuppliers();

            List<SupplierVm> productSuppliersVm = _imapper.Map<List<Supplier>, List<SupplierVm>>(productSuppliers);

            return productSuppliersVm;
        }
    }
}
