using System.Data;
using WebApiDemo1.DTO.InputDTO;

namespace WebApiDemo1.Repositories
{
    public interface IStudentRepository
    {
        public StudentDto GetStudentById(int id);
        public List<StudentDto> GetAllStudents();
        public int GetStudentCount();
        public string GetStudentFullNameById(int id);
        public void DeleteStudent(int id);
        public int Add(StudentDto studentDto);
        public void Update(StudentDto studentDto);
    }
}
