using ConstructionApplication.Core.DataModels.CostMaster;

namespace ConstructionApplication.Repository.Interfaces
{
    public interface ICostMasterRepository
    {
        public List<CostMaster> GetByServiceType(int serviceTypeId);
        public CostMaster GetActiveCostDetail(int serviceTypeId);
        public int Create(CostMaster costMaster);
    }
}
