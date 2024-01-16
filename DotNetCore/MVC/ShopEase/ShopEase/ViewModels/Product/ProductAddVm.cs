namespace ShopEase.ViewModels
{
    public class ProductAddVm
    {
        public string Title { get; set; }
        public int BrandId { get; set; }
        public decimal Price { get; set; }
        public decimal Discount { get; set; }
        public int CategoryId { get; set; }
        public int SupplierId { get; set; }
        public IFormFile ImageFile { get; set; }
        public string ImageName { get; set; }
        public int Quantity { get; set; }
    }
}
