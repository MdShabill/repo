namespace MyWebApp.Models
{
    public class Product
    {
        public int Id { get; set; }

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

        public int SizeId { get; set; }
        public string SizeName { get; set; }
        
        public int ColorId { get; set; }
        public string ColorName { get; set; }

        //TODO: it should be of enum type
        public string Fit { get; set; }

        //Add a master table for fabric and treat it with dropdown
        public string FabricId { get; set; }
        public string FabricName { get; set; }

        public string CategoryId { get; set; }
        public string CategoryName { get; set; }

        public int Discount { get; set; }
        public int Price { get; set; }
        
    }
}
