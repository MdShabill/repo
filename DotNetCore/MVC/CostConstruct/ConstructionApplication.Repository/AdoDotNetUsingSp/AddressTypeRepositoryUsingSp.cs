using ConstructionApplication.Core.DataModels.AddressType;
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
    public class AddressTypeRepositoryUsingSp : IAddressTypeRepository
    {
        private readonly string _connectionString;

        public AddressTypeRepositoryUsingSp(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<AddressType> GetAll()
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                using (SqlCommand sqlCommand = new("Sp_GetAllAddressTypes", sqlConnection))
                {
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlConnection.Open();

                    using (SqlDataAdapter sqlDataAdapter = new(sqlCommand))
                    {
                        DataTable dataTable = new();
                        sqlDataAdapter.Fill(dataTable);

                        List<AddressType> addressTypes = new();

                        for (int i = 0; i < dataTable.Rows.Count; i++)
                        {
                            AddressType addressType = new()
                            {
                                Id = Convert.ToInt32(dataTable.Rows[i]["Id"]),
                                Name = Convert.ToString(dataTable.Rows[i]["Name"])!
                            };
                            addressTypes.Add(addressType);
                        }
                        return addressTypes;
                    }
                }
            }
        }

    }
}
