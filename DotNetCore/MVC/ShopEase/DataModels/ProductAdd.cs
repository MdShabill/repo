namespace ShopEase.DataModels
{
    public class ProductAdd
    {
        public string ProductName { get; set; }
        public int BrandId { get; set; }
        public decimal Price { get; set; }
        public decimal Discount { get; set; }
        public int CategoryId { get; set; }
        public int SupplierId { get; set; }
        public string ImageName { get; set; }
    }
}
