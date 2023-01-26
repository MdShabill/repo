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
        
        public CustomersController(IConfiguration configuration)
        {
            _Configuration = configuration;
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
                string errorMessage = validateCustomerRegisterOrUpdate(customer);
                if (!string.IsNullOrEmpty(errorMessage))
                {
                    return BadRequest(errorMessage);
                }

                if (ModelState.IsValid)
                {
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

        private string validateCustomerRegisterOrUpdate(CustomerDto customerDto, bool isUpdate = false)
        {
            string errorMessage = "";
            
            customerDto.FullName = customerDto.FullName.Trim();
            customerDto.Gender = customerDto.Gender.Trim();
            customerDto.Country = customerDto.Country.Trim();

            // Approach 1
            if (isUpdate == true)
            {
                if (customerDto.Id < 1)
                {
                    errorMessage = "Id can not be less than 0";
                }
            }

            // Approach 2
            //if (isUpdate != false)
            //{
            //    if (customerDto.Id < 1)
            //    {
            //        errorMessage = "Id can not be less than 0";
            //    }
            //}

            // Approach 3
            //if (isUpdate == true && customer.Id < 1)
            //{
            //    errorMessage = "Id can not be less than 0";
            //}


            if (string.IsNullOrWhiteSpace(customerDto.FullName))
            {
                errorMessage = "FullName can not be blank";
            }
            else if (customerDto.FullName.Length < 3 || customerDto.FullName.Length > 30)
            {
                errorMessage = "FullName should be between 3 and 30 characters.";
            }
            else if (customerDto.Age <= 18)
            {
                errorMessage = "Invalid age, customer age should be above 18";
            }
            else if (string.IsNullOrWhiteSpace(customerDto.Country))
            {
                errorMessage = "Country can not be blank";
            }
            else if (string.IsNullOrWhiteSpace(customerDto.Gender))
            {
                errorMessage = "Gender can not be blank";
            }
            return errorMessage;
        }

        [HttpPost]
        [Route("CustomerUpdate")]
        public IActionResult CustomerUpdate([FromBody] CustomerDto customer)
        {
            try
            {
                string errorMessage = validateCustomerRegisterOrUpdate(customer, true);
                if (!string.IsNullOrEmpty(errorMessage))
                {
                    return BadRequest(errorMessage);
                }

                if (ModelState.IsValid)
                {
                    CustomerRepository customerRepository = new(_Configuration);
                    customerRepository.Update(customer);
                    return Ok("Record updated");
                }
                return BadRequest("Record not updated");
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








