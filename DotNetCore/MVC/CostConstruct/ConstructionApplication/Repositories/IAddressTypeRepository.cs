using ConstructionApplication.DataModels.AddressType;
using System.Reflection.Metadata.Ecma335;

namespace ConstructionApplication.Repositories
{
    public interface IAddressTypeRepository
    {
        public List<AddressType> GetAll();
    }
}
