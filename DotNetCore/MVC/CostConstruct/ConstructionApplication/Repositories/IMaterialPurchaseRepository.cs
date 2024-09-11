using ConstructionApplication.DataModels.MaterialPurchase;

namespace ConstructionApplication.Repositories
{
    public interface IMaterialPurchaseRepository
    {
        public int Create(MaterialPurchase materialPurchase);
    }
}
