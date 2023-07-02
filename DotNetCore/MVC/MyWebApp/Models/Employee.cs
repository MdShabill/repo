using Microsoft.Build.Framework;

namespace MyWebApp.Models
{
    public class Employee
    {
        public int Id { get; set; }
        
        public string FullName { get; set; }
        public string FatherName { get; set; }
        public string Email { get; set; }
        public int CountryId { get; set; }

        public int QualificationId { get; set; }
        public string QualificationName { get; set; }
    }
}
