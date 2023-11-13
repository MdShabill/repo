namespace ShopEase.DataModels
{
    public class Order
    {
        public int Id { get; set; }
        public int OrderNumber { get; set; }
        public int ProductId { get; set; }
        public int CustomerId { get; set; }
        public string ProductName { get; set; }
        public string FullName { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public int AddressId { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set;}
        public int PinCode { get; set;}
        public string CountryName { get; set;}
        public string AddressTypeName { get; set;}
    }
}
