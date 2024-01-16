namespace ShopEase.ViewModels
{
    public class OrderVm
    {
        public int Id { get; set; } 
        public int OrderId { get; set; }
        public int OrderNumber { get; set; }
        public int ProductId { get; set; }
        public int CustomerId { get; set; }
        public string Title { get; set; }
        public string FullName { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string Address { get; set; }
        public int AddressId { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public int PinCode { get; set; }
        public string CountryName { get; set; }
        public string AddressTypeName { get; set; }
        public string CardNumber { get; set; }
        public DateTime ExpiryDate { get; set;}
        public int ExpiryMonth { get; set; }
        public int ExpiryYear { get; set; }
        public int CVV { get; set; }
    }
}
