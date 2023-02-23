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
        public List<CustomerDto> Login(string email, string password);
        public int Add(CustomerDto customer);
        public void Update(CustomerDto customer);
    }
}
