using System.Collections;
using System.Data;
using WebApiDemo1.DataModel;
using WebApiDemo1.DTO.InputDTO;

namespace WebApiDemo1.Repositories
{
    public interface ICustomerRepository
    {
        public List<Customer> GetAllCustomersAsList();
        public int GetCustomersCount();
        public Customer GetCustomerById(int id);
        public string GetCustomerFullNameById(int customerId);
        public List<Customer> GetCustomersDetailByGenderByCountry(int gender, string country);
        public CustomerDto GetCustomerDetailsByEmailAndPassword(string email, byte[] password);        
        public void UpdateOnLoginSuccessfull(string email);
        public void UpdateOnLoginFailed(string email);
        public int GetLoginFailedCount(string email);
        public void UpdateIsLocked(string email, bool isLocked = true);
        public void UpdateNewPassword(string email, byte[] password);
        public int Add(Customer customer);
        public void Update(Customer customer);
    }
}
