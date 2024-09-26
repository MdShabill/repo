using ConstructionApplication.DataModels.MaterialPurchase;

namespace ConstructionApplication.Repositories
{
    public interface IMaterialPurchaseRepository
    {
        public List<MaterialPurchase> GetAll();
        public int Create(MaterialPurchase materialPurchase);
    }
}
