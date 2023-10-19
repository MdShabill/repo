namespace ShopEase.ViewModels
{
    public class ProductVm
    {
        public string ProductName { get; set; }
        public string BrandName { get; set; }
        public decimal Price { get; set; }
        public decimal Discount { get; set; }
        public decimal ActualPrice { get; set; }
        //public decimal ActualPrice
        //{
        //    get 
        //    { 
        //        return Price - Discount; 
        //    }
        //}
        public string CategoryName { get; set; }
        public string SupplierName { get; set; }
    }
}
