using WebApiDemo1.DataModel;
using WebApiDemo1.DTO.InputDTO;

namespace WebApiDemo1.Repositories
{
    public interface IAddressRepository
    {
        public void Add(Address address);
        public void Update(Address address);
    }
}
