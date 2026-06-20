using ConstructionApplication.Core.DataModels.CostMaster;

namespace ConstructionApplication.Repository.Interfaces
{
    public interface ICostMasterRepository
    {
        public List<CostMaster> GetByServiceType(int serviceTypeId, int siteId);
        public CostMaster GetActiveCostDetail(int serviceTypeId, int siteId);
        public CostMaster GetById(int id, int siteId);
        public int Create(CostMaster costMaster);
        public int Update(CostMaster costMaster);
        public void Delete(int id, int siteId);
    }
}