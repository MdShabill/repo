using Microsoft.AspNetCore.Mvc;
using System.Data;
using Newtonsoft.Json;
using WebApiDemo1.DTO.InputDTO;
using WebApiDemo1.Repositories;
using WebApiDemo1.Enums;
using System.Text.RegularExpressions;
using Microsoft.Data.SqlClient;
using System.Text;
using System.Security.Cryptography;
using WebApiDemo1.Helpers;
using WebApiDemo1.DTO.InputDTO;
using AutoMapper;
using WebApiDemo1.DataModel;
using System.Transactions;
using System.Net;

namespace WebApiDemo1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {

        ICustomerRepository _customerRepository;
        IAddressRepository _addressRepository;
        IMapper _imapper;

        public CustomersController(ICustomerRepository customerRepository, IAddressRepository addressRepository)
        {
            _customerRepository = customerRepository;
            _addressRepository = addressRepository;

            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<AddressDto, Address>();
                cfg.CreateMap<CustomerDto, Customer>();
                cfg.CreateMap<Customer, CustomerDto>();
            });

            _imapper = configuration.CreateMapper();
        }

        [HttpGet]
        [Route("GetAllCustomers")]
        public IActionResult GetAllCustomers()
        {
            List<Customer> customers = _customerRepository.GetAllCustomersAsList();
        
            List<CustomerDto> customerdto = _imapper.Map<List<Customer>, List<CustomerDto>>(customers);
        
            for (int i = 0; i < customers.Count; i++)
            {
                customerdto[i].FullName = customers[i].FirstName + ", " + customers[i].LastName;
            }
        
            if (customerdto.Count > 0)
                return Ok(customerdto);
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
            Customer customers = _customerRepository.GetCustomerById(id);

            //Approach 2 With AutoMapper
            CustomerDto customerdto = _imapper.Map<Customer, CustomerDto>(customers);

            customerdto.FullName = customers.FirstName + ", " + customers.LastName; 
            
            //Approach 1 without automapper
            //CustomerDto customerdto = new()
            //{
            //    Id = customers.Id,
            //    FullName = customers.FirstName + ", " + customers.LastName,
            //    Gender = customers.Gender,
            //    Age = customers.Age,
            //    Email = customers.Email,
            //};
            return Ok(customerdto);
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
        [Route("ChangePassword")]
        public IActionResult ChangePassword([FromBody] ChangePasswordDto changePassword)
        {
            try
            {
                if (changePassword.NewPassword != changePassword.ReEnterPassword)
                    return BadRequest("Re-entered password does not match with new password");

                if (changePassword.NewPassword == changePassword.CurrentPassword)
                    return BadRequest("New password cannot be same as current password");

                byte[] hashValueCurrentPassword = StringHelper.StringToByteArray(changePassword.CurrentPassword);
                CustomerDto customer = _customerRepository.GetCustomerDetailsByEmailAndPassword(changePassword.Email, hashValueCurrentPassword);

                if (ModelState.IsValid)
                {
                    if (customer is null)
                        return BadRequest("Email password or account does not exist ");

                    byte[] hashValueNewPassword = StringHelper.StringToByteArray(changePassword.NewPassword);
                    _customerRepository.UpdateNewPassword(changePassword.Email, hashValueNewPassword);
                    return Ok("New password updated");
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

        //[HttpGet]
        //[Route("ForgetPassword/{email}/{password}")]
        //public IActionResult ForgetPassword(string email, string password) 
        //{

        //}

        [HttpPost]
        [Route("Register")]
        public IActionResult Register([FromBody] CustomerDto customerDto)
        {
            byte[] hashValuePassword = StringHelper.StringToByteArray(customerDto.Password);
            customerDto.HashValuePassword = hashValuePassword;

            try
            {
                string errorMessage = validateCustomerRegisterOrUpdate(customerDto);
                if (!string.IsNullOrEmpty(errorMessage))
                    return BadRequest(errorMessage);

                if (ModelState.IsValid)
                {
                    //using (TransactionScope transactionScope = new TransactionScope())
                    //using (var transactionScope = new TransactionScope())
                    using (TransactionScope transactionScope = new ())
                    {
                        try
                        {
                            Customer customer = _imapper.Map<CustomerDto, Customer>(customerDto);
                            customer.Id = _customerRepository.Add(customer);

                            //Approach #1
                            //for(int i = 0; i < customerDto.Addresses.Count; i++)
                            //{
                            //    Address address = _imapp
                            //    er.Map<AddressDto, Address>(customerDto.Addresses[i]);
                            //    address.CustomerId = customer.Id;
                            //    _addressRepository.Add(address);
                            //}
                            //Approach #2
                            foreach (AddressDto addressDto in customerDto.Addresses)
                            {
                                Address address = _imapper.Map<AddressDto, Address>(addressDto);
                                address.CustomerId = customer.Id;

                                //AddressRepository addressRepository = new();

                                _addressRepository.Add(address);
                            }
                            transactionScope.Complete();
                            return Ok(customer.Id);
                        }
                        catch (TransactionException ex)
                        {
                            transactionScope.Dispose();
                        }
                    }                  
                }
                return BadRequest();
            }
            catch (SqlException ex)
            {
                if (ex.Number == Constants.UniqueConstraintViolationErrorcode)
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
        [Route("Update")]
        public IActionResult Update([FromBody] CustomerDto customerDto)
        {
            byte[] hashValuePassword = StringHelper.StringToByteArray(customerDto.Password);
            customerDto.HashValuePassword = hashValuePassword;

            try
            {
                string errorMessage = validateCustomerRegisterOrUpdate(customerDto, true);
                if (!string.IsNullOrEmpty(errorMessage))
                    return BadRequest(errorMessage);

                if (ModelState.IsValid)
                {
                    using (var transactionScope = new TransactionScope())
                    {
                        try
                        {
                            Customer customer = _imapper.Map<CustomerDto, Customer>(customerDto);
                            _customerRepository.Update(customer);

                            foreach (AddressDto addressDto in customerDto.Addresses)
                            {
                                if(addressDto != null)
                                {
                                    Address address = _imapper.Map<AddressDto, Address>(addressDto);
                                    address.CustomerId = customer.CustomerId;
                                    _addressRepository.Add(address);
                                    _addressRepository.Update(address);
                                }
                            }
                            transactionScope.Complete();
                            return Ok("Record updated");
                        }
                        catch (Exception ex)
                        {
                            transactionScope.Dispose();
                            return BadRequest(ex.Message);
                        }
                    }
                }
                return BadRequest("Record not updated");
            }
            catch (SqlException ex)
            {
                if (ex.Number == Constants.UniqueConstraintViolationErrorcode)
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

        private string validateCustomerRegisterOrUpdate(CustomerDto customerDto, bool isUpdate = false)
        {
            string errorMessage = "";

            customerDto.FullName = customerDto.FullName.Trim();
            //customerDto.Country = customerDto.Country.Trim();

            //if (isUpdate == true)
            //{
            //    if (customerDto.CustomerId < 1)
            //        errorMessage = "Id can not be less than 0";
            //}

            if (string.IsNullOrWhiteSpace(customerDto.FullName))
                errorMessage = "FullName can not be blank";

            else if (customerDto.FullName.Length < 3 || customerDto.FullName.Length > 30)
                errorMessage = "FullName should be between 3 and 30 characters.";

            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            Match match = regex.Match(customerDto.Email);
            if (!match.Success)
                errorMessage = "Email is invalid";

            else if (customerDto.Age <= 18)
                errorMessage = "Invalid age, customer age should be above 18";

            else if(! Enum.IsDefined(typeof(GenderTypes), customerDto.Gender))
                errorMessage = "Invalid Gender";

            //else if (!Enum.IsDefined(typeof(AddressTypes), customerDto.AddressType))
                //errorMessage = "Invalid Address Type";

            return errorMessage;
        }
    }
}
