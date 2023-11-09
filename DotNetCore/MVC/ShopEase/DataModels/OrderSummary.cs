namespace ShopEase.DataModels
{
    public class OrderSummary
    {
        public int OrderNumber { get; set; }
        public DateTime OrderDate { get; set; }
        public int Price { get; set; }
        public string FullName { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public int PinCode { get; set; }
        public string AddressTypeName { get; set; }
        public string CountryName { get; set; }
        public string ImageName { get; set; }
    }
}
