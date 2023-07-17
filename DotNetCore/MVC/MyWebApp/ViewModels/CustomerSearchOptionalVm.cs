using MyWebApp.Enums;

namespace MyWebApp.ViewModels
{
    public class CustomerSearchOptionalVm
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public GenderType Gender { get; set; }
    }
}
