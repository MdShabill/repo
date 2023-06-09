using Microsoft.AspNetCore.Mvc;
using MyWebApp.Models;
using Microsoft.Data.SqlClient;
using System.Data;
using Microsoft.Identity.Client;

namespace MyWebApp.Controllers
{
    public class CustomerController : Controller
    {
        public readonly IConfiguration _Configuration;
        SqlConnection sqlConnection;

        public CustomerController(IConfiguration configuration)
        {
            _Configuration = configuration;
            sqlConnection = new(_Configuration.GetConnectionString("EcommerceDBConnection").ToString());
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View("Register");
        }

        [HttpPost]
        public IActionResult Register(Customer customer)
        {
            string sqlQuery = @"INSERT INTO Customers
                   (FirstName, LastName, Gender, 
                    Email, Age, Mobile)
                    VALUES 
                   (@FirstName, @LastName, @Gender, 
                    @Email, @Age, @Mobile)";
            SqlCommand sqlCommand = new(sqlQuery, sqlConnection);
            sqlCommand.Parameters.AddWithValue("@FirstName", customer.FirstName);
            sqlCommand.Parameters.AddWithValue("@LastName", customer.LastName);
            sqlCommand.Parameters.AddWithValue("@Gender", customer.Gender);
            sqlCommand.Parameters.AddWithValue("@Email", customer.Email);
            sqlCommand.Parameters.AddWithValue("@Age", customer.Age);
            sqlCommand.Parameters.AddWithValue("@Mobile", customer.Mobile);
            sqlConnection.Open();
            sqlCommand.ExecuteNonQuery();
            sqlConnection.Close();

            return View("RegisterSuccess");
        }

        [HttpPost]
        public IActionResult Update(Customer customer) 
        {
            string sqlQuery = @" UPDATE Customers SET 
                   FirstName = @FirstName, 
                   LastName = @LastName, 
                   Gender = @Gender, 
                   Email = @Email, 
                   Age = @Age, 
                   Mobile = @Mobile
                   WHERE Id = @Id ";
            SqlCommand sqlCommand = new(sqlQuery, sqlConnection);
            sqlCommand.Parameters.AddWithValue("@Id", customer.Id);
            sqlCommand.Parameters.AddWithValue("@FirstName", customer.FirstName);
            sqlCommand.Parameters.AddWithValue("@LastName", customer.LastName);
            sqlCommand.Parameters.AddWithValue("@Gender", customer.Gender);
            sqlCommand.Parameters.AddWithValue("@Email", customer.Email);
            sqlCommand.Parameters.AddWithValue("@Age", customer.Age);
            sqlCommand.Parameters.AddWithValue("@Mobile", customer.Mobile);
            sqlConnection.Open();
            sqlCommand.ExecuteNonQuery();
            sqlConnection.Close();
            
            return View("UpdateSuccess");
        }

        [HttpGet]
        public IActionResult GetAllCustomers() 
        {
            SqlDataAdapter sqlDataAdapter = new("SELECT * FROM Customers", sqlConnection);
            DataTable dataTable = new();
            sqlDataAdapter.Fill(dataTable);
            
            List<Customer> customers = new();
            
            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                Customer customer = new()
                {
                    Id = (int)dataTable.Rows[i]["Id"],
                    FirstName = (string)dataTable.Rows[i]["FirstName"],
                    LastName = (string)dataTable.Rows[i]["LastName"],
                    Gender = (int)dataTable.Rows[i]["Gender"],
                    Email = (string)dataTable.Rows[i]["Email"],
                    Age = (int)dataTable.Rows[i]["Age"],
                    Mobile = Convert.ToString(dataTable.Rows[i]["Mobile"]),
                };
                customers.Add(customer);
            }
            ViewBag.customerCount = customers.Count;
            return View("GetAllCustomers", customers);
        }

        [HttpGet]
        public IActionResult View(int Id)
        {
            SqlDataAdapter sqlDataAdapter = new($"SELECT * FROM Customers Where Id = @id", sqlConnection);
            sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@Id", Id);
            DataTable dataTable = new();
            sqlDataAdapter.Fill(dataTable);

            Customer customer = new()
            {
                Id = (int)dataTable.Rows[0]["Id"],
                FirstName = (string)dataTable.Rows[0]["FirstName"],
                LastName = (string)dataTable.Rows[0]["LastName"],
                Gender = (int)dataTable.Rows[0]["Gender"],
                Email = (string)dataTable.Rows[0]["Email"],
                Age = (int)dataTable.Rows[0]["Age"],
                Mobile = (string)dataTable.Rows[0]["Mobile"],
            };
            return View("View", customer);
        }

        [HttpGet]
        public IActionResult Edit(int Id)
        {
            SqlDataAdapter sqlDataAdapter = new($"SELECT * FROM Customers Where Id = @id", sqlConnection);
            sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@Id", Id);
            DataTable dataTable = new();
            sqlDataAdapter.Fill(dataTable);

            Customer customer = new()
            {
                Id = (int)dataTable.Rows[0]["Id"],
                FirstName = (string)dataTable.Rows[0]["FirstName"],
                LastName = (string)dataTable.Rows[0]["LastName"],
                Gender = (int)dataTable.Rows[0]["Gender"],
                Email = (string)dataTable.Rows[0]["Email"],
                Age = (int)dataTable.Rows[0]["Age"],
                Mobile = (string)dataTable.Rows[0]["Mobile"],
            };
            return View("Edit", customer);
        }

        [HttpGet]
        public IActionResult Delete(int id) 
        {
            string deleteQuery = "Delete From Customers Where Id = @Id";
            SqlCommand sqlCommand = new(deleteQuery, sqlConnection);
            sqlCommand.Parameters.AddWithValue("@Id", id);
            sqlConnection.Open();
            sqlCommand.ExecuteNonQuery();
            sqlConnection.Close();
        
            return View ("DeleteSuccess");
        }
    }  
}
