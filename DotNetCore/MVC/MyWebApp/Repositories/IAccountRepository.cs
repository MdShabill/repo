using MyWebApp.DataModel;

namespace MyWebApp.Repositories
{
    public interface IAccountRepository
    {
        public Customer GetCustomerDetailsByEmailAndPassword(string email, string password);
    }
}
