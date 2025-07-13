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
                string sqlQuery = "Select Id, Name, Location From Sites";
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
                    };
                    sites.Add(site);
                }
                return sites;
            }
        }

        public Site GetSiteById(int id)
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                string sqlQuery = "SELECT Id, Name, Location FROM Sites WHERE Id = @SiteId";
                SqlCommand cmd = new(sqlQuery, sqlConnection);
                cmd.Parameters.AddWithValue("@SiteId", id);

                sqlConnection.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Site
                        {
                            Id = (int)reader["Id"],
                            Name = reader["Name"].ToString(),
                            Location = reader["Location"].ToString()
                        };
                    }
                }
            }
            return null;
        }

    }
}
