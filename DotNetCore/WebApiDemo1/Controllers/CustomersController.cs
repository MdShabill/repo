using Microsoft.AspNetCore.Mvc;
using System.Data;
using Newtonsoft.Json;
using WebApplication1.DTO.InputDTO;
using WebApiDemo1.Repositories;
using WebApiDemo1.Enums;
using System.Text.RegularExpressions;
using Microsoft.Data.SqlClient;
using System.Text;
using System.Security.Cryptography;
using WebApiDemo1.Helpers;

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
            List<CustomerDto> customers = _customerRepository.GetAllCustomersAsList();

            if (customers.Count > 0)
                return Ok(customers);
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
        [Route("GetCustomerById/{id}")]
        public IActionResult GetCustomerById(int id)
        {
            CustomerDto customer = _customerRepository.GetAllCustomerById(id);

            if (customer is not null)
                return Ok(customer);
            else
                return NotFound("No Record Found for given id");
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
        public IActionResult GetCustomersDetailByGenderByCountry(int gender, string country)
        {
            if (country.Length > 10)
                return BadRequest("country should not be more than of 10 characters");

            List<CustomerDto> customers = _customerRepository.GetCustomersDetailByGenderByCountry(gender, country);

            if (customers.Count > 0)
                return Ok(customers);
            else
                return NotFound();
        }

        [HttpGet]
        [Route("Login/{email}/{password}")]
        public IActionResult Login(string email, string password)
        {
            byte[] hashValuePassword = StringHelper.StringToByteArray(password);
            CustomerDto customer = _customerRepository.GetCustomerDetailsByEmailAndPassword(email, hashValuePassword);

            if (customer is null)
            {
                _customerRepository.UpdateOnLoginFailed(email);
                int aleadyFailedCountInDB = _customerRepository.GetLoginFailedCount(email);
                if (aleadyFailedCountInDB > 1) //2
                {
                    _customerRepository.UpdateIsLocked(email);
                }

                return NotFound("Invalid Email or Password");
            }

            if (customer.IsLocked == true)
                return Ok("your account has been locked, kindly contact system administrator");

            _customerRepository.UpdateOnLoginSuccessfull(email);
            return Ok("Login Successfull");
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
                {
                    if (ex.Message.Contains("UQ_Customers_Email"))
                        return BadRequest("Email already exist");

                    if (ex.Message.Contains("UQ_Customers_MobileNumber"))
                        return BadRequest("Mobile Number already exist");

                    else
                        return BadRequest("Some error at database side");
                }
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
                {
                    if (ex.Message.Contains("UQ_Customers_Email"))
                        return BadRequest("Email already exist");

                    if (ex.Message.Contains("UQ_Customers_MobileNumber"))
                        return BadRequest("Mobile Number already exist");

                    else
                        return BadRequest("Some error at database side");
                }
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

            else if(! Enum.IsDefined(typeof(GenderTypes), customer.Gender))
                errorMessage = "Invalid Gender";

            return errorMessage;
        }
    }
}
