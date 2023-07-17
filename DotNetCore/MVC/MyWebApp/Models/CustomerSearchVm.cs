using MyWebApp.Enums;

namespace MyWebApp.Models
{
    public class CustomerSearchVm
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public GenderType Gender { get; set; }
    }
}
