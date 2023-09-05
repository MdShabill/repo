using MyWebApp.ViewModels;

namespace MyWebApp.Repositories
{
    public interface IEmployeeRepository
    {
        public List<Employee> GetAll();
        public int GetAllEmployeesCount();
        public void Add(Employee employee);
        public List<Qualification> GetQualification();
    }
}
