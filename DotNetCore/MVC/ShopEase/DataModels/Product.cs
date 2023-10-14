namespace ShopEase.DataModels
{
    public class Product
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public int BrandId { get; set; }
        public string BrandName { get; set; }
        public decimal Price { get; set; }
        public decimal Discount { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int SupplierId { get; set; }
        public string SupplierName { get; set; }
    }
}
