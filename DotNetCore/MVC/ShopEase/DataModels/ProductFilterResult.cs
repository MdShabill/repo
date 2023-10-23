namespace ShopEase.DataModels
{
    public class ProductFilterResult
    {
        public string ProductName { get; set; }
        public int BrandId { get; set; }
        public int Min { get; set; }
        public int Max { get; set; }
        public int CategoryId { get; set; }
    }
}
