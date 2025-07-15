using ConstructionApplication.Core.DataModels.Site;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConstructionApplication.Repository.Interfaces
{
    public interface ISiteRepository
    {
        List<Site> GetAllSites();
        public Site GetSiteById(int Id);
        public int Update(Site site);
        public int Create(Site site);
        public void Delete(int id);
    }
}
