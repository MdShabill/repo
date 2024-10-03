using ConstructionApplication.DataModels.MaterialPurchase;

namespace ConstructionApplication.Repositories
{
    public interface IMaterialPurchaseRepository
    {
        public List<MaterialPurchase> GetAll(DateTime? DateFrom, DateTime? DateTo, int? MaterialId, int? SupplierId, int? BrandId);
        public int Create(MaterialPurchase materialPurchase);
    }
}
