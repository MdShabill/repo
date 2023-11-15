using ShopEase.DataModels;
using ShopEase.ViewModels;
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

        public List<Order> GetAllOrders(int? customerId)
        {
            using(SqlConnection sqlConnection = new(_connectionString))
            {
                string sqlQuery = @"Select Orders.Id, Orders.OrderNumber,
                             Products.ProductName, Customers.FullName,
                             Orders.OrderDate, Orders.Price, Orders.Quantity,
                             Addresses.AddressLine1, Addresses.AddressLine2, 
                             Addresses.PinCode, Countries.CountryName,
                             AddressTypes.AddressTypeName
                             From Orders 
                             JOIN Customers ON Orders.CustomerId = Customers.Id
                             JOIN Addresses ON Orders.AddressId = Addresses.Id
                             JOIN Products ON Orders.ProductId = Products.Id
                             JOIN AddressTypes ON Addresses.AddressTypeID = AddressTypes.Id
                             JOIN Countries ON Addresses.CountryId = Countries.Id
                             Where 1=1";

                SqlDataAdapter sqlDataAdapter = new(sqlQuery, sqlConnection);
                if (customerId != null)
                {
                    sqlQuery += " And Orders.CustomerId = @customerId ";
                    sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@customerId", customerId);
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
                        OrderNumber = (int)dataTable.Rows[i]["OrderNumber"],
                        ProductName = (string)dataTable.Rows[i]["ProductName"],
                        FullName = (string)dataTable.Rows[i]["FullName"],
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

        public OrderDetail GetOrder(int orderNumber)
        {
            using(SqlConnection sqlConnection = new(_connectionString))
            {
                string sqlQuery = @"Select Orders.OrderNumber, 
                       Orders.Price, Orders.Quantity, Orders.OrderDate,
                       Products.ProductName, Products.ImageName, Customers.FullName,
                       Customers.Mobile, Addresses.AddressLine1,
                       Addresses.AddressLine2, Addresses.PinCode,
                       Countries.CountryName, AddressTypes.AddressTypeName 
                       FROM Orders 
                       JOIN Customers ON Orders.CustomerId = Customers.Id
                       JOIN Addresses ON Orders.AddressId = Addresses.Id
                       JOIN Products ON Orders.ProductId = Products.Id
                       JOIN AddressTypes ON Addresses.AddressTypeID = AddressTypes.Id
                       JOIN Countries ON Addresses.CountryId = Countries.Id
                       Where Orders.OrderNumber = @orderNumber ";

                SqlDataAdapter sqlDataAdapter = new(sqlQuery, sqlConnection);
                sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@orderNumber", orderNumber);

                DataTable dataTable = new();
                sqlDataAdapter.Fill(dataTable);

                if (dataTable.Rows.Count > 0)
                {
                    OrderDetail order = new()
                    {
                        OrderNumber = (int)dataTable.Rows[0]["OrderNumber"],
                        OrderDate = (DateTime)dataTable.Rows[0]["OrderDate"],
                        ProductName = (string)dataTable.Rows[0]["ProductName"],
                        ImageName = (string)dataTable.Rows[0]["ImageName"],
                        FullName = (string)dataTable.Rows[0]["FullName"],
                        Mobile = (string)dataTable.Rows[0]["Mobile"],
                        AddressLine1 = (string)dataTable.Rows[0]["AddressLine1"],
                        AddressLine2 = (string)dataTable.Rows[0]["AddressLine2"],
                        PinCode = (int)dataTable.Rows[0]["PinCode"],
                        CountryName = (string)dataTable.Rows[0]["CountryName"],
                        AddressTypeName = (string)dataTable.Rows[0]["AddressTypeName"],
                        Price = Convert.ToInt32(dataTable.Rows[0]["Price"]),
                        Quantity = (int)dataTable.Rows[0]["Quantity"]
                    };
                    return order;
                }
                return null;
            }
        }

        public int AddOrder(Order order)
        {
            using(SqlConnection sqlConnection = new(_connectionString))
            {
                string sqlQuery = @"Insert Into Orders 
                                  (OrderNumber, ProductId, CustomerId, OrderDate, Price, Quantity, AddressId)
                                  Values
                                  (@orderNumber, @productId, @customerId, GetDate(), @price, @quantity, @addressId)";

                SqlCommand sqlCommand = new(sqlQuery, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@OrderNumber", order.OrderNumber);
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
