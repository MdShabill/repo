namespace ShopEase.DataModels.OderItem
{
    public class OrderItem
    {
        public int Id { get; set; }
        //public int OrderId { get; set; }
        //public string OrderNumber { get; set; }
        public int CustomerId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
