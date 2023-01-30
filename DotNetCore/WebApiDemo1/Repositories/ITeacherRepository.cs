using Microsoft.AspNetCore.Mvc;
using System.Data;
using WebApiDemo1.DTO;
using WebApplication1.DTO.InputDTO;

namespace WebApiDemo1.Repositories
{
    public interface ITeacherRepository
    {
        public DataTable GetAllTeachers();
        public int GetTeachersCount();
        public DataTable GetTeachersDetailById(int teacherId);
        public DataTable GetTeachersByDepartmentByTeacherName(string teacherName, string? department);
        public DataTable GetTeachersBySalaryRange(int minimumSalary, int maximumSalary);
        public int TeacherAdd(TeacherDto teacher);
        public void TeacherUpdate(TeacherDto teacher);
    }
}
