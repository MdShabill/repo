﻿using System.Data;
using WebApiDemo1.DTO.InputDTO;

namespace WebApiDemo1.Repositories
{
    public interface IEmployeeRepository
    {
        public List<EmployeeDto> GetAllEmployeesAsList();
        public EmployeeDto GetAllEmployeeById(int id);
        public int GetEmployeesCount();
        public List<EmployeeDto> GetEmployeesDetailByGenderBySalary(int gender, int salary);
        public List<EmployeeDto> GetEmployeesBySalaryRange(int minimumSalary, int maximumSalary);
        public EmployeeDto GetEmployeeDetailsByEmailAndPassword(string email, byte[] password);
        public void UpdateOnLoginSuccessfull(string email);
        public void UpdateOnLoginFailed(string email);
        public int GetLoginFailedCount(string email);
        public void UpdateIsLocked(string email, bool isLocked = true);
        public int Add(EmployeeDto employee);
        public void Update(EmployeeDto employee);
    }
}
