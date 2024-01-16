namespace ShopEase.DataModels.Cart
{
    public class Cart
    {
        public int CustomerId { get; set; }
        public int ProductId { get; set; }
        public string Title { get; set; }
        public string ImageName { get; set; }
        public int Quantity { get; set; }
        public DateTime AddDate { get; set; }
        public int Price { get; set; }
    }
}
