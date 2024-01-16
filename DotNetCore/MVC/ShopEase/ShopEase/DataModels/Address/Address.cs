namespace ShopEase.DataModels.Address
{
    public class Address
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public string AddressDetail { get; set; }
        public int CustomerId { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public int PinCode { get; set; }
        public int CountryId { get; set; }
        public string CountryName { get; set; }
        public int AddressTypeId { get; set; }
        public string AddressTypeName { get; set; }
    }
}
