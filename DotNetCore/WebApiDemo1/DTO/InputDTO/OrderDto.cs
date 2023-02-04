using WebApiDemo1.Enums;

namespace WebApiDemo1.DTO.InputDTO
{
    public class OrderDto
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string OrderDate { get; set; }
        public int TotalAmount { get; set; }
        public ProductType ProductName { get; set; }
    }
}
