using ConstructionApplication.Core.DataModels.Usres;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConstructionApplication.Repository.Interfaces
{
    public interface IUserRepository
    {
        public User GetUserDetailByEmail(string email);
        public void UpdateOnLoginSuccessful(string email);
        public void UpdateOnLoginFailed(string email);
        public void UpdateIsLocked(string email, bool isLocked = true);
    }
}
