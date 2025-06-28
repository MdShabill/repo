using ConstructionApplication.Core.DataModels.Usres;
using ConstructionApplication.Core.Enums;
using ConstructionApplication.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConstructionApplication.Repository.AdoDotNet
{
    public class UserRepository : IUserRepository
    {
        private readonly string _connectionString;

        public UserRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public User GetUserDetailByEmail(string email)
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                string sqlQuery = @"SELECT TOP 1
                            Id, Name, Gender, Email, Password,
                            Mobile, LoginFailedCount, IsLocked
                            FROM Users 
                            WHERE Email = @email
                            ORDER BY Id DESC";

                SqlCommand sqlCommand = new(sqlQuery, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@email", email);

                sqlConnection.Open();
                SqlDataReader reader = sqlCommand.ExecuteReader();

                User user = null;

                if (reader.Read())
                {
                    user = new User
                    {
                        Id = (int)reader["Id"],
                        Name = reader["Name"] != DBNull.Value ? (string)reader["Name"] : null,
                        Gender = (GenderTypes)reader["Gender"],
                        Email = reader["Email"] != DBNull.Value ? (string)reader["Email"] : null,
                        Password = reader["Password"] != DBNull.Value ? (string)reader["Password"] : null,
                        Mobile = reader["Mobile"] != DBNull.Value ? (string)reader["Mobile"] : null,
                        LoginFailedCount = reader["LoginFailedCount"] != DBNull.Value ? (int?)reader["LoginFailedCount"] : null,
                        IsLocked = reader["IsLocked"] != DBNull.Value ? (bool)reader["IsLocked"] : false
                    };
                }

                reader.Close();
                return user;
            }
        }

        public void UpdateOnLoginSuccessful(string email)
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                string sqlQuery = @"UPDATE Users 
                                  SET 
                                  LastSuccessFulLoginDate = getdate(), 
                                  LoginFailedCount = 0
                                  WHERE Email = @email";
                SqlCommand sqlCommand = new(sqlQuery, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@email", email);
                sqlConnection.Open();
                sqlCommand.ExecuteNonQuery();
                sqlConnection.Close();
            }
        }

        public void UpdateOnLoginFailed(string email)
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                string sqlQuery = @"UPDATE Users
                                    SET
                                    LoginFailedCount = ISNULL(LoginFailedCount, 0) + 1,
                                    LastFailedLoginDate = GETDATE()
                                    WHERE Email = @email ";
                SqlCommand sqlCommand = new(sqlQuery, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@Email", email);
                sqlConnection.Open();
                sqlCommand.ExecuteNonQuery();
                sqlConnection.Close();
            }
        }

        public void UpdateIsLocked(string email, bool isLocked)
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                string sqlQuery = @"Update Users 
                                    Set 
                                    Islocked = @isLocked
                                    Where Email = @email";
                SqlCommand sqlCommand = new(sqlQuery, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@email", email);
                sqlCommand.Parameters.AddWithValue("@isLocked", isLocked);

                sqlConnection.Open();
                sqlCommand.ExecuteNonQuery();
                sqlConnection.Close();
            }
        }
    }
}
