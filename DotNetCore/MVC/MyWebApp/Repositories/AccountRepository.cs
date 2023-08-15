using MyWebApp.DataModel;
using MyWebApp.Enums;
using System.Data;
using Microsoft.Data.SqlClient;
namespace MyWebApp.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly string _connectionString;

        public AccountRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public Customer GetCustomerDetailsByEmailAndPassword(string email, string password)
        {
            using (SqlConnection sqlConnection = new(_connectionString))
            {
                SqlDataAdapter sqlDataAdapter = new(@"SELECT * FROM Customers
                        WHERE Email = @email AND Password = @password ", sqlConnection);
                sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@email", email);
                sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@password", password);
                DataTable dataTable = new();
                sqlDataAdapter.Fill(dataTable);

                if (dataTable.Rows.Count > 0)
                {
                    Customer customers = new()
                    {
                        Id = (int)dataTable.Rows[0]["Id"],
                        FirstName = (string)dataTable.Rows[0]["FirstName"],
                        LastName = (string)dataTable.Rows[0]["LastName"],
                        Gender = (GenderType)dataTable.Rows[0]["Gender"],
                        Email = (string)dataTable.Rows[0]["Email"],
                        Password = (string)dataTable.Rows[0]["Password"],
                        DateOfBirth = (DateTime)dataTable.Rows[0]["DateOfBirth"],
                        Mobile = Convert.ToString(dataTable.Rows[0]["Mobile"]),
                    };
                    return customers;
                }
                return null;
            }
        }
    }
}
