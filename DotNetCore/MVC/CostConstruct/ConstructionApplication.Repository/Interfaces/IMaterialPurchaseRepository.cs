using ConstructionApplication.Core.DataModels.MaterialPurchase;

namespace ConstructionApplication.Repository.Interfaces
{
    public interface IMaterialPurchaseRepository
    {
        public List<MaterialPurchase> GetAll(int siteId, DateTime? DateFrom, DateTime? DateTo, int? MaterialId, int? SupplierId, int? BrandId);
        public int Create(MaterialPurchase materialPurchase);
        public void Delete(int id);
    }
}
