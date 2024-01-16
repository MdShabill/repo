namespace ShopEase.DataModels.Product
{
    public class ProductSearchResult
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ImageName { get; set; }
        public string CategoryName { get; set; }
        public int BrandId { get; set; }
        public string BrandName { get; set; }
        public decimal Price { get; set; }
        public decimal ActualPrice { get; set; }
        public int Quantity { get; set; }
    }
}
