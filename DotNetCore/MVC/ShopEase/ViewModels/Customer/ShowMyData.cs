using ShopEase.Enums;

namespace ShopEase.ViewModels.Customer
{
    public class ShowMyData
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public GenderType Gender { get; set; }
        public string Email { get; set; }
        public int RegistrationNumber { get; set; }
        public string Department { get; set; }
        public string City { get; set; }
    }
}
