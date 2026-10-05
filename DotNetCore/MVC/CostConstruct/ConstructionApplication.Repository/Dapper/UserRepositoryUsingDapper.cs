using ConstructionApplication.Core.DataModels.Usres;
using ConstructionApplication.Repository.Interfaces;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace ConstructionApplication.Repository.Dapper
{
    public class UserRepositoryUsingDapper : IUserRepository
    {
        private readonly string _connectionString;
        private readonly ILogger<UserRepositoryUsingDapper> _logger;

        public UserRepositoryUsingDapper(string connectionString,
               ILogger<UserRepositoryUsingDapper> logger)
        {
            _connectionString = connectionString;
            _logger = logger;
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

            Stopwatch dbTimer = Stopwatch.StartNew();

            try
            {
                User user = db.QueryFirstOrDefault<User>(query, new { email });

                dbTimer.Stop();

                _logger.LogInformation("\nDatabase: SELECT User | Time: {Time} ms", dbTimer.ElapsedMilliseconds);

                return user;
            }
            catch
            {
                dbTimer.Stop();

                _logger.LogError(
                    "\nDatabase: SELECT User | Failed | Time: {Time} ms",
                    dbTimer.ElapsedMilliseconds);

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

            Stopwatch dbTimer = Stopwatch.StartNew();

            try
            {
                db.Execute(query, new { email });

                dbTimer.Stop();

                _logger.LogInformation("\nDatabase: UPDATE Login Success | Time: {Time} ms",
                    dbTimer.ElapsedMilliseconds);
            }
            catch
            {
                dbTimer.Stop();

                _logger.LogError("\nDatabase: UPDATE Login Success | Failed | Time: {Time} ms",
                    dbTimer.ElapsedMilliseconds);

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

                    Stopwatch dbStopwatch = Stopwatch.StartNew();

                    db.Execute(query, new { Email = email });

                    dbStopwatch.Stop();

                    _logger.LogInformation("\nDatabase: UPDATE Login Failed | Time: {ElapsedMs} ms",
                        dbStopwatch.ElapsedMilliseconds);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Database: UPDATE Login Failed | Failed");
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

                    Stopwatch dbStopwatch = Stopwatch.StartNew();

                    db.Execute(query, new {email, isLocked});
                    dbStopwatch.Stop();

                    _logger.LogInformation("\nDatabase: UPDATE User Lock | Time: {ElapsedMs} ms",
                        dbStopwatch.ElapsedMilliseconds);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,"Database: UPDATE User Lock | Failed");
                throw;
            }
        }
    }
}