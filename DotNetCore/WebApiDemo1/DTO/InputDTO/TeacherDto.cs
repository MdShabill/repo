﻿using WebApiDemo1.Enums;

namespace WebApiDemo1.DTO.InputDTO
{
    public class TeacherDto
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public int Age { get; set; }
        public GenderTypes Gender { get; set; }
        public string Email { get; set; }
        public string MobileNumber { get; set; }
        public string SchoolName { get; set; }
        public string Department { get; set; }
        public int Salary { get; set; }
    }
}
