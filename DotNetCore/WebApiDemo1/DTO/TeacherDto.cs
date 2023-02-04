using WebApiDemo1.Enums;

namespace WebApiDemo1.DTO
{
    public class TeacherDto
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public int Age { get; set; }
        public GenderType Gender { get; set; }
        public string SchoolName { get; set; }
        public string Department { get; set; }
        public int Salary { get; set; }
    }
}
