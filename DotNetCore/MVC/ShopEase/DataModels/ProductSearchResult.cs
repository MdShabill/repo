namespace ShopEase.DataModels
{
    public class ProductSearchResult
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public string CategoryName { get; set; }
        public int BrandId { get; set; }
        public string BrandName { get; set; }
        public decimal Price { get; set; }
        public decimal ActualPrice { get; set; }
        public int Quantity { get; set; }
    }
}
