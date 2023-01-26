using Microsoft.AspNetCore.Mvc;
using System.Data;
using WebApiDemo1.DTO.InputDTO;
using WebApplication1.DTO.InputDTO;

namespace WebApiDemo1.Repositories
{
    public interface IProductRepository
    {
        public DataTable GetAllProducts();
        public int GetProductsCount();
        public string GetProductDetailByBaradNameById(int productId);
        public DataTable GetProductsDetailByBrandNameByProductName(string brandName, string? productName);
        public DataTable GetProductsDetailByBrandNameByPriceUpto(string brandName, int priceUpto);
        public DataTable GetProductsByPriceRange(int minimumPrice, int maximumPrice);
        public int ProductAdd(ProductDto product);
        public void Update(ProductDto product);
    }
}
