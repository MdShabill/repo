using ShopEase.DataModels;
using ShopEase.ViewModels;
using System.Data.SqlClient;

namespace ShopEase.Repositories
{
    public class CardDetailRepository : ICardDetailRepository
    {
        private readonly string _connectionString;

        public CardDetailRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void AddCardDetail(CardDetail cardDetail)
        {
            using(SqlConnection sqlConnection = new(_connectionString))
            {
                string sqlQuery = @"Insert Into CardDetails 
                                  (OrderId, CustomerId, FullName, 
                                  CardNumber, ExpiryDate, CVV)
                                  Values
                                  (@orderId, @customerId, @fullName, 
                                  @cardNumber, @expiryDate, @cVV)";
                SqlCommand sqlCommand = new(sqlQuery, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@orderId", cardDetail.OrderId);
                sqlCommand.Parameters.AddWithValue("@customerId", cardDetail.CustomerId);
                sqlCommand.Parameters.AddWithValue("@fullName", cardDetail.NickName);
                sqlCommand.Parameters.AddWithValue("@cardNumber", cardDetail.CardNumber);
                sqlCommand.Parameters.AddWithValue("@expiryDate", cardDetail.ExpiryDate);
                sqlCommand.Parameters.AddWithValue("@cVV", cardDetail.CVV);

                sqlConnection.Open();
                int affectedRowCount = sqlCommand.ExecuteNonQuery();
                sqlConnection.Close();
            }
        }
    }
}
