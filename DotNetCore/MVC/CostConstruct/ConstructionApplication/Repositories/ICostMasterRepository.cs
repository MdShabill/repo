﻿using ConstructionApplication.DataModels.CostMaster;

namespace ConstructionApplication.Repositories
{
    public interface ICostMasterRepository
    {
        public List<CostMaster> GetAll();
        public CostMaster GetActiveCostDetail(int JobCategoryId);
        public int Create(CostMaster costMaster);
    }
}
