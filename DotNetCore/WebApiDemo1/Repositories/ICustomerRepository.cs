using System.Data;
using WebApplication1.DTO.InputDTO;

namespace WebApiDemo1.Repositories
{
    public interface ICustomerRepository
    {
        public DataTable GetAllCustomers();
        public int Register(CustomerDto customer);
        
    }
}
