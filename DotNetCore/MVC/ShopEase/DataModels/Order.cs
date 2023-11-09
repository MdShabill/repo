namespace ShopEase.DataModels
{
    public class Order
    {
        public int? OrderNumber { get; set; }
        public int ProductId { get; set; }
        public int CustomerId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public int AddressId { get; set; }
    }
}
