using Html_JsDemo.DatModels;
using System.Data.SqlClient;

namespace Html_JsDemo.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly string _connectionString;

        public CustomerRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public int Register(Customer customer)
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                string sqlQuery = @"Insert Into Customers
                       (FullName, Email, Gender, Age)
                       Values
                       (@fullName, @email, @gender, @age)";

                SqlCommand sqlCommand = new(sqlQuery, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@fullName", customer.FullName);
                sqlCommand.Parameters.AddWithValue("@email", customer.Email);
                sqlCommand.Parameters.AddWithValue("@gender", customer.Gender);
                sqlCommand.Parameters.AddWithValue("@age", customer.Age);

                sqlConnection.Open();
                int affectedRowCount = sqlCommand.ExecuteNonQuery();
                sqlConnection.Close();

                return affectedRowCount;
            }
        }
    }
}
