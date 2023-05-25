using WebApiDemo1.Enums;

namespace WebApiDemo1.DataModel
{
    public class Doctor
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public GenderTypes Gender { get; set; }
        public string Email { get; set; }
        public int RegistrationNumber { get; set; }
        public string Department { get; set; }
        public string City { get; set; }
    }
}
