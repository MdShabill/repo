using Microsoft.Data.SqlClient;
using System.Data;
using WebApiDemo1.DTO.InputDTO;
using WebApplication1.DTO.InputDTO;

namespace WebApiDemo1.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly string _connectionString;

        public OrderRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public DataTable GetAllOrders()
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                SqlDataAdapter sqlDataAdapter = new("Select * From Orders", sqlConnection);
                DataTable dataTable = new();
                sqlDataAdapter.Fill(dataTable);
                return dataTable;
            }
        }

        public int GetOrdersCount()
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                string sqlQuery = "Select Count(*) From Orders";
                SqlCommand sqlCommand = new(sqlQuery, sqlConnection);
                sqlConnection.Open();
                int orderCount = Convert.ToInt32(sqlCommand.ExecuteScalar());
                sqlConnection.Close();
                return orderCount;
            }
        }

        public DataTable GetOrderDetailById(int orderId)
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                SqlDataAdapter sqlDataAdapter = new("Select * From Orders Where Id = @orderId", sqlConnection);
                sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@orderId", orderId);
                DataTable dataTable = new();
                sqlDataAdapter.Fill(dataTable);
                return dataTable;
            }
        }

        public DataTable GetOrdersDetailByOrderDate(string orderDateTime)
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                SqlDataAdapter sqlDataAdapter = new("Select * From Orders Where OrderDate = @orderDateTime", sqlConnection);
                sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@orderDateTime", orderDateTime);
                DataTable dataTable = new();
                sqlDataAdapter.Fill(dataTable);
                return dataTable;
            }
        }

        public DataTable GetOrdersByAmountRange(int minimumAmount, int maximumAmount)
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                SqlDataAdapter sqlDataAdapter = new(@"Select * From Orders 
                        Where Amount Between @minimumAmount AND @maximumAmount
                        Order By Amount", sqlConnection);
                sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@minimumAmount", minimumAmount);
                sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@maximumAmount", maximumAmount);
                DataTable dataTable = new();
                sqlDataAdapter.Fill(dataTable);
                return dataTable;
            }
        }

        public int OrderAdd(OrderDto order)
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                string sqlQuery = @"INSERT INTO Orders(CustomerId, OrderDate, Amount, ProductName)
                        VALUES (@CustomerId, @OrderDate, @Amount, @ProductName)
                        Select Scope_Identity() ";
                SqlCommand sqlCommand = new(sqlQuery, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@CustomerId", order.CustomerId);
                sqlCommand.Parameters.AddWithValue("@OrderDate", order.OrderDate);
                sqlCommand.Parameters.AddWithValue("@Amount", order.TotalAmount);
                sqlCommand.Parameters.AddWithValue("@ProductName", order.ProductName);
                sqlConnection.Open();
                order.Id = Convert.ToInt32(sqlCommand.ExecuteScalar());
                sqlConnection.Close();
                return order.Id;
            }
        }

        public void OrderUpdate(OrderDto order)
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                string sqlQuery = @" UPDATE Orders SET CustomerId = @CustomerId, OrderDate = @OrderDate,
                        Amount = @Amount, ProductName = @ProductName
                        WHERE Id = @Id ";
                SqlCommand sqlCommand = new(sqlQuery, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@Id", order.Id);
                sqlCommand.Parameters.AddWithValue("@CustomerId", order.CustomerId);
                sqlCommand.Parameters.AddWithValue("@OrderDate", order.OrderDate);
                sqlCommand.Parameters.AddWithValue("@Amount", order.TotalAmount);
                sqlCommand.Parameters.AddWithValue("@ProductName", order.ProductName);
                sqlConnection.Open();
                sqlCommand.ExecuteNonQuery();
                sqlConnection.Close();
            }
        }
    }
}
