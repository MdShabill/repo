using ShopEase.DataModels;
using System.Data;
using System.Data.SqlClient;

namespace ShopEase.Repositories
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
            using(SqlConnection sqlConnection = new(_connectionString))
            {
                string sqlQuery = @"Insert Into Orders 
                                  (ProductId, CustomerId, OrderDate, Price, Quantity, AddressId)
                                  Values
                                  (@productId, @customerId, GetDate(), @price, @quantity, @addressId)";

                SqlCommand sqlCommand = new(sqlQuery, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@productId", order.ProductId);
                sqlCommand.Parameters.AddWithValue("@customerId", order.CustomerId);
                sqlCommand.Parameters.AddWithValue("@price", order.Price);
                sqlCommand.Parameters.AddWithValue("@quantity", order.Quantity);
                sqlCommand.Parameters.AddWithValue("@addressId", order.AddressId);

                sqlConnection.Open();
                int affectedRowCount = sqlCommand.ExecuteNonQuery();
                sqlConnection.Close();

                return affectedRowCount;
            }
        }
    }
}
