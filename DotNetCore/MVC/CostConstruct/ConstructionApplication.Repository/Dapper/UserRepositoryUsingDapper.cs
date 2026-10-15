using ConstructionApplication.Repository.Interfaces;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using ConstructionApplication.Core.DataModels;

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
            using var db = new SqlConnection(_connectionString);

            string query = @"SELECT TOP 1
                                Id, Name, Gender, Email, Password,
                                Mobile, LoginFailedCount, IsLocked
                             FROM 
                                Users
                             WHERE 
                                Email = @email
                             ORDER BY Id DESC";
            try
            {
                User user = db.QueryFirstOrDefault<User>(query, new { email });

                return user;
            }
            catch
            {
                throw;
            }
        }

        public void UpdateOnLoginSuccessful(string email)
        {
            using var db = new SqlConnection(_connectionString);

            string query = @"
                        UPDATE Users
                        SET 
                          LastSuccessFulLoginDate = GETDATE(),
                          LoginFailedCount = 0
                        WHERE 
                            Email = @email";
            try
            {
                db.Execute(query, new { email });
            }
            catch
            {
                throw;
            }
        }

        public void UpdateOnLoginFailed(string email)
        {
            try
            {
                using (IDbConnection db = new SqlConnection(_connectionString))
                {
                    string query = @"
                             UPDATE Users
                             SET 
                                LoginFailedCount = ISNULL(LoginFailedCount, 0) + 1,
                                LastFailedLoginDate = GETDATE()
                             WHERE 
                                Email = @Email";

                    db.Execute(query, new { Email = email });
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UpdateIsLocked(string email, bool isLocked)
        {
            try
            {
                using (IDbConnection db = new SqlConnection(_connectionString))
                {
                    string query = @"
                            UPDATE Users
                             SET 
                                IsLocked = @isLocked
                             WHERE 
                                Email = @email";
                    db.Execute(query, new {email, isLocked});
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}