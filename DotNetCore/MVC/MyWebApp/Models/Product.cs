namespace MyWebApp.Models
{
    public class Product
    {
        public int Id { get; set; }

        public string ProductName;
        public string BrandName;


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
