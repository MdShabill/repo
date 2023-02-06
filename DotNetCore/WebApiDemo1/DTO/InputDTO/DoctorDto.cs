using WebApiDemo1.Enums;

namespace WebApiDemo1.DTO.InputDTO
{
    public class DoctorDto
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Department { get; set; }
        public GenderTypes Gender { get; set; }
        public string City { get; set; }
    }
}
