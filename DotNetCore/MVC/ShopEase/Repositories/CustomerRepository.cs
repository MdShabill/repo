using ShopEase.DataModels;
using System.Data;
using System.Data.SqlClient;

namespace ShopEase.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly string _connectionString;

        public CustomerRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<Customer> GetAll()
        {
            using(SqlConnection sqlConnection = new(_connectionString))
            {
                string sqlQuery = @"Select 
                           FullName, Mobile,  
                           Gender, Email
                           From 
                           Customers";
                SqlDataAdapter sqlDataAdapter = new(sqlQuery, sqlConnection);
                DataTable dataTable = new();
                sqlDataAdapter.Fill(dataTable);

                List<Customer> customers = new();

                for(int i = 0; i < dataTable.Rows.Count; i++)
                {
                    Customer customer = new()
                    {
                        FullName = (string)dataTable.Rows[i]["FullName"],
                        Mobile = (string)dataTable.Rows[i]["Mobile"],
                        Gender = (int)dataTable.Rows[i]["Gender"],
                        Email = (string)dataTable.Rows[i]["Email"],
                    };
                    customers.Add(customer);
                }
                return customers;
            }
        }

        public int Register(Customer customer)
        {
            using(SqlConnection sqlConnection = new(_connectionString)) 
            {
                string sqlQuery = @"Insert Into Customers
                       (FullName, Mobile, Gender, Email, Password)
                       Values
                       (@fullName, @mobile, @gender, @email, @password)";

                SqlCommand sqlCommand = new(sqlQuery, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@fullName", customer.FullName);
                sqlCommand.Parameters.AddWithValue("@mobile", customer.Mobile);
                sqlCommand.Parameters.AddWithValue("@gender", customer.Gender);
                sqlCommand.Parameters.AddWithValue("@email", customer.Email);
                sqlCommand.Parameters.AddWithValue("@password", customer.Password);

                sqlConnection.Open();
                int affectedRowCount = sqlCommand.ExecuteNonQuery();
                sqlConnection.Close();

                return affectedRowCount;
            }
        }
    }
}
