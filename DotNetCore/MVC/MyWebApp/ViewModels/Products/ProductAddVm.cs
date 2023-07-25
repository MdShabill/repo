using MyWebApp.Enums;

namespace MyWebApp.ViewModels.Products
{
    public class ProductAddVm
    {
        public string ProductName { get; set; }

        public string BrandName { get; set; }

        public int SizeId { get; set; }
        public int SizeName { get; set; }

        public int ColorId { get; set; }
        public int ColorName { get; set; }

        public FitType Fit { get; set; }

        public int FabricId { get; set; }
        public int FabricName { get; set; }

        public int CategoryId { get; set; }
        public int CategoryName { get; set; }

        public int Discount { get; set; }

        public int Price { get; set; }
    }
}
