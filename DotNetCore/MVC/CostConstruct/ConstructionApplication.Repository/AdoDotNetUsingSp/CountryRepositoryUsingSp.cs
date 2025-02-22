using ConstructionApplication.Core.DataModels.Country;
using ConstructionApplication.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConstructionApplication.Repository.AdoDotNetUsingSp
{
    public class CountryRepositoryUsingSp : ICountryRepository
    {
        private readonly string _connectionString;

        public CountryRepositoryUsingSp(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<Country> GetAllCountries()
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                SqlDataAdapter sqlDataAdapter = new("Sp_GetAllCountries", sqlConnection);
                sqlDataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                DataTable dataTable = new();
                sqlDataAdapter.Fill(dataTable);

                List<Country> countries = new();

                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    Country country = new()
                    {
                        Id = (int)dataTable.Rows[i]["Id"],
                        Name = (string)dataTable.Rows[i]["Name"]
                    };
                    countries.Add(country);
                }
                return countries;
            }
        }
    }
}
