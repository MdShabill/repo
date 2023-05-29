using WebApiDemo1.Enums;

namespace WebApiDemo1.DTO.InputDTO
{
    public interface IProductInputDto
    {
        string BrandName { get; set; }
        string Category { get; set; }
        ColorType Color { get; set; }
        int Discount { get; set; }
        string Fabric { get; set; }
        string Fit { get; set; }
        int Id { get; set; }
        int Price { get; set; }
        string ProductName { get; set; }
        int Size { get; set; }
    }
}