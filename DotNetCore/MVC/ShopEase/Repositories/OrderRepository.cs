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

        public OrderSummary GetLastOrderSummaryDetails()
        {
            using(SqlConnection sqlConnection = new(_connectionString))
            {
                string sqlQuery = @"SELECT top 1 
                            Orders.OrderNumber, Orders.OrderDate, 
                            Customers.FullName, Products.ImageName,
                            Addresses.AddressLine1, Addresses.AddressLine2,
                            Addresses.PinCode, Countries.CountryName,
                            Orders.Price, AddressTypes.AddressTypeName
                            FROM Orders 
                            JOIN Customers ON Orders.CustomerId = Customers.Id
                            JOIN Addresses ON Orders.AddressId = Addresses.Id
                            JOIN Products ON Orders.ProductId = Products.Id
                            JOIN AddressTypes ON Addresses.AddressTypeID = AddressTypes.Id
                            JOIN Countries ON Addresses.CountryId = Countries.Id
                            ORDER BY Orders.OrderDate DESC";

                SqlDataAdapter sqlDataAdapter = new(sqlQuery, sqlConnection);
                DataTable dataTable = new();
                sqlDataAdapter.Fill(dataTable);

                if (dataTable.Rows.Count > 0)
                {
                    OrderSummary orderSummary = new()
                    {
                        OrderNumber = (int)dataTable.Rows[0]["OrderNumber"],
                        OrderDate = (DateTime)dataTable.Rows[0]["OrderDate"],
                        Price = Convert.ToInt32(dataTable.Rows[0]["Price"]),
                        FullName = (string)dataTable.Rows[0]["FullName"],
                        AddressLine1 = (string)dataTable.Rows[0]["AddressLine1"],
                        AddressLine2 = (string)dataTable.Rows[0]["AddressLine2"],
                        PinCode = (int)dataTable.Rows[0]["PinCode"],
                        CountryName = (string)dataTable.Rows[0]["CountryName"],
                        AddressTypeName = (string)dataTable.Rows[0]["AddressTypeName"],
                        ImageName = (string)dataTable.Rows[0]["ImageName"]
                    };
                    return orderSummary;
                }
                return null;

            }
        }

        public int PlaceOrder(Order order)
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
