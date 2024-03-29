﻿using System.Data;
using WebApiDemo1.DTO.InputDTO;

namespace WebApiDemo1.Repositories
{
    public interface IStudentRepository
    {
        public StudentDto GetById(int id);
        public List<StudentDto> GetAllStudents();
        public int GetStudentCount();
        public string GetFullNameById(int id);
        public void Delete(int id);
        public int Add(StudentDto studentDto);
        public void Update(StudentDto studentDto);
    }
}
