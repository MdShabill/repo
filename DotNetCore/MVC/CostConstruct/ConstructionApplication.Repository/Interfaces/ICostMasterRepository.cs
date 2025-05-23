﻿using ConstructionApplication.Core.DataModels.CostMaster;

namespace ConstructionApplication.Repository.Interfaces
{
    public interface ICostMasterRepository
    {
        public List<CostMaster> GetByJobCategory(int jobCategoryId);
        public CostMaster GetActiveCostDetail(int jobCategoryId);
        public int Create(CostMaster costMaster);
    }
}
