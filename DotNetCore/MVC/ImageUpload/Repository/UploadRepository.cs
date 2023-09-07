using System.Data.SqlClient;
using UploadFile.Models;

namespace UploadFile.Repository
{
    public class UploadRepository : IUploadRepository
    {
        private readonly string _connectionString;

        public UploadRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public int AddImage(ProductImage productImage)
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                string sqlQuery = @"Insert Into ProductImages
                                    (ProductName, Description, ImageName)
                                    Values
                                    (@ProductName, @Description, @uniqueFileName) ";
                SqlCommand sqlCommand = new(sqlQuery, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@ProductName", productImage.ProductName);
                sqlCommand.Parameters.AddWithValue("@Description", productImage.Description);
                sqlCommand.Parameters.AddWithValue("@uniqueFileName", productImage.ImageName);
                sqlConnection.Open();
                int affectedRowsCount = sqlCommand.ExecuteNonQuery();
                sqlConnection.Close();

                return affectedRowsCount;
            }
        }
    }
}
