using MyWebApp.Models;

namespace MyWebApp.Repositories
{
    public interface IEmployeeRepository
    {
        public List<Employee> GetAll();
        public void Add(Employee employee);
    }
}
