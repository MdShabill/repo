using System.Data;
using WebApiDemo1.DTO.InputDTO;
using WebApplication1.DTO.InputDTO;

namespace WebApiDemo1.Repositories
{
    public interface IEmployeeRepository
    {
        public DataTable GetAllEmployees();
        public int GetEmployeesCount();
        public string GetEmployeesFullNameById(int employeeId);
        public DataTable GetEmployeesDetailByGenderBySalary(string gender, int salary);
        public DataTable GetEmployeesBySalaryRange(int minimumSalary, int maximumSalary);
        public int Add(EmployeeDto employee);
        public void Update(EmployeeDto employee);
    }
}
