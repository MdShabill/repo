using MyWebApp.Enums;

namespace MyWebApp.DataModel
{
    public class Customer
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public GenderType Gender { get; set; }
        //public string Gender { get; set; }
        public string Email { get; set; }
        public string? Password { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string? Mobile { get; set; }
        public DateTime LastSuccessfulLoginDate { get; set; }
        public DateTime LastFailedLoginDate { get; set; }
        public int? LoginFailedCount { get; set; }
        public bool IsLocked { get; set; }
    }
}
