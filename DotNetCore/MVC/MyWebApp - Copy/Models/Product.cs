namespace MyWebApp.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public string BrandName { get; set; }
        public int SizeId { get; set; }
        public int ColorId { get; set; }
        public string Fit { get; set; }
        public string Fabric { get; set; }
        public string Category { get; set; }
        public int Discount { get; set; }
        public int Price { get; set; }
        public string SizeName { get; set; }
        public string ColorName { get; set; }
    }
}
