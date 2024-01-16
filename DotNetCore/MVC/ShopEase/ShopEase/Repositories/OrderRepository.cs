using ShopEase.DataModels.OderItem;
using ShopEase.DataModels.Order;
using ShopEase.ViewModels;
using System.Data;
using System.Data.SqlClient;
using System.Transactions;

namespace ShopEase.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly string _connectionString;

        public OrderRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<Order> GetAllOrders(int? orderNumber, int? customerId)
        {
            using(SqlConnection sqlConnection = new(_connectionString))
            {
                string sqlQuery = @"Select Orders.Id, OrderItems.ProductId, Orders.OrderNumber, 
                             Orders.OrderDate, Orders.Price, OrderItems.Quantity, Products.Title,
                             Products.ImageName, Customers.FullName, Customers.Mobile,
                             Addresses.AddressLine1, Addresses.AddressLine2, 
                             Addresses.PinCode, Countries.CountryName,
                             AddressTypes.AddressTypeName
                             From Orders
                             Join OrderItems On OrderItems.OrderId = Orders.Id
                             JOIN Products ON OrderItems.ProductId = Products.Id
                             JOIN Customers ON Orders.CustomerId = Customers.Id
                             JOIN Addresses ON Orders.AddressId = Addresses.Id
                             JOIN AddressTypes ON Addresses.AddressTypeID = AddressTypes.Id
                             JOIN Countries ON Addresses.CountryId = Countries.Id
                             Where 1=1";

                SqlDataAdapter sqlDataAdapter = new(sqlQuery, sqlConnection);
                if (customerId != null)
                {
                    sqlQuery += " And Orders.CustomerId = @customerId ";
                    sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@customerId", customerId);
                }

                if (orderNumber != null)
                {
                    sqlQuery += " And Orders.OrderNumber = @orderNumber ";
                    sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@orderNumber", orderNumber);
                }

                sqlDataAdapter.SelectCommand.CommandText = sqlQuery;
                DataTable dataTable = new();
                sqlDataAdapter.Fill(dataTable);

                List<Order> orders = new();

                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    Order order = new Order()
                    {
                        Id = (int)dataTable.Rows[i]["Id"],
                        ProductId = (int)dataTable.Rows[i]["ProductId"],
                        OrderNumber = (string)dataTable.Rows[i]["OrderNumber"],
                        Title = (string)dataTable.Rows[i]["Title"],
                        ImageName = (string)dataTable.Rows[i]["ImageName"],
                        FullName = (string)dataTable.Rows[i]["FullName"],
                        Mobile = (string)dataTable.Rows[i]["Mobile"],
                        OrderDate = (DateTime)dataTable.Rows[i]["OrderDate"],
                        Price = Convert.ToInt32(dataTable.Rows[i]["Price"]),
                        Quantity = (int)dataTable.Rows[i]["Quantity"],
                        AddressLine1 = (string)dataTable.Rows[i]["AddressLine1"],
                        AddressLine2 = (string)dataTable.Rows[i]["AddressLine2"],
                        PinCode = (int)dataTable.Rows[i]["PinCode"],
                        CountryName = (string)dataTable.Rows[i]["CountryName"],
                        AddressTypeName = (string)dataTable.Rows[i]["AddressTypeName"],
                    };
                    orders.Add(order);
                }
                return orders;
            }
        }

        //TODO: Refactor and move these three query into three different repo methods
        public int AddOrder(Order order)
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                sqlConnection.Open();
                using (SqlTransaction transaction = sqlConnection.BeginTransaction())
                {
                    try
                    {
                        string stringQuery = @"INSERT INTO Orders 
                                    (OrderNumber,CustomerId, OrderDate, 
                                    Price, AddressId)
                                    VALUES
                                    (@orderNumber, @customerId, GETDATE(),
                                    @price, @addressId)
                                    Select Scope_Identity()";

                        SqlCommand sqlCommand = new(stringQuery, sqlConnection, transaction);
                     
                        sqlCommand.Parameters.AddWithValue("@orderNumber", order.OrderNumber);
                        sqlCommand.Parameters.AddWithValue("@customerId", order.CustomerId);
                        sqlCommand.Parameters.AddWithValue("@price", order.Price);
                        sqlCommand.Parameters.AddWithValue("@addressId", order.AddressId);

                        order.OrderId = Convert.ToInt32(sqlCommand.ExecuteScalar());

                        transaction.Commit();
                        sqlConnection.Close();
                        
                        return order.OrderId;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new Exception($"Error adding order and updating product quantity: {ex.Message}");
                    }
                }
            }
        }

        public void AddOrderItem(OrderItem orderItem)
        {
           using(SqlConnection sqlConnection = new(_connectionString))
           {
                string orderItemQuery = @"INSERT INTO OrderItems 
                                         (OrderId, OrderNumber, ProductId, Quantity)
                                         VALUES
                                         (@orderId, @orderNumber, @productId, @quantity)";

                SqlCommand orderItemCommand = new SqlCommand(orderItemQuery, sqlConnection);
                orderItemCommand.Parameters.AddWithValue("@orderId", orderItem.OrderId);
                orderItemCommand.Parameters.AddWithValue("@orderNumber", orderItem.OrderNumber);
                orderItemCommand.Parameters.AddWithValue("@productId", orderItem.ProductId);
                orderItemCommand.Parameters.AddWithValue("@quantity", orderItem.Quantity);

                sqlConnection.Open();
                orderItemCommand.ExecuteNonQuery();
                sqlConnection.Close();
           }
        }

        public void UpdateProductQuantity(int productId, int orderedQuantity)
        {
            using(SqlConnection sqlConnection=new(_connectionString)) 
            {
                string updateQuery = @"UPDATE Products
                                     SET Quantity = Quantity - @orderedQuantity
                                     WHERE Id = @productId";

                SqlCommand sqlUpdatCommand = new(updateQuery, sqlConnection);

                sqlUpdatCommand.Parameters.AddWithValue("@orderedQuantity", orderedQuantity);
                sqlUpdatCommand.Parameters.AddWithValue("@productId", productId);

                sqlConnection.Open ();
                sqlUpdatCommand.ExecuteNonQuery();
                sqlConnection.Close();
            }
        }
    }
}
