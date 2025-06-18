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
                string sqlQuery = @"Select Id, Name, Gender, Email, Password,
                                    MobileNumber, LoginFailedCount, IsLocked
                                    From 
                                    Users 
                                    Where Email = @email ";

                SqlDataAdapter sqlDataAdapter = new(sqlQuery, sqlConnection);
                sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@email", email);
                DataTable dataTable = new();
                sqlDataAdapter.Fill(dataTable);

                if (dataTable.Rows.Count > 0)
                {
                    User users = new()
                    {
                        Id = (int)dataTable.Rows[0]["Id"],
                        Name = (string)dataTable.Rows[0]["Name"],
                        Gender = (GenderTypes)dataTable.Rows[0]["Gender"],
                        Email = (string)dataTable.Rows[0]["Email"],
                        Password = dataTable.Rows[0]["Password"] != DBNull.Value ? (string)dataTable.Rows[0]["Password"] : null,
                        MobileNumber = (string)dataTable.Rows[0]["MobileNumber"],
                        LoginFailedCount = dataTable.Rows[0]["LoginFailedCount"] != DBNull.Value ? (int)dataTable.Rows[0]["LoginFailedCount"] : null,
                        IsLocked = dataTable.Rows[0]["IsLocked"] != DBNull.Value ? (bool)dataTable.Rows[0]["IsLocked"] : false
                    };
                    return users;
                }
                return null;
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
