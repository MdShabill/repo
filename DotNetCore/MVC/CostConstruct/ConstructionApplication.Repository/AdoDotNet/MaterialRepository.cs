using ConstructionApplication.Core.DataModels.Material;
using System.Data.SqlClient;
using System.Data;
using ConstructionApplication.Repository.Interfaces;

namespace ConstructionApplication.Repository.AdoDotNet
{
    public class MaterialRepository : IMaterialRepository
    {
        private readonly string _connectionString;

        public MaterialRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<Material> GetAll()
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                string sqlQuery = "SELECT Id, Name FROM Materials";
                SqlDataAdapter sqlDataAdapter = new(sqlQuery, sqlConnection);
                DataTable dataTable = new();
                sqlDataAdapter.Fill(dataTable);

                List<Material> materials = new();

                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    Material material = new()
                    {
                        Id = (int)dataTable.Rows[i]["Id"],
                        Name = (string)dataTable.Rows[i]["Name"]
                    };
                    materials.Add(material);
                }
                return materials;
            }
        }

        public Material GetMaterialInfo(int Id)
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                string sqlQuery = @"SELECT Id, UnitOfMeasure, UnitPrice 
                            FROM Materials
                            WHERE Id = @id";

                SqlCommand sqlCommand = new(sqlQuery, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@id", Id);

                sqlConnection.Open();
                SqlDataReader reader = sqlCommand.ExecuteReader();

                Material material = null;

                if (reader.Read())
                {
                    material = new Material
                    {
                        Id = (int)reader["Id"],
                        UnitOfMeasure = (string)reader["UnitOfMeasure"],
                        UnitPrice = (decimal)reader["UnitPrice"]
                    };
                }

                reader.Close();

                return material;
            }
        }

    }
}
