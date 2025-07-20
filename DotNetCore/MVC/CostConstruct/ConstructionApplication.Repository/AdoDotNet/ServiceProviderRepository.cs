using ConstructionApplication.Core.DataModels.ServiceProviders;
using System.Data.SqlClient;
using System.Data;
using ConstructionApplication.Core.Enums;
using ConstructionApplication.Repository.Interfaces;
using System;
using ConstructionApplication.Core.DataModels.ServiceTypes;

namespace ConstructionApplication.Repository.AdoDotNet
{
    public class ServiceProviderRepository : IServiceProviderRepository
    {
        private readonly string _connectionString;

        public ServiceProviderRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<ServiceProvider> GetAll(int? serviceTypeId, int? id)
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                string sqlQuery = @"
                    SELECT 
                        ServiceProviders.Id As ServiceProviderId, ServiceProviders.ServiceTypeId, ServiceTypes.Name As ServiceTypes, 
                        ServiceProviders.Name As ServiceProviderName, ServiceProviders.Gender, ServiceProviders.DOB, 
                        ServiceProviders.MobileNumber, ServiceProviders.ReferredBy, Addresses.AddressLine1, Addresses.AddressTypeId,
                        AddressTypes.Name As AddressTypes, Addresses.CountryId,
	                    Countries.Name As CountryName, Addresses.PinCode
                    FROM ServiceProviders
                    LEFT JOIN ServiceTypes ON ServiceProviders.ServiceTypeId = ServiceTypes.Id
                    LEFT JOIN Addresses ON ServiceProviders.Id = Addresses.ServiceProviderId
                    LEFT JOIN AddressTypes ON Addresses.AddressTypeId = AddressTypes.Id
                    LEFT JOIN Countries ON Addresses.CountryId = Countries.Id
                    WHERE (@serviceTypeId IS NULL OR ServiceTypeId = @serviceTypeId)
                    AND (@id IS NULL OR ServiceProviders.Id = @id) ";

                SqlDataAdapter sqlDataAdapter = new(sqlQuery, sqlConnection);
                sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@serviceTypeId", serviceTypeId ?? (object)DBNull.Value);
                sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@id", id ?? (object)DBNull.Value);
                DataTable dataTable = new();
                sqlDataAdapter.Fill(dataTable);

                List<ServiceProvider> serviceProviders = new();

                //Approach: 1 = With For Loop

                //for (int i = 0; i < dataTable.Rows.Count; i++)
                //{
                //    ServiceProvider serviceProvider = new()
                //    {


                //        ServiceTypeId = (int)dataTable.Rows[i]["ServiceTypeId"],
                //        ServiceTypes = (string)dataTable.Rows[i]["ServiceTypes"],
                //        ServiceProviderId = (int)dataTable.Rows[i]["ServiceProviderId"],
                //        ServiceProviderName = (string)dataTable.Rows[i]["ServiceProviderName"],
                //        Gender = (GenderTypes)dataTable.Rows[i]["Gender"],
                //        DOB = (DateTime)dataTable.Rows[i]["DOB"],
                //        MobileNumber = (string)dataTable.Rows[i]["MobileNumber"],
                //        ReferredBy = dataTable.Rows[i]["ReferredBy"] != DBNull.Value ? (string)dataTable.Rows[i]["ReferredBy"] : null,

                //        AddressLine1 = dataTable.Rows[i]["AddressLine1"] != DBNull.Value ? (string)dataTable.Rows[i]["AddressLine1"] : null,
                //        AddressTypeId = dataTable.Rows[i]["AddressTypeId"] != DBNull.Value ? (int)dataTable.Rows[i]["AddressTypeId"] : 0,
                //        AddressTypes = dataTable.Rows[i]["AddressTypes"] != DBNull.Value ? (string)dataTable.Rows[i]["AddressTypes"] : null,
                //        CountryId = dataTable.Rows[i]["CountryId"] != DBNull.Value ? (int)dataTable.Rows[i]["CountryId"] : 0,
                //        CountryName = dataTable.Rows[i]["CountryName"] != DBNull.Value ? (string)dataTable.Rows[i]["CountryName"] : null,
                //        PinCode = dataTable.Rows[i]["PinCode"] != DBNull.Value ? (int)dataTable.Rows[i]["PinCode"] : 0
                //    };
                //    serviceProviders.Add(serviceProvider);
                //}

                //Approach: 2 = With For Each Loop
                foreach (DataRow row in dataTable.Rows)
                {
                    ServiceProvider serviceProvider = new()
                    {
                        ServiceTypeId = (int)row["ServiceTypeId"],
                        ServiceTypes = (string)row["ServiceTypes"],
                        ServiceProviderId = (int)row["ServiceProviderId"],
                        ServiceProviderName = (string)row["ServiceProviderName"],
                        Gender = (GenderTypes)row["Gender"],
                        DOB = (DateTime)row["DOB"],
                        MobileNumber = (string)row["MobileNumber"],
                        ReferredBy = row["ReferredBy"] != DBNull.Value ? (string)row["ReferredBy"] : null,

                        AddressLine1 = row["AddressLine1"] != DBNull.Value ? (string)row["AddressLine1"] : null,
                        AddressTypeId = row["AddressTypeId"] != DBNull.Value ? (int)row["AddressTypeId"] : 0,
                        AddressTypes = row["AddressTypes"] != DBNull.Value ? (string)row["AddressTypes"] : null,
                        CountryId = row["CountryId"] != DBNull.Value ? (int)row["CountryId"] : 0,
                        CountryName = row["CountryName"] != DBNull.Value ? (string)row["CountryName"] : null,
                        PinCode = row["PinCode"] != DBNull.Value ? (int)row["PinCode"] : 0
                    };
                    serviceProviders.Add(serviceProvider);
                }
                return serviceProviders;
            }
        }

        public int Add(ServiceProvider serviceProvider)
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                string sqlQuery = @"Insert Into ServiceProviders
                       (ServiceTypeId, Name, Gender, DOB, ImageName, MobileNumber, ReferredBy)
                       Values
                       (@serviceTypeId, @name, @gender, @dOB, @imageName, @mobileNumber, @referredBy)
                       Select Scope_Identity()";

                SqlCommand sqlCommand = new(sqlQuery, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@serviceTypeId", serviceProvider.ServiceTypeId);
                sqlCommand.Parameters.AddWithValue("@name", serviceProvider.ServiceProviderName);
                sqlCommand.Parameters.AddWithValue("@gender", serviceProvider.Gender);
                sqlCommand.Parameters.AddWithValue("@dOB", serviceProvider.DOB);
                if (string.IsNullOrEmpty(serviceProvider.ImageName))
                    sqlCommand.Parameters.AddWithValue("@imageName", DBNull.Value);
                else
                    sqlCommand.Parameters.AddWithValue("@imageName", serviceProvider.ImageName);

                sqlCommand.Parameters.AddWithValue("@mobileNumber", serviceProvider.MobileNumber);
                sqlCommand.Parameters.AddWithValue("@referredBy", serviceProvider.ReferredBy);

                sqlConnection.Open();
                serviceProvider.ServiceProviderId = Convert.ToInt32(sqlCommand.ExecuteScalar());
                sqlConnection.Close();

                return serviceProvider.ServiceProviderId;
            }
        }

        public void Delete(int serviceProviderId)
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                string deleteServiceProviderQuery = @"DELETE FROM ServiceProviders WHERE Id = @ServiceProviderId";
                SqlCommand deleteServiceProviderCommand = new(deleteServiceProviderQuery, sqlConnection);
                deleteServiceProviderCommand.Parameters.AddWithValue("@ServiceProviderId", serviceProviderId);
                sqlConnection.Open();
                deleteServiceProviderCommand.ExecuteNonQuery();
                sqlConnection.Close();
            }
        }

        public int Update(ServiceProvider serviceProvider)
        {
            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                string sqlQuery = @"UPDATE ServiceProviders SET
                            ServiceTypeId = @serviceTypeId,
                            Name = @name,
                            Gender = @gender,
                            DOB = @dob,
                            MobileNumber = @mobileNumber,
                            ReferredBy = @referredBy
                            WHERE Id = @id";

                using (SqlCommand sqlCommand = new SqlCommand(sqlQuery, sqlConnection))
                {
                    sqlCommand.Parameters.AddWithValue("@id", serviceProvider.ServiceProviderId);
                    sqlCommand.Parameters.AddWithValue("@serviceTypeId", serviceProvider.ServiceTypeId);
                    sqlCommand.Parameters.AddWithValue("@name", serviceProvider.ServiceProviderName);
                    sqlCommand.Parameters.AddWithValue("@gender", serviceProvider.Gender);
                    sqlCommand.Parameters.AddWithValue("@dob", serviceProvider.DOB);
                    sqlCommand.Parameters.AddWithValue("@mobileNumber", serviceProvider.MobileNumber);
                    sqlCommand.Parameters.AddWithValue("@referredBy", serviceProvider.ReferredBy);

                    sqlConnection.Open();
                    int affectedRowCount = sqlCommand.ExecuteNonQuery();
                    sqlConnection.Close();
                    return affectedRowCount;
                }
            }
        }
    }
}
