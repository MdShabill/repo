using MyWebApp.Enums;

namespace MyWebApp.ViewModels.Products
{
    public class ProductFilterVm
    {
        public string ProductName { get; set; }
        
        public int SizeId { get; set; }
        public int ColorId { get; set; } 
        public int FabricId { get; set; }
        
        public int Min { get; set; }
        public int Max { get; set; }
    }
}
