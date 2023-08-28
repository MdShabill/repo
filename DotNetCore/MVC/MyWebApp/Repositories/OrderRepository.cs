using System.Data;
using Microsoft.Data.SqlClient;
using MyWebApp.Enums;
using MyWebApp.DataModel;
using MyWebApp.ViewModels.Products;
    
namespace MyWebApp.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly string _connectionString;

        public OrderRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public int PlaceOrder(Order order)
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                string sqlQuery = @"Insert Into Orders
                        (Productid, CustomerId, OrderDate, Price, Quantity)
                        Values
                        (@ProductId, @CustomerId, GetDate(), @Price, @Quantity) ";

                SqlCommand sqlCommand = new(sqlQuery, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@ProductId", order.ProductId);
                sqlCommand.Parameters.AddWithValue("@CustomerId", order.CustomerId);
                sqlCommand.Parameters.AddWithValue("@Price", order.Price);
                sqlCommand.Parameters.AddWithValue("@Quantity", order.Quantity);
                sqlConnection.Open();
                int affectedRowCount = sqlCommand.ExecuteNonQuery();
                sqlConnection.Close();
                return affectedRowCount;
            }
        }
    }
}
