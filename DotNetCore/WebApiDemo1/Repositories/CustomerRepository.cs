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

        public int Register(CustomerDto customer)
        {
            string sqlQuery = @"INSERT INTO Customers(Name, Gender, Age, Country)
                                        VALUES (@FullName, @Gender, @Age, @Country)
                                        Select Scope_Identity() "
            ;

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
    }
}
