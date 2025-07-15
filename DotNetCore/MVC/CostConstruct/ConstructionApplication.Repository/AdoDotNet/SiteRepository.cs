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
                string sqlQuery = "Select Id, Name, Location, CreatedDate From Sites";
                SqlDataAdapter sqlDataAdapter = new(sqlQuery, sqlConnection);
                DataTable dataTable = new();
                sqlDataAdapter.Fill(dataTable);

                List<Site> sites = new();

                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    Site site = new()
                    {
                        Id = (int)dataTable.Rows[i]["Id"],
                        Name = (string)dataTable.Rows[i]["Name"],
                        Location = (string)dataTable.Rows[i]["Location"],
                        CreatedDate = (DateTime)dataTable.Rows[i]["CreatedDate"],
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
                string sqlQuery = "SELECT Id, Name, Location, CreatedDate FROM Sites WHERE Id = @SiteId";
                SqlCommand cmd = new(sqlQuery, sqlConnection);
                cmd.Parameters.AddWithValue("@SiteId", Id);

                sqlConnection.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Site
                        {
                            Id = (int)reader["Id"],
                            Name = reader["Name"].ToString(),
                            Location = reader["Location"].ToString(),
                            CreatedDate = (DateTime)reader["CreatedDate"]
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
                                        (Name, Location, CreatedDate)
                                        VALUES
                                        (@name, @location, @createDate)";
                SqlCommand insertCommand = new SqlCommand(insertQuery, sqlConnection);
                insertCommand.Parameters.AddWithValue("@name", site.Name);
                insertCommand.Parameters.AddWithValue("@location", site.Location);
                insertCommand.Parameters.AddWithValue("@createDate", site.CreatedDate);
                sqlConnection.Open();
                int affectedRowCount = insertCommand.ExecuteNonQuery();
                sqlConnection.Close();

                return affectedRowCount;
            }
        }

        public int Update(Site site)
        {
            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                string sqlQuery = @"UPDATE Sites SET
                            Name = @name,
                            Location = @location,
                            CreatedDate = @createDate
                            WHERE Id = @id";

                using (SqlCommand updateCommand = new SqlCommand(sqlQuery, sqlConnection))
                {
                    updateCommand.Parameters.AddWithValue("@id", site.Id);
                    updateCommand.Parameters.AddWithValue("@name", site.Name);
                    updateCommand.Parameters.AddWithValue("@location", site.Location);
                    updateCommand.Parameters.AddWithValue("@createDate", site.CreatedDate);

                    sqlConnection.Open();
                    int affectedRowCount = updateCommand.ExecuteNonQuery();
                    sqlConnection.Close();
                    return affectedRowCount;
                }
            }
        }

        public void Delete(int id)
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                string deleteQuery = "DELETE FROM Sites WHERE Id = @Id";
                SqlCommand deleteCommand = new(deleteQuery, sqlConnection);
                deleteCommand.Parameters.AddWithValue("@Id", id);
                sqlConnection.Open();
                deleteCommand.ExecuteNonQuery();
                sqlConnection.Close();
            }
        }
    }
}
