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
    }
}
