﻿using ShopEase.DataModels.Cart;
using ShopEase.DataModels.OderItem;
using ShopEase.DataModels.Order;
using ShopEase.ViewModels;
using System.Data;
using System.Data.Common;
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
            using (SqlConnection sqlConnection = new(_connectionString))
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

        public int AddOrder(Order order)
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                string stringQuery = @"INSERT INTO Orders 
                            (OrderNumber, CustomerId, OrderDate, 
                            Price, AddressId)
                            VALUES
                            (@orderNumber, @customerId, GETDATE(),
                            @price, @addressId)
                            Select Scope_Identity()";

                SqlCommand sqlCommand = new(stringQuery, sqlConnection);

                sqlCommand.Parameters.AddWithValue("@orderNumber", order.OrderNumber);
                sqlCommand.Parameters.AddWithValue("@customerId", order.CustomerId);
                sqlCommand.Parameters.AddWithValue("@price", order.Price);
                sqlCommand.Parameters.AddWithValue("@addressId", order.AddressId);

                sqlConnection.Open();
                order.OrderId = Convert.ToInt32(sqlCommand.ExecuteScalar());
                sqlConnection.Close();

                return order.OrderId;
            }
        }

        public void AddOrderItem(OrderItem orderItem, int orderId, string orderNumber)
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                string orderItemQuery = @"INSERT INTO OrderItems 
                         (OrderId, OrderNumber, ProductId, Quantity)
                         VALUES
                         (@orderId, @orderNumber, @productId, @quantity)";

                SqlCommand orderItemCommand = new(orderItemQuery, sqlConnection);
                orderItemCommand.Parameters.AddWithValue("@orderId", orderId);
                orderItemCommand.Parameters.AddWithValue("@orderNumber", orderNumber);
                orderItemCommand.Parameters.AddWithValue("@productId", orderItem.ProductId);
                orderItemCommand.Parameters.AddWithValue("@quantity", orderItem.Quantity);

                sqlConnection.Open();
                orderItemCommand.ExecuteNonQuery();
                sqlConnection.Close();
            }
        }

        public void AddOrderItem(List<Cart> cartItems, int orderId, string orderNumber)
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                for (int i = 0; i < cartItems.Count; i++)
                {
                    string insertQuery = @"
                            Insert Into OrderItems (OrderId, OrderNumber, ProductId, Quantity)
                                        Values  (@orderId, @orderNumber, @productId, @quantity)";
                    SqlCommand sqlCommand = new(insertQuery, sqlConnection);
                    sqlCommand.Parameters.AddWithValue("@orderId", orderId);
                    sqlCommand.Parameters.AddWithValue("@orderNumber", orderNumber);
                    sqlCommand.Parameters.AddWithValue("@productId", cartItems[i].ProductId);
                    sqlCommand.Parameters.AddWithValue("@quantity", cartItems[i].Quantity);

                    sqlConnection.Open();
                    sqlCommand.ExecuteNonQuery();
                    sqlConnection.Close();
                }
            }
        }

        public void UpdateProductQuantity(int productId, int orderQuantity)
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            { 
                string updateQuery = @"UPDATE Products
                     SET Quantity = Quantity - @orderQuantity
                     WHERE Id = @productId";

                SqlCommand sqlUpdatCommand = new(updateQuery, sqlConnection);

                sqlUpdatCommand.Parameters.AddWithValue("@orderQuantity", orderQuantity);
                sqlUpdatCommand.Parameters.AddWithValue("@productId", productId);

                sqlConnection.Open();
                sqlUpdatCommand.ExecuteNonQuery();
                sqlConnection.Close();
            }
        }

    }
}
