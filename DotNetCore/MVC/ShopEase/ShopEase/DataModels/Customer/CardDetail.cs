namespace ShopEase.DataModels
{
    public class CardDetail
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int CustomerId { get; set; }
        public string FullName { get; set; }
        public string CardNumber { get; set; }
        public DateTime ExpiryDate { get; set; }
        public int ExpiryMonth { get; set; }
        public int ExpiryYear {  get; set; }
        public int CVV { get; set; }
    }
}
