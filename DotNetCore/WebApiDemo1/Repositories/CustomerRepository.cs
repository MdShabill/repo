using Microsoft.Data.SqlClient;
using System.Data;
using WebApplication1.DTO.InputDTO;


namespace WebApiDemo1.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        public readonly IConfiguration _Configuration;
        SqlConnection sqlConnection;

        public CustomerRepository(IConfiguration configuration)
        {
            _Configuration = configuration;
            sqlConnection = new(_Configuration.GetConnectionString("CustomerDBConnection").ToString());
        }

        public DataTable GetAllCustomers()
        {
            SqlDataAdapter sqlDataAdapter = new("SELECT * FROM Customers", sqlConnection);
            DataTable dataTable = new();
            sqlDataAdapter.Fill(dataTable);
            return dataTable;
        }

        public int GetCustomersCount()
        {
            string sqlQuery = "SELECT COUNT(*) FROM Customers";
            SqlCommand sqlCommand = new(sqlQuery, sqlConnection);
            sqlConnection.Open();
            int customerCount = Convert.ToInt32(sqlCommand.ExecuteScalar());
            sqlConnection.Close();
            return customerCount;
        }

        public string GetCustomerFullNameById(int customerId)
        {
            string sqlQuery = "SELECT Name FROM Customers where id = @customerId";
            SqlCommand sqlCommand = new(sqlQuery, sqlConnection);
            sqlCommand.Parameters.AddWithValue("@customerId", customerId);
            sqlConnection.Open();
            string fullName = Convert.ToString(sqlCommand.ExecuteScalar());
            sqlConnection.Close();
            return fullName;
        }

        public DataTable GetCustomersDetailByGenderByCountry(string gender, string country)
        {
            SqlDataAdapter sqlDataAdapter = new(@"SELECT * FROM Customers 
                    WHERE Gender = @gender AND Country = @country ", sqlConnection);

            sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@gender", gender);
            sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@country", country);
            DataTable dataTable = new();
            sqlDataAdapter.Fill(dataTable);
            return dataTable;

        }

        public DataTable GetCustomersDetailByNameByCountry(string name, string country)
        {
            string sqlQuery = "SELECT * FROM Customers WHERE Name = @name ";

            if (!string.IsNullOrWhiteSpace(country))
            {
                sqlQuery += "AND Country = @country ";
            }

            SqlDataAdapter sqlDataAdapter = new(sqlQuery, sqlConnection);
            sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@name", name);
            sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@country", country);
            DataTable dataTable = new();
            sqlDataAdapter.Fill(dataTable);
            return dataTable;
        }

        public int Add(CustomerDto customer)
        {
            string sqlQuery = @"INSERT INTO Customers(Name, Gender, Age, Country)
                    VALUES (@FullName, @Gender, @Age, @Country)
                    Select Scope_Identity() ";

            SqlCommand sqlCommand = new(sqlQuery, sqlConnection);
            sqlCommand.Parameters.AddWithValue("@FullName", customer.FullName);
            sqlCommand.Parameters.AddWithValue("@Gender", customer.Gender);
            sqlCommand.Parameters.AddWithValue("@Age", customer.Age);
            sqlCommand.Parameters.AddWithValue("@Country", customer.Country);
            sqlConnection.Open();
            customer.Id = Convert.ToInt32(sqlCommand.ExecuteScalar());
            sqlConnection.Close();
            return customer.Id;

        }

        public void Update(CustomerDto customer)
        {
            string sqlQuery = @" UPDATE Customers SET Name = @FullName, Gender = @Gender,
                    Age = @Age, Country = @Country
                    WHERE Id = @Id ";
            SqlCommand sqlCommand = new(sqlQuery, sqlConnection);
            sqlCommand.Parameters.AddWithValue("@Id", customer.Id);
            sqlCommand.Parameters.AddWithValue("@FullName", customer.FullName);
            sqlCommand.Parameters.AddWithValue("@Gender", customer.Gender);
            sqlCommand.Parameters.AddWithValue("@Age", customer.Age);
            sqlCommand.Parameters.AddWithValue("@Country", customer.Country);
            sqlConnection.Open();
            sqlCommand.ExecuteNonQuery();
            sqlConnection.Close();
        }

    }
}
