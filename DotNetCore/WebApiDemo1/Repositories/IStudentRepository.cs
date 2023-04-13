using System.Data;
using WebApiDemo1.DTO.InputDTO;

namespace WebApiDemo1.Repositories
{
    public interface IStudentRepository
    {
        public DataTable GetAllStudents();
        public DataTable GetStudentCount();
        public void DeleteStudent(int id);
        public int Add(StudentDto studentDto);
        public void Update(StudentDto studentDto);
    }
}
