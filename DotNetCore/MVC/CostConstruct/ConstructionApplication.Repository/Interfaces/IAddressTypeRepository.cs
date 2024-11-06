using ConstructionApplication.Core.DataModels.AddressType;
using System.Reflection.Metadata.Ecma335;

namespace ConstructionApplication.Repository.Interfaces
{
    public interface IAddressTypeRepository
    {
        public List<AddressType> GetAll();
    }
}
