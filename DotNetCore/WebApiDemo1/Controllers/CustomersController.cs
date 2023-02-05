using Microsoft.AspNetCore.Mvc;
using System.Data;
using Newtonsoft.Json;
using WebApplication1.DTO.InputDTO;
using WebApiDemo1.Repositories;
using WebApiDemo1.Enums;
using System.Text.RegularExpressions;
using Microsoft.Data.SqlClient;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        ICustomerRepository _customerRepository;

        public CustomersController(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        [HttpGet]
        [Route("GetAllCustomers")]
        public IActionResult GetAllCustomers()
        {
            DataTable dataTable = _customerRepository.GetAllCustomers();

            if (dataTable.Rows.Count > 0)
                return Ok(JsonConvert.SerializeObject(dataTable));
            else
                return NotFound();
        }

        [HttpGet]
        [Route("GetCustomersCount")]
        public IActionResult GetCustomersCount()
        {
            int customerCount = _customerRepository.GetCustomersCount();
            return Ok(customerCount);
        }

        [HttpGet]
        [Route("GetCustomerDetailById/{CustomerId}")]
        public IActionResult GetCustomerDetailById(int customerId)
        {
            if (customerId < 1)
                return BadRequest("Customer id should be greater than 0");

            DataTable dataTable = _customerRepository.GetCustomerDetailById(customerId);

            if (dataTable.Rows.Count > 0)
                return Ok(JsonConvert.SerializeObject(dataTable));
            else
                return NotFound();
        }

        [HttpGet]
        [Route("GetCustomerFullNameById/{CustomerId}")]
        public IActionResult GetCustomerFullNameById(int customerId)
        {
            if (customerId < 1)
                return BadRequest("Customer id should be greater than 0");

            string customerFullName = _customerRepository.GetCustomerFullNameById(customerId);
            return Ok(customerFullName);
        }

        [HttpGet]
        [Route("GetCustomersDetailByGenderByCountry/{gender}/{country}")]
        public IActionResult GetCustomersDetailByGenderByCountry(string gender, string country)
        {
            if (gender.Length > 6)
                return BadRequest("Gender should not be more than of 6 characters");

            if (country.Length > 10)
                return BadRequest("country should not be more than of 10 characters");

            DataTable dataTable = _customerRepository.GetCustomersDetailByGenderByCountry(gender, country);

            if (dataTable.Rows.Count > 0)
                return Ok(JsonConvert.SerializeObject(dataTable));
            else
                return NotFound();
        }

        [HttpPost]
        [Route("CustomerRegister")]
        public IActionResult CustomerRegister([FromBody] CustomerDto customer)
        {
            try
            {
                string errorMessage = validateCustomerRegisterOrUpdate(customer);
                if (!string.IsNullOrEmpty(errorMessage))
                    return BadRequest(errorMessage);

                if (ModelState.IsValid)
                {
                    int id = _customerRepository.Add(customer);
                    return Ok(id);
                }
                return BadRequest();
            }
            catch (SqlException ex)
            {
                if (ex.Number == 2627)
                    return BadRequest("Email already exist");
                else
                    return BadRequest("Some error at database side");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", @"Unable to save changes. 
                    Try again, and if the problem persists 
                    see your system administrator.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        [Route("CustomerUpdate")]
        public IActionResult CustomerUpdate([FromBody] CustomerDto customer)
        {
            try
            {
                string errorMessage = validateCustomerRegisterOrUpdate(customer, true);
                if (!string.IsNullOrEmpty(errorMessage))
                    return BadRequest(errorMessage);

                if (ModelState.IsValid)
                {
                    _customerRepository.Update(customer);
                    return Ok("Record updated");
                }
                return BadRequest("Record not updated");
            }
            catch (SqlException ex)
            {
                if (ex.Number == 2627)
                    return BadRequest("Email already exist");
                else
                    return BadRequest("Some error at database side");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", @"Unable to save changes. 
                    Try again, and if the problem persists 
                    see your system administrator.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        private string validateCustomerRegisterOrUpdate(CustomerDto customer, bool isUpdate = false)
        {
            string errorMessage = "";

            customer.FullName = customer.FullName.Trim();
            customer.Country = customer.Country.Trim();

            if (isUpdate == true)
            {
                if (customer.Id < 1)
                    errorMessage = "Id can not be less than 0";
            }

            if (string.IsNullOrWhiteSpace(customer.FullName))
                errorMessage = "FullName can not be blank";

            else if (customer.FullName.Length < 3 || customer.FullName.Length > 30)
                errorMessage = "FullName should be between 3 and 30 characters.";

            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            Match match = regex.Match(customer.Email);
            if (!match.Success)
                errorMessage = "Email is invalid";

            else if (customer.Age <= 18)
                errorMessage = "Invalid age, customer age should be above 18";

            else if (string.IsNullOrWhiteSpace(customer.Country))
                errorMessage = "Country can not be blank";

            else if(! Enum.IsDefined(typeof(GenderType), customer.Gender))
                errorMessage = "Invalid Gender";

            return errorMessage;
        }
    }
}
