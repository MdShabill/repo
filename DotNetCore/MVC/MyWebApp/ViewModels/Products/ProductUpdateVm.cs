using MyWebApp.Enums;

namespace MyWebApp.ViewModels.Products
{
    public class ProductUpdateVm
    {
        public int Id { get; set; }

        public string ProductName { get; set; }

        public string BrandName { get; set; }

        public int MyId { get; set; }

        public int ColorId { get; set; }

        public FitType Fit { get; set; }

        public int FabricId { get; set; }

        public int CategoryId { get; set; }

        public int Discount { get; set; }

        public int Price { get; set; }
    }
}
