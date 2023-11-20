using ShopEase.Enum;

namespace ShopEase.ViewModels
{
    public class CustomerVm
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Mobile { get; set; }
        public GenderType Gender { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
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
