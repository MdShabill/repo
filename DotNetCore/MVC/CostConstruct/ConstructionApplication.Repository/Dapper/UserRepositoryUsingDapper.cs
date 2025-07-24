using ConstructionApplication.Core.DataModels.Usres;
using ConstructionApplication.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace ConstructionApplication.Repository.Dapper
{
    public class UserRepositoryUsingDapper : IUserRepository
    {
        private readonly string _connectionString;

        public UserRepositoryUsingDapper(string connectionString)
        {
            _connectionString = connectionString;
        }

        public User GetUserDetailByEmail(string email)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                string query = @"SELECT TOP 1
                                    Id, Name, Gender, Email, Password,
                                    Mobile, LoginFailedCount, IsLocked
                                 FROM Users 
                                 WHERE Email = @email
                                 ORDER BY Id DESC";

                return db.QueryFirstOrDefault<User>(query, new { email });
            }
        }

        public void UpdateOnLoginSuccessful(string email)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                string query = @"UPDATE Users 
                                 SET 
                                 LastSuccessFulLoginDate = GETDATE(), 
                                 LoginFailedCount = 0 
                                 WHERE Email = @email";

                db.Execute(query, new { email });
            }
        }

        public void UpdateOnLoginFailed(string email)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                string query = @"UPDATE Users
                                 SET
                                 LoginFailedCount = ISNULL(LoginFailedCount, 0) + 1,
                                 LastFailedLoginDate = GETDATE()
                                 WHERE Email = @Email";

                db.Execute(query, new { Email = email });
            }
        }

        public void UpdateIsLocked(string email, bool isLocked)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                string query = @"UPDATE Users 
                                 SET IsLocked = @isLocked 
                                 WHERE Email = @email";

                db.Execute(query, new { email, isLocked });
            }
        }
    }
}
