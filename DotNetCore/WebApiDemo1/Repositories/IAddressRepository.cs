using WebApiDemo1.DTO.InputDTO;

namespace WebApiDemo1.Repositories
{
    public interface IAddressRepository
    {
        void AddAddress(CustomerDto customer);
    }
}
