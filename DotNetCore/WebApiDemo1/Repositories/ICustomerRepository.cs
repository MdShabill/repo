using System.Collections;
using System.Data;
using WebApiDemo1.DTO.InputDTO;
using WebApplication1.DTO.InputDTO;

namespace WebApiDemo1.Repositories
{
    public interface ICustomerRepository
    {
        public List<CustomerDto> GetAllCustomersAsList();
        public int GetCustomersCount();
        public CustomerDto GetAllCustomerById(int id);
        public string GetCustomerFullNameById(int customerId);
        public List<CustomerDto> GetCustomersDetailByGenderByCountry(int gender, string country);
        public CustomerDto GetCustomerDetailsByEmailAndPassword(string email, byte[] password);        
        public void UpdateOnLoginSuccessfull(string email);
        public void UpdateOnLoginFailed(string email);
        public int GetLoginFailedCount(string email);
        public void UpdateIsLocked(string email, bool isLocked = true);
        public int Add(CustomerDto customer);
        public void Update(CustomerDto customer);
    }
}
