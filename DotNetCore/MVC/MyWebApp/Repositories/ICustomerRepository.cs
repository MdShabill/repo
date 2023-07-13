using MyWebApp.Enums;
using MyWebApp.Models;

namespace MyWebApp.Repositories
{
    public interface ICustomerRepository
    {
        public List<Customer> GetAll();
        public Customer Get(int id);
        public int Delete(int id);
        public List<Customer> GetCustomers(string firstName, string lastName, int gender);
        public int Register(Customer customer);
        public int Update(Customer customer);
    }
}
