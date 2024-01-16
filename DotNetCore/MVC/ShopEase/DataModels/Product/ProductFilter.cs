namespace ShopEase.DataModels.Product
{
    public class ProductFilter
    {
        public string Title { get; set; }
        public int CategoryId { get; set; }
        public int BrandId { get; set; }
        public int Min { get; set; }
        public int Max { get; set; }
    }
}
