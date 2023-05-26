using WebApiDemo1.Enums;

namespace WebApiDemo1.DTO.OutPutDTO
{
    public class ProductOutputDto
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public string BrandName { get; set; }
        public int Size { get; set; }
        public ColorType Color { get; set; }
        public string Fit { get; set; }
        public string Fabric { get; set; }
        public string Category { get; set; }
        public int Discount { get; set; }
        public int Price { get; set; }
    }
}
