using MyWebApp.Enums;

namespace MyWebApp.ViewModels.Products
{
    public class ProductVm
    {
        public int Id { get; set; }

        public int ProductId  { get; set; }

        public string ProductName { get; set; }

        public string BrandName { get; set; }


        //private string fatherName;
        //public string FatherName
        //{
        //    get { return fatherName.Trim(); }

        //    set
        //    {
        //        if (!string.IsNullOrEmpty(value))
        //        {
        //            fatherName = value.Trim();
        //        }
        //    }
        //}

        public string SizeName { get; set; }

        public string ColorName { get; set; }

        //TODO: it should be of enum type
        public FitType Fit { get; set; }

        //Add a master table for fabric and treat it with dropdown
        public string FabricName { get; set; }

        public string CategoryName { get; set; }

        public int Discount { get; set; }

        public int Price { get; set; }
    }
}
