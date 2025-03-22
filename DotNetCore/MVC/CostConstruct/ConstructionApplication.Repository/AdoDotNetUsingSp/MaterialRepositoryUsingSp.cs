using ConstructionApplication.Core.DataModels.Material;
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
    public class MaterialRepositoryUsingSp : IMaterialRepository
    {
        private readonly string _connectionString;

        public MaterialRepositoryUsingSp(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<Material> GetAll()
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                using (SqlCommand sqlCommand = new("Sp_MaterialsDetails", sqlConnection))
                {
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("@Mode", "GetAll");

                    sqlConnection.Open();
                    using (SqlDataAdapter sqlDataAdapter = new(sqlCommand))
                    {
                        DataTable dataTable = new();
                        sqlDataAdapter.Fill(dataTable);

                        List<Material> materials = new();

                        for (int i = 0; i < dataTable.Rows.Count; i++)
                        {
                            Material material = new()
                            {
                                Id = Convert.ToInt32(dataTable.Rows[i]["Id"]),
                                Name = Convert.ToString(dataTable.Rows[i]["Name"])!
                            };
                            materials.Add(material);
                        }
                        return materials;
                    }
                }
            }
        }

        public Material GetMaterialInfo(int Id)
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                using (SqlCommand sqlCommand = new("Sp_MaterialsDetails", sqlConnection))
                {
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("@Mode", "GetMaterialInfo");
                    sqlCommand.Parameters.AddWithValue("@Id", Id);

                    sqlConnection.Open();
                    using (SqlDataAdapter sqlDataAdapter = new(sqlCommand))
                    {
                        DataTable dataTable = new();
                        sqlDataAdapter.Fill(dataTable);

                        if (dataTable.Rows.Count > 0)
                        {
                            Material material = new()
                            {
                                Id = Convert.ToInt32(dataTable.Rows[0]["Id"]),
                                UnitOfMeasure = Convert.ToString(dataTable.Rows[0]["UnitOfMeasure"])!,
                                UnitPrice = Convert.ToDecimal(dataTable.Rows[0]["UnitPrice"])
                            };
                            return material;
                        }
                        return null!;
                    }
                }
            }
        }
    }
}
