using Microsoft.AspNetCore.Mvc;
using System.Data;
using WebApiDemo1.DTO.InputDTO;
using WebApplication1.DTO.InputDTO;

namespace WebApiDemo1.Repositories
{
    public interface IProductRepository
    {
        public List<ProductDto> GetAllProductAsList();
        public int GetProductsCount();
        public string GetProductDetailByBrandNameById(int productId);
        public List<ProductDto> GetProductsDetailByBrandNameByProductName(string brandName, string? productName);
        public List<ProductDto> GetProductsDetailByBrandNameByPrice(string brandName, int price);
        public List<ProductDto> GetProductsByPriceRange(int minimumPrice, int maximumPrice);
        public int ProductAdd(ProductDto product);
        public void Update(ProductDto product);
    }
}
