﻿using System.Data;
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

        public int Add(ProductImage productImage)
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

        public ProductImage GetImageById(int id)
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                string sqlQuery = @"Select * From ProductImages
                                   Where Id = @id ";

                SqlDataAdapter sqlDataAdapter = new(sqlQuery, sqlConnection);
                sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@id", id);
                DataTable dataTable = new();
                sqlDataAdapter.Fill(dataTable);

                ProductImage productImage = new()
                {
                    Id = (int)dataTable.Rows[0]["Id"],
                    ProductName = (string)dataTable.Rows[0]["ProductName"],
                    Description = (string)dataTable.Rows[0]["Description"],
					ImageName = (string)dataTable.Rows[0]["ImageName"],
                };
                return productImage;
            }
        }
    }
}
