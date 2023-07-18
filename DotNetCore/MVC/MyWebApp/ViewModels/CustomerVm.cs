using MyWebApp.Enums;

namespace MyWebApp.ViewModels
{
    public class CustomerVm
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public GenderType Gender { get; set; }
        //public string Gender { get; set; }
        public string Email { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Mobile { get; set; }
    }
}

