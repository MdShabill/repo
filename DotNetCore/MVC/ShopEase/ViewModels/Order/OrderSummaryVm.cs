namespace ShopEase.ViewModels
{
    public class OrderSummaryVm
    {
        public int OrderNumber { get; set; }
        public DateTime OrderDate { get; set; }
        public string ImageName { get; set; }
        public string FullName { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public int PinCode { get; set; }
        public string CountryName { get; set; }
        public string AddressTypeName { get; set; }
        public decimal Price { get; set; }
    }
}
