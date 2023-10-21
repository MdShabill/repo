using ShopEase.DataModels;

namespace ShopEase.Repositories
{
    public interface ICustomerRepository
    {
        public List<Customer> GetAll();
        public int Register(Customer customer);

        public Customer GetCustomerDetailByEmail(string email);
        public void UpdateOnLoginSuccessfull(string email);
        public void UpdateOnLoginFailed(string email);
        public void UpdateIsLocked(string email, bool isLocked = true);
    }
}
