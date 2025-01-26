using ConstructionApplication.Core.DataModels.Contractor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConstructionApplication.Repository.Interfaces
{
    public interface IContractorCRUD
    {
        public List<Contractor> GetAll(int? jobCategoryId, int? id);
    }
}
