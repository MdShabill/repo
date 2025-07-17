using ConstructionApplication.Core.DataModels.AddressType;
using ConstructionApplication.Core.DataModels.SiteStatus;
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
    public class SiteStatusRepository : ISiteStatusRepository
    {
        private readonly string _connectionString;

        public SiteStatusRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<SiteStatus> GetAll()
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                string sqlQuery = "Select Id, Status From SiteStatus";
                SqlDataAdapter sqlDataAdapter = new(sqlQuery, sqlConnection);
                DataTable dataTable = new();
                sqlDataAdapter.Fill(dataTable);

                List<SiteStatus> siteStatuses = new();

                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    SiteStatus siteStatus = new()
                    {
                        Id = (int)dataTable.Rows[i]["Id"],
                        Status = (string)dataTable.Rows[i]["Status"]
                    };
                    siteStatuses.Add(siteStatus);
                }
                return siteStatuses;
            }
        }
    }
}
