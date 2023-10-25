namespace ShopEase.DataModels
{
    public class ProductFilter
    {
        public string ProductName { get; set; }
        public int CategoryId { get; set; }
        public int BrandId { get; set; }
        public int Min { get; set; }
        public int Max { get; set; }
    }
}
