using WebApiDemo1.Enums;

namespace WebApplication1.DTO.InputDTO
{
    public class CustomerDto
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public GenderTypes Gender { get; set; }
        public int Age { get; set; }
        public string Email { get; set; }
        public string Country { get; set; }
        public string Password { get; set; }
        public string MobileNumber { get; set; }
        public DateTime LastFailedLoginDate { get; set; }
        public DateTime LastSucccessfulLoginDate { get; set; }
        public int LoginFailedCount { get; set; }
        public bool IsLocked { get; set; }
        
    }
}