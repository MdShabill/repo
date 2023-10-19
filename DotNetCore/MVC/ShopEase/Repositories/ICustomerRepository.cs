using ShopEase.DataModels;

namespace ShopEase.Repositories
{
    public interface ICustomerRepository
    {
        public List<Customer> GetAll();
        public int Register(Customer customer);
    }
}
