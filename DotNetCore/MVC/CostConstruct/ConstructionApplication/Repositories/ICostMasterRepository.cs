using ConstructionApplication.DataModels.CostMaster;

namespace ConstructionApplication.Repositories
{
    public interface ICostMasterRepository
    {
        public int Create(CostMaster costMaster);
    }
}
