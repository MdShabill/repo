using ConstructionApplication.Core.DataModels.CostMaster;
using ConstructionApplication.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConstructionApplication.Core.DataModels.ServiceTypes;

namespace ConstructionApplication.Repository.AdoDotNetUsingSp
{
    public class CostMasterRepositoryUsingSp : ICostMasterRepository
    {
        private readonly string _connectionString;

        public CostMasterRepositoryUsingSp(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<CostMaster> GetByServiceType(int serviceTypeId, int siteId)
        {
            using SqlConnection sqlConnection = new(_connectionString);
            SqlCommand sqlCommand = new("Sp_CostMasterCRUD", sqlConnection);
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Mode", "GET_BY_SERVICETYPE");
            sqlCommand.Parameters.AddWithValue("@ServiceTypeId", serviceTypeId);
            sqlCommand.Parameters.AddWithValue("@SiteId", siteId);
            SqlDataAdapter adapter = new(sqlCommand);
            DataTable table = new();
            adapter.Fill(table);
            List<CostMaster> list = new();
            foreach (DataRow row in table.Rows)
            {
                list.Add(new CostMaster
                {
                    Id = (int)row["Id"],
                    ServiceTypeId = (int)row["ServiceTypeId"],
                    SiteId = (int)row["SiteId"],
                    Name = row["Name"].ToString(),
                    Cost = (decimal)row["Cost"],
                    Date = (DateTime)row["Date"]
                });
            }
            return list;
        }

        public CostMaster GetActiveCostDetail(int serviceTypeId, int siteId)
        {
            using SqlConnection connection = new(_connectionString);
            SqlCommand command = new("Sp_CostMasterCRUD", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@Mode", "GET_ACTIVE_COST");
            command.Parameters.AddWithValue("@ServiceTypeId", serviceTypeId);
            command.Parameters.AddWithValue("@SiteId", siteId);
            command.Parameters.AddWithValue("@CurrentDate", DateTime.Now);
            SqlDataAdapter adapter = new(command);
            DataTable table = new();
            adapter.Fill(table);
            if (table.Rows.Count == 0)
                return null;
            DataRow row = table.Rows[0];
            return new CostMaster
            {
                ServiceTypeId = (int)row["ServiceTypeId"],
                SiteId = (int)row["SiteId"],
                Cost = (decimal)row["Cost"],
                Date = (DateTime)row["Date"]
            };
        }

        public CostMaster GetById(int id, int siteId)
        {
            using SqlConnection connection = new(_connectionString);
            SqlCommand command = new("Sp_CostMasterCRUD", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@Mode", "GET_BY_ID");
            command.Parameters.AddWithValue("@Id", id);
            command.Parameters.AddWithValue("@SiteId", siteId);
            SqlDataAdapter adapter = new(command);
            DataTable table = new();
            adapter.Fill(table);
            if (table.Rows.Count == 0)
                return null;
            DataRow row = table.Rows[0];
            return new CostMaster
            {
                Id = (int)row["Id"],
                ServiceTypeId = (int)row["ServiceTypeId"],
                SiteId = (int)row["SiteId"],
                Name = row["Name"].ToString(),
                Cost = (decimal)row["Cost"],
                Date = (DateTime)row["Date"]
            };
        }

        public int Create(CostMaster costMaster)
        {
            using SqlConnection connection = new(_connectionString);
            SqlCommand command = new("Sp_CostMasterCRUD", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@Mode", "CREATE");
            command.Parameters.AddWithValue("@ServiceTypeId", costMaster.ServiceTypeId);
            command.Parameters.AddWithValue("@SiteId", costMaster.SiteId);
            command.Parameters.AddWithValue("@Cost", costMaster.Cost);
            command.Parameters.AddWithValue("@Date", costMaster.Date);
            connection.Open();
            object result = command.ExecuteScalar();
            return Convert.ToInt32(result);
        }

        public int Update(CostMaster costMaster)
        {
            using SqlConnection connection = new(_connectionString);
            SqlCommand command = new("Sp_CostMasterCRUD", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@Mode", "UPDATE");
            command.Parameters.AddWithValue("@Id", costMaster.Id);
            command.Parameters.AddWithValue("@ServiceTypeId", costMaster.ServiceTypeId);
            command.Parameters.AddWithValue("@SiteId", costMaster.SiteId);
            command.Parameters.AddWithValue("@Cost", costMaster.Cost);
            command.Parameters.AddWithValue("@Date", costMaster.Date);
            connection.Open();
            object result = command.ExecuteScalar();
            return Convert.ToInt32(result);
        }

        public void Delete(int id, int siteId)
        {
            using SqlConnection connection = new(_connectionString);
            SqlCommand command = new("Sp_CostMasterCRUD", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@Mode", "DELETE");
            command.Parameters.AddWithValue("@Id", id);
            command.Parameters.AddWithValue("@SiteId", siteId);
            connection.Open();
            command.ExecuteNonQuery();
        }
    }
}