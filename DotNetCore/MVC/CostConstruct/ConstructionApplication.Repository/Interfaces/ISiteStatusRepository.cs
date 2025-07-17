using ConstructionApplication.Core.DataModels.SiteStatus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConstructionApplication.Repository.Interfaces
{
    public interface ISiteStatusRepository
    {
        public List<SiteStatus> GetAll();
    }
}
