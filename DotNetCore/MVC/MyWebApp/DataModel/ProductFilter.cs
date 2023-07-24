using MyWebApp.Enums;

namespace MyWebApp.DataModel
{
    public class ProductFilter
    {
        public string ProductName { get; set; }
        
        public int SizeId { get; set; }
        public int ColorId { get; set; }
        
        public int Min { get; set; }
        public int Max { get; set; }
    }
}
