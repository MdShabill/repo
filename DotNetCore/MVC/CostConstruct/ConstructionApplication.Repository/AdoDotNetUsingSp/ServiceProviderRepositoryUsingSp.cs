﻿using ConstructionApplication.Core.DataModels.ServiceProviders;
using ConstructionApplication.Core.Enums;
using ConstructionApplication.Repository.Interfaces;
using System.Data.SqlClient;
using System.Data;

namespace ConstructionApplication.Repository.AdoDotNetUsingSp
{
    public class ServiceProviderRepositoryUsingSp : IServiceProviderRepository
    {
        private readonly string _connectionString;

        public ServiceProviderRepositoryUsingSp(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<ServiceProvider> GetAll(int? serviceTypeId, int? id)
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                string sqlQuery = "SP_ServiceProviderCRUD";

                SqlDataAdapter sqlDataAdapter = new(sqlQuery, sqlConnection)
                {
                    SelectCommand = { CommandType = CommandType.StoredProcedure }
                };
                sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@Mode", 2);
                sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@serviceTypeId", serviceTypeId ?? (object)DBNull.Value);
                sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@ServiceProviderId", id ?? (object)DBNull.Value);
                DataTable dataTable = new();
                sqlDataAdapter.Fill(dataTable);

                List<ServiceProvider> serviceProviders = new();

                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    ServiceProvider serviceProvider = new()
                    {
                        ServiceTypeId = dataTable.Rows[i]["ServiceTypeId"] != DBNull.Value ? (int)dataTable.Rows[i]["ServiceTypeId"] : 0,
                        ServiceTypes = dataTable.Rows[i]["ServiceTypes"] != DBNull.Value ? (string)dataTable.Rows[i]["ServiceTypes"] : null,
                        ServiceProviderId = (int)dataTable.Rows[i]["ServiceProviderId"],
                        ServiceProviderName = (string)dataTable.Rows[i]["ServiceProviderName"],
                        Gender = (GenderTypes)dataTable.Rows[i]["Gender"],
                        DOB = (DateTime)dataTable.Rows[i]["DOB"],
                        MobileNumber = (string)dataTable.Rows[i]["MobileNumber"],
                        ReferredBy = dataTable.Rows[i]["ReferredBy"] != DBNull.Value ? (string)dataTable.Rows[i]["ReferredBy"] : null,

                        AddressLine1 = dataTable.Rows[i]["AddressLine1"] != DBNull.Value ? (string)dataTable.Rows[i]["AddressLine1"] : null,
                        AddressTypeId = dataTable.Rows[i]["AddressTypeId"] != DBNull.Value ? (int)dataTable.Rows[i]["AddressTypeId"] : 0,
                        AddressTypes = dataTable.Rows[i]["AddressTypes"] != DBNull.Value ? (string)dataTable.Rows[i]["AddressTypes"] : null,
                        CountryId = dataTable.Rows[i]["CountryId"] != DBNull.Value ? (int)dataTable.Rows[i]["CountryId"] : 0,
                        CountryName = dataTable.Rows[i]["CountryName"] != DBNull.Value ? (string)dataTable.Rows[i]["CountryName"] : null,
                        PinCode = dataTable.Rows[i]["PinCode"] != DBNull.Value ? (int)dataTable.Rows[i]["PinCode"] : 0
                    };
                    serviceProviders.Add(serviceProvider);
                }
                return serviceProviders;
            }
        }

        public List<ServiceProviderName> GetServiceProviders(ServiceTypes serviceType)
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                string storedProcedure = "SP_ServiceProviderCRUD";

                SqlDataAdapter sqlDataAdapter = new(storedProcedure, sqlConnection)
                {
                    SelectCommand = { CommandType = CommandType.StoredProcedure }
                };

                sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@Mode", 2);
                sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@serviceTypeId", DBNull.Value);
                sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@ServiceProviderId", DBNull.Value);

                DataTable dataTable = new();
                sqlDataAdapter.Fill(dataTable);

                List<ServiceProviderName> serviceProviders = new();

                foreach (DataRow row in dataTable.Rows)
                {
                    ServiceProviderName serviceProvider = new()
                    {
                        //ServiceProviderId = Convert.ToInt32(row["ServiceProviderId"]),
                        //ServiceProviderName = Convert.ToString(row["ServiceProviderName"]),
                        //ServiceTypes = Convert.ToString(row["JobTypes"])
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
                string sqlQuery = "SP_ServiceProviderCRUD";

                SqlCommand sqlCommand = new(sqlQuery, sqlConnection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                sqlCommand.Parameters.AddWithValue("@Mode", 1);
                sqlCommand.Parameters.AddWithValue("@serviceTypeId", serviceProvider.ServiceTypeId);
                sqlCommand.Parameters.AddWithValue("@ServiceProviderName", serviceProvider.ServiceProviderName);
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
                string storedProcedureName = "SP_ServiceProviderCRUD";

                using (SqlCommand deleteServiceProviderCommand = new(storedProcedureName, sqlConnection))
                {
                    // Set the command type to StoredProcedure
                    deleteServiceProviderCommand.CommandType = CommandType.StoredProcedure;

                    deleteServiceProviderCommand.Parameters.AddWithValue("@Mode", 4);
                    deleteServiceProviderCommand.Parameters.AddWithValue("@ServiceProviderId", serviceProviderId);
                    sqlConnection.Open();
                    deleteServiceProviderCommand.ExecuteNonQuery();
                    sqlConnection.Close();
                }
            }
        }


        public int Update(ServiceProvider serviceProvider)
        {
            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                string sqlQuery = "SP_ServiceProviderCRUD";

                using (SqlCommand sqlUpdateCommand = new SqlCommand(sqlQuery, sqlConnection))
                {
                    // Set the command type to StoredProcedure
                    sqlUpdateCommand.CommandType = CommandType.StoredProcedure;

                    sqlUpdateCommand.Parameters.AddWithValue("@Mode", 3);
                    sqlUpdateCommand.Parameters.AddWithValue("@ServiceProviderId", serviceProvider.ServiceProviderId);
                    sqlUpdateCommand.Parameters.AddWithValue("@serviceTypeId", serviceProvider.ServiceTypeId);
                    sqlUpdateCommand.Parameters.AddWithValue("@ServiceProviderName", serviceProvider.ServiceProviderName);
                    sqlUpdateCommand.Parameters.AddWithValue("@gender", serviceProvider.Gender);
                    sqlUpdateCommand.Parameters.AddWithValue("@dob", serviceProvider.DOB);
                    sqlUpdateCommand.Parameters.AddWithValue("@mobileNumber", serviceProvider.MobileNumber);
                    sqlUpdateCommand.Parameters.AddWithValue("@referredBy", serviceProvider.ReferredBy);

                    sqlConnection.Open();
                    int affectedRowCount = sqlUpdateCommand.ExecuteNonQuery();
                    sqlConnection.Close();

                    return affectedRowCount;
                }
            }
        }
    }
}
