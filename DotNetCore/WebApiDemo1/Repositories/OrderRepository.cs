using Microsoft.Data.SqlClient;
using System.Data;
using WebApiDemo1.DTO.InputDTO;
using WebApiDemo1.Enums;
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

        public List<OrderDto> GetAllOrdersAsList()
        {
            List<OrderDto> orders = new();

            using (SqlConnection sqlConnection = new(_connectionString))
            {
                SqlDataAdapter sqlDataAdapter = new("SELECT * FROM Orders", sqlConnection);
                DataTable dataTable = new();
                sqlDataAdapter.Fill(dataTable);

                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    OrderDto orderDto = new();
                    orderDto.Id = (int)dataTable.Rows[i]["Id"];
                    orderDto.CustomerId = (int)dataTable.Rows[i]["CustomerId"];
                    orderDto.TotalAmount = (int)dataTable.Rows[i]["Amount"];
                    orderDto.ProductName = (ProductType)dataTable.Rows[i]["ProductName"];

                    orders.Add(orderDto);
                }
                return orders;
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

        public OrderDto GetOrderDetailById(int orderId)
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                SqlDataAdapter sqlDataAdapter = new("SELECT * FROM Orders WHERE Id = @orderId", sqlConnection);
                sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@orderId", orderId);
                DataTable dataTable = new();
                sqlDataAdapter.Fill(dataTable);

                if (dataTable.Rows.Count > 0)
                {
                    OrderDto orderDto = new();
                    orderDto.Id = (int)dataTable.Rows[0]["Id"];
                    orderDto.CustomerId = (int)dataTable.Rows[0]["CustomerId"];
                    orderDto.TotalAmount = (int)dataTable.Rows[0]["Amount"];
                    orderDto.ProductName = (ProductType)dataTable.Rows[0]["ProductName"];

                    return orderDto;
                }
                else
                    return null;
            }
        }

        public List<OrderDto> GetOrdersByAmountRange(int minimumAmount, int maximumAmount)
        {
            List<OrderDto> orders = new();

            using (SqlConnection sqlConnection = new(_connectionString))
            {
                SqlDataAdapter sqlDataAdapter = new(@"Select * From Orders 
                        Where Amount Between @minimumAmount AND @maximumAmount
                        Order By Amount", sqlConnection);
                sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@minimumAmount", minimumAmount);
                sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@maximumAmount", maximumAmount);
                DataTable dataTable = new();
                sqlDataAdapter.Fill(dataTable);

                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    OrderDto orderDto = new();
                    orderDto.Id = (int)dataTable.Rows[i]["Id"];
                    orderDto.CustomerId = (int)dataTable.Rows[i]["CustomerId"];
                    orderDto.TotalAmount = (int)dataTable.Rows[i]["Amount"];
                    orderDto.ProductName = (ProductType)dataTable.Rows[i]["ProductName"];

                    orders.Add(orderDto);
                }
                return orders;
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
