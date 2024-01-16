using ShopEase.DataModels.Cart;
using ShopEase.DataModels.Product;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;

namespace ShopEase.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly string _connectionString;

        public CartRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<Cart> GetAll(int? customerId)
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                string sqlQuery = @"Select Carts.ProductId, Carts.CustomerId, Carts.Quantity,
                                    Products.Title, Products.ImageName, Products.Price
                                    From Carts
                                    Inner Join Products On Carts.ProductId = Products.Id
                                    Where 1=1";

                SqlDataAdapter sqlDataAdapter = new(sqlQuery, sqlConnection);
                if (customerId != null)
                {
                    sqlQuery += " And Carts.CustomerId = @customerId ";
                    sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@customerId", customerId);
                }

                sqlDataAdapter.SelectCommand.CommandText = sqlQuery;
                DataTable dataTable = new();
                sqlDataAdapter.Fill(dataTable);

                List<Cart> carts = new();

                for(int i = 0; i < dataTable.Rows.Count; i++)
                {
                    Cart cart = new()
                    {
                        ProductId = (int)dataTable.Rows[i]["ProductId"],
                        Title = (string)dataTable.Rows[i]["Title"],
                        ImageName = (string)dataTable.Rows[i]["ImageName"],
                        CustomerId = (int)dataTable.Rows[i]["CustomerId"],
                        Price = Convert.ToInt32(dataTable.Rows[i]["Price"]),
                        Quantity = (int)dataTable.Rows[i]["Quantity"],
                    };
                    carts.Add(cart);
                }
                return carts;
            }
        }

        public int Add(Cart cart)
        {
            using(SqlConnection sqlConnection = new (_connectionString))
            {
                string sqlQuery = @"Insert Into Carts(CustomerId, ProductId, Quantity, AddDate)
                                    Values(@customerId, @productId, @quantity, GetDate())";

                SqlCommand sqlCommand = new(sqlQuery, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@customerId", cart.CustomerId);
                sqlCommand.Parameters.AddWithValue("@ProductId", cart.ProductId);
                sqlCommand.Parameters.AddWithValue("@quantity", cart.Quantity);

                sqlConnection.Open();
                int affectedRowCount = sqlCommand.ExecuteNonQuery();
                sqlConnection.Close();

                return affectedRowCount;
            }
        }

        public void Delete(int customerId)
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                string sqlQuery = @"Delete From Carts
                                    Where CustomerId = @customerId";
                SqlCommand sqlCommand = new(sqlQuery, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@customerId", customerId);

                sqlConnection.Open();
                sqlCommand.ExecuteNonQuery();
                sqlConnection.Close();
            }
        }
    }
}
