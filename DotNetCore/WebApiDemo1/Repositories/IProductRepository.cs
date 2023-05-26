using Microsoft.AspNetCore.Mvc;
using System.Data;
using WebApiDemo1.DataModel;
using WebApiDemo1.DTO.InputDTO;
using WebApiDemo1.DTO.InputDTO;
using WebApiDemo1.DTO.OutPutDTO;

namespace WebApiDemo1.Repositories
{
    public interface IProductRepository
    {
        public List<ProductInputDto> GetAllProductAsList();
        public int GetProductsCount();
        public string GetProductDetailById(int productId);
        public List<ProductInputDto> GetProductsDetailByBrandNameByProductName(string brandName, string? productName);
        public List<ProductInputDto> GetProductsDetailByBrandNameByPrice(string brandName, int price);
        public List<ProductInputDto> GetProductsByPriceRange(int minimumPrice, int maximumPrice);
        public List<Product> GetFilteredProducts(Product products);
        public List<ProductOutputDto> GetFilteredProducts_1(ProductInputDto productInputDto);
        public int Add(ProductInputDto product);
        public void Update(ProductInputDto product);
    }
}
