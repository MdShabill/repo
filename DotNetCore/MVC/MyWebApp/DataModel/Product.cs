using MyWebApp.Enums;

namespace MyWebApp.DataModel
{
    public class Product
    {
        public int Id { get; set; }

        public string ProductName { get; set; }
        public string BrandName { get; set; }

        public int SizeId { get; set; }
        public string SizeName { get; set; }

        public int ColorId { get; set; }
        public string ColorName { get; set; }

        public FitType Fit { get; set; }

        public int FabricId { get; set; }
        public string FabricName { get; set; }

        public int CategoryId { get; set; }
        public string CategoryName { get; set; }

        public int Discount { get; set; }

        public int Price { get; set; }
    }
}
