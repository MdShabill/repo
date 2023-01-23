using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using System.Configuration;
using System.Data;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using System.Reflection;
using WebApplication1.DTO.InputDTO;
using WebApiDemo1.Repositories;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        public readonly IConfiguration _Configuration;
        SqlConnection sqlConnection;

        public CustomersController(IConfiguration configuration)
        {
            _Configuration = configuration;
            sqlConnection = new(_Configuration.GetConnectionString("CustomerDBConnection").ToString());
        }

        [HttpGet]
        [Route("GetAllCustomers")]
        public IActionResult GetAllCustomers()
        {
            CustomerRepository customerRepository = new(_Configuration);
            DataTable dataTable = customerRepository.GetAllCustomers();

            if (dataTable.Rows.Count > 0)
            {
                return Ok(JsonConvert.SerializeObject(dataTable));
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet]
        [Route("GetCustomersCount")]
        public IActionResult GetCustomersCount()
        {
            CustomerRepository customerRepository = new(_Configuration);
            int customerCount = customerRepository.GetCustomersCount();
            return Ok(customerCount);
        }

        [HttpGet]
        [Route("GetCustomerFullNameById/{CustomerId}")]
        public IActionResult GetCustomerFullNameById(int customerId)
        {
            if (customerId < 1)
            {
                return BadRequest("Customer id should be greater than 0");
            }

            CustomerRepository customerRepository = new(_Configuration);
            string fullName = customerRepository.GetCustomerFullNameById(customerId);
            return Ok(fullName);
        }

        [HttpGet]
        [Route("GetCustomersDetailByGenderByCountry/{gender}/{country}")]
        public IActionResult GetCustomersDetailByGenderByCountry(string gender, string country)
        {
            if (gender.Length > 6)
            {
                return BadRequest("Gender should not be more than 6 characters");
            }

            country = country.Trim();
            if (country.Length > 10)
            {
                return BadRequest("Country should not be more than 10 characters");
            }

            CustomerRepository customerRepository = new(_Configuration);
            DataTable dataTable = customerRepository.GetCustomersDetailByGenderByCountry(gender, country);

            if (dataTable.Rows.Count > 0)
            {
                return Ok(JsonConvert.SerializeObject(dataTable));
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet]
        [Route("GetCustomersDetailByNameByCountry/{Name}/{Country?}")]
        public IActionResult GetCustomersDetailByNameByCountry(string name, string? country)
        {
            if (name.Length < 3 || name.Length > 20)
            {
                return BadRequest("Customer name should be between 3 and 20 characters.");
            }

            CustomerRepository customerRepository = new(_Configuration);
            DataTable dataTable = customerRepository.GetCustomersDetailByNameByCountry(name, country);

            if (dataTable.Rows.Count > 0)
            {
                return Ok(JsonConvert.SerializeObject(dataTable));
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost]
        [Route("CustomerRegister")]
        public IActionResult CustomerRegister([FromBody] CustomerDto customer)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (string.IsNullOrWhiteSpace(customer.FullName))
                    {
                        return BadRequest("Customer fullName can not be blank");
                    }
                    customer.FullName = customer.FullName.Trim();
                    if (customer.FullName.Length < 3 || customer.FullName.Length > 20)
                    {
                        return BadRequest("Customer full name should be between 3 and 20 characters");
                    }

                    if (string.IsNullOrWhiteSpace(customer.Gender))
                    {
                        return BadRequest("Customer gender can not be blank");
                    }

                    if (customer.Age < 21)
                    {
                        return BadRequest("Invalid customer age should be above 21");
                    }

                    customer.Country = customer.Country.Trim();
                    if (customer.Country.Length < 3 || customer.Country.Length > 20)
                    {
                        return BadRequest("Country name should be between 3 and 20 characters");
                    }

                    CustomerRepository customerRepository = new(_Configuration);
                    int id = customerRepository.Add(customer);

                    return Ok(id);
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", @"Unable to save changes. 
                    Try again, and if the problem persists 
                    see your system administrator.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}








