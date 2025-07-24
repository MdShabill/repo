using ConstructionApplication.Core.DataModels.Country;
using ConstructionApplication.Core.DataModels.Site;
using ConstructionApplication.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConstructionApplication.Core.DataModels.CostMaster;
using ConstructionApplication.Core.DataModels.ServiceProviders;
using ConstructionApplication.Core.Enums;

namespace ConstructionApplication.Repository.AdoDotNet
{
    public class SiteRepository : ISiteRepository
    {
        private readonly string _connectionString;

        public SiteRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<Site> GetAllSites()
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                string sqlQuery = @"Select Sites.Id, Sites.Name, Sites.StartedDate,
                       Sites.SiteStatusId, SiteStatus.Status, Addresses.AddressLine1,
                       Addresses.AddressTypeId, AddressTypes.Name As AddressTypes,
                       Addresses.CountryId, Countries.Name As CountryName,
	                   Addresses.PinCode
                       From Sites
                       LEFT JOIN SiteStatus ON Sites.SiteStatusId = SiteStatus.Id
                       LEFT JOIN Addresses ON Sites.Id = Addresses.SiteId
                       LEFT JOIN AddressTypes ON Addresses.AddressTypeId = AddressTypes.Id
                       LEFT JOIN Countries ON Addresses.CountryId = Countries.Id";
                SqlDataAdapter sqlDataAdapter = new(sqlQuery, sqlConnection);
                DataTable dataTable = new();
                sqlDataAdapter.Fill(dataTable);

                List<Site> sites = new();

                foreach (DataRow row in dataTable.Rows)
                {
                    Site site = new()
                    {
                        Id = row["Id"] != DBNull.Value ? (int)row["Id"] : 0,
                        Name = row["Name"] != DBNull.Value ? (string)row["Name"] : null,
                        StartedDate = row["StartedDate"] != DBNull.Value ? (DateTime?)row["StartedDate"] : null,
                        SiteStatusId = row["SiteStatusId"] != DBNull.Value ? (int)row["SiteStatusId"] : 0,
                        Status = row["Status"] != DBNull.Value ? (string)row["Status"] : null,

                        AddressLine1 = row["AddressLine1"] != DBNull.Value ? (string)row["AddressLine1"] : null,
                        AddressTypeId = row["AddressTypeId"] != DBNull.Value ? (int)row["AddressTypeId"] : 0,
                        AddressTypes = row["AddressTypes"] != DBNull.Value ? (string)row["AddressTypes"] : null,
                        CountryId = row["CountryId"] != DBNull.Value ? (int)row["CountryId"] : 0,
                        CountryName = row["CountryName"] != DBNull.Value ? (string)row["CountryName"] : null,
                        PinCode = row["PinCode"] != DBNull.Value ? (int)row["PinCode"] : 0
                    };
                    sites.Add(site);
                }
                return sites;
            }
        }

        public Site GetSiteById(int Id)
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                string sqlQuery = @"Select Sites.Id, Sites.Name, Sites.StartedDate,
                       Sites.SiteStatusId, SiteStatus.Status, Addresses.AddressLine1,
                       Addresses.AddressTypeId, AddressTypes.Name As AddressTypes,
                       Addresses.CountryId, Countries.Name As CountryName,
	                   Addresses.PinCode
                       From Sites
                       LEFT JOIN SiteStatus ON Sites.SiteStatusId = SiteStatus.Id
                       LEFT JOIN Addresses ON Sites.Id = Addresses.SiteId
                       LEFT JOIN AddressTypes ON Addresses.AddressTypeId = AddressTypes.Id
                       LEFT JOIN Countries ON Addresses.CountryId = Countries.Id 
                       WHERE Sites.Id = @SiteId";
                SqlCommand sqlCommand = new(sqlQuery, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@SiteId", Id);

                sqlConnection.Open();
                using (SqlDataReader reader = sqlCommand.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Site
                        {
                            Id = reader["Id"] != DBNull.Value ? (int)reader["Id"] : 0,
                            Name = reader["Name"] != DBNull.Value ? reader["Name"].ToString() : null,
                            StartedDate = reader["StartedDate"] != DBNull.Value ? (DateTime?)reader["StartedDate"] : null,
                            SiteStatusId = reader["SiteStatusId"] != DBNull.Value ? (int)reader["SiteStatusId"] : 0,
                            Status = reader["Status"] != DBNull.Value ? reader["Status"].ToString() : null,

                            AddressLine1 = reader["AddressLine1"] != DBNull.Value ? reader["AddressLine1"].ToString() : null,
                            AddressTypeId = reader["AddressTypeId"] != DBNull.Value ? (int)reader["AddressTypeId"] : 0,
                            AddressTypes = reader["AddressTypes"] != DBNull.Value ? reader["AddressTypes"].ToString() : null,
                            CountryId = reader["CountryId"] != DBNull.Value ? (int)reader["CountryId"] : 0,
                            CountryName = reader["CountryName"] != DBNull.Value ? reader["CountryName"].ToString() : null,
                            PinCode = reader["PinCode"] != DBNull.Value ? (int)reader["PinCode"] : 0
                        };
                    }
                }
            }
            return null;
        }

        public int Create(Site site)
        {
            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                string insertQuery = @"INSERT INTO Sites
                                        (Name, StartedDate, SiteStatusId, Note)
                                        VALUES
                                        (@name, @startedDate, @siteStatusId, @note)
                                        Select Scope_Identity() ";
                SqlCommand insertCommand = new SqlCommand(insertQuery, sqlConnection);
                insertCommand.Parameters.AddWithValue("@name", site.Name);
                insertCommand.Parameters.AddWithValue("@startedDate", site.StartedDate);
                insertCommand.Parameters.AddWithValue("@siteStatusId", site.SiteStatusId);
                insertCommand.Parameters.AddWithValue("@note", string.IsNullOrEmpty(site.Note) ? DBNull.Value : site.Note);
                sqlConnection.Open();
                site.Id = Convert.ToInt32(insertCommand.ExecuteScalar());
                sqlConnection.Close();

                return site.Id;
            }
        }

        public int Update(Site site)
        {
            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                string sqlQuery = @"UPDATE Sites SET
                            Name = @name,
                            StartedDate = @startedDate,
                            SiteStatusId = @siteStatusId,
                            Note = @note  
                            WHERE Id = @id";

                using (SqlCommand updateCommand = new SqlCommand(sqlQuery, sqlConnection))
                {
                    updateCommand.Parameters.AddWithValue("@id", site.Id);
                    updateCommand.Parameters.AddWithValue("@name", site.Name);
                    updateCommand.Parameters.AddWithValue("@startedDate", site.StartedDate);
                    updateCommand.Parameters.AddWithValue("@siteStatusId", site.SiteStatusId);
                    updateCommand.Parameters.AddWithValue("@note", string.IsNullOrEmpty(site.Note) ? DBNull.Value : site.Note);

                    sqlConnection.Open();
                    int affectedRowCount = updateCommand.ExecuteNonQuery();
                    sqlConnection.Close();
                    return affectedRowCount;
                }
            }
        }

        public void Delete(int siteId)
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                string deleteQuery = "DELETE FROM Sites WHERE Id = @Id";
                SqlCommand deleteCommand = new(deleteQuery, sqlConnection);
                deleteCommand.Parameters.AddWithValue("@Id", siteId);
                sqlConnection.Open();
                deleteCommand.ExecuteNonQuery();
                sqlConnection.Close();
            }
        }

        public void AddSiteServiceProviderBridge(int siteId, Core.Enums.ServiceTypes ServiceType, List<int> ServiceProviderIds)
        {
            throw new NotImplementedException();
        }
    }
}
