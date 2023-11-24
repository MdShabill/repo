namespace ShopEase.ViewModels
{
    public class ProductSearchResultVm
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public string CategoryName { get; set; }
        public int BrandId { get; set; }
        public string BrandName { get; set; }
        public decimal Price { get; set; }
        public decimal ActualPrice { get; set; }
    }
}
