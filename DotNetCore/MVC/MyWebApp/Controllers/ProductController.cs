using Microsoft.AspNetCore.Mvc;
using MyWebApp.Models;
using Microsoft.Data.SqlClient;
using System.Data;
using Microsoft.CodeAnalysis;
using Microsoft.AspNetCore.Mvc.Rendering;
using MyWebApp.Repositories;

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
            List<ProductColor> productColors = GetColors();
            ViewBag.ProductColors = new SelectList(productColors, "Id", "ColorName");

            List<ProductSizes> productSizes = GetSizes();
            ViewBag.productSizes = new SelectList(productSizes, "Id", "Size");

            return View("Add");
        }

        [HttpPost]
        public IActionResult Add(Product product)
        {
            _productRepository.Add(product);
            return View("AddSuccess");
        }

        public IActionResult Update()
        {
            return View("Update");
        }

        [HttpPost]
        public IActionResult Update(Product product)
        {
            _productRepository.Update(product);
            return View("UpdateSuccess");
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
    }
}
