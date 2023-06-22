using MyWebApp.Models;

namespace MyWebApp.Repositories
{
    public interface ICustomerRepository
    {
        public List<Customer> GetAll();
        public Customer Get(int id);
        public void Delete(int id);
        public void Register(Customer customer);
        public void Update(Customer customer);
    }
}
