using ConstructionApplication.DataModels.CostMaster;

namespace ConstructionApplication.Repositories
{
    public interface ICostMasterRepository
    {
        public CostMaster GetActiveCostDetail();
        public int Create(CostMaster costMaster);
    }
}
