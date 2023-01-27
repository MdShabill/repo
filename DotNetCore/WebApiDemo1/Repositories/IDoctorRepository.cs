using System.Data;
using WebApiDemo1.DTO.InputDTO;
using WebApplication1.DTO.InputDTO;

namespace WebApiDemo1.Repositories
{
    public interface IDoctorRepository
    {
        public DataTable GetAllDoctors();
        public int GetDoctorsCount();
        public string GetDoctorDepartmentById(int doctorId);
        public DataTable GetDoctorsDetailByFullNameByDepartment(string fullName, string department);
        public DataTable GetDoctorsDetailByDepartmentByCity(string department, string? city);
        public int Add(DoctorDto doctor);
        public void Update(DoctorDto doctor);
    }
}
