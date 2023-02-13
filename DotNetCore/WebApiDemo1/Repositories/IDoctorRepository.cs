using System.Data;
using WebApiDemo1.DTO.InputDTO;

namespace WebApiDemo1.Repositories
{
    public interface IDoctorRepository
    {
        public List<DoctorDto> GetAllDoctorsAsList();
        public int GetDoctorsCount();
        public DoctorDto GetDoctorDetailById(int doctorId);
        public List<DoctorDto> GetDoctorsByDepartmentByDoctorName(string department, string doctorName);
        public List<DoctorDto> GetDoctorsNameListByDepartment(string department);
        public int Add(DoctorDto doctor);
        public void Update(DoctorDto doctor);
    }
}
