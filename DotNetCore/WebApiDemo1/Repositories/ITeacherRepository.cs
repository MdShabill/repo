using Microsoft.AspNetCore.Mvc;
using System.Data;
using WebApiDemo1.DTO;
using WebApiDemo1.DTO.InputDTO;
using WebApplication1.DTO.InputDTO;

namespace WebApiDemo1.Repositories
{
    public interface ITeacherRepository
    {
        public List<TeacherDto> GetAllTeachersAsList();
        public int GetTeachersCount();
        public TeacherDto GetTeacherDetailById(int id);
        public List<TeacherDto> GetTeachersByDepartmentByTeacherName(string fullName, string? department);
        public List<TeacherDto> GetTeachersBySalaryRange(int minimumSalary, int maximumSalary);
        public int Add(TeacherDto teacher);
        public void Update(TeacherDto teacher);
    }
}
