using MyWebApp.Models;
using System.Data;
using Microsoft.Data.SqlClient;
using MyWebApp.Enums;

namespace MyWebApp.Repositories
{
    public class Product1Repository : IProduct1Repository
    {
        private readonly string _connectionString;

        public Product1Repository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<Product1> GetProduct(string productName)
        {
            using(SqlConnection sqlConnection = new (_connectionString))
            {
                string sqlQuery = @"
                        Select * From Products 
                        Where 
                        ProductName LIKE '%' + @productName + '%'";

                SqlDataAdapter sqlDataAdapter = new(sqlQuery, sqlConnection);
                sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@ProductName", productName);
                DataTable dataTable= new();
                sqlDataAdapter.Fill(dataTable);

                List<Product1> products = new();

                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    Product1 product1 = new()
                    {
                        Id = (int)dataTable.Rows[i]["Id"],
                        ProductName = (string)dataTable.Rows[i]["ProductName"]
                    };
                    products.Add(product1);
                }
                return products;
            }
        }
    }
}
