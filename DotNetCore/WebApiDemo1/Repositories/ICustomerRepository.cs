using System.Data;
using WebApplication1.DTO.InputDTO;

namespace WebApiDemo1.Repositories
{
    public interface ICustomerRepository
    {
        public DataTable GetAllCustomers();
        public int GetCustomersCount();
        public DataTable GetCustomerDetailById(int customerId);
        public string GetCustomerFullNameById(int customerId);
        public DataTable GetCustomersDetailByGenderByCountry(string gender, string country);
        public int Add(CustomerDto customer);
        public void Update(CustomerDto customer);
    }
}
