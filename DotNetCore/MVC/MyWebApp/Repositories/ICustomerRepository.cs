using MyWebApp.Models;

namespace MyWebApp.Repositories
{
    public interface ICustomerRepository
    {
        public List<Customer> GetAll();
        public Customer Get(int id);
        public int Delete(int id);
        public int Register(Customer customer);
        public int Update(Customer customer);
    }
}
