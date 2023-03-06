using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using WebApiDemo1.Repositories;
using WebApiDemo1.DTO.InputDTO;
using WebApiDemo1.DTO.InputDTO.BankAccountServicesDTO;
using WebApiDemo1.DataModel;
using WebApiDemo1.Enums;
using AutoMapper;

namespace WebApiDemo1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BankAccountServicesController : ControllerBase
    {
        IBankAccountServiceRepository _bankAccountServiceRepository;

        public BankAccountServicesController(IBankAccountServiceRepository bankAccountServiceRepository)
        {
            _bankAccountServiceRepository = bankAccountServiceRepository;
        }

        [HttpPost]
        [Route("CreateSavingsAccount")]
        public IActionResult CreateSavingsAccount([FromBody] SavingsAccountDto savingsAccountDto)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    // var bankAccount = new BankAccount
                    //{
                    //    BankName = savingsAccount.BankName,
                    //    BranchName = savingsAccount.BranchName,
                    //    IfscCode = savingsAccount.IfscCode,
                    //    AccountNumber = savingsAccount.AccountNumber,
                    //    AccountType = Enums.AccountTypes.SavingsAccount,
                    //    AccountHolder1Name = savingsAccount.AccountHolder1Name,
                    //    Holder1Email = savingsAccount.Holder1Email,
                    //    Holder1Address = savingsAccount.Holder1Address,
                    //};

                    //Approach 2 - AutoMapper
                    var config = new MapperConfiguration(cfg =>
                    {
                        cfg.CreateMap<SavingsAccountDto, BankAccount>();
                    });

                    IMapper iMapper = config.CreateMapper();
                    BankAccount bankAccount = iMapper.Map<SavingsAccountDto, BankAccount>(savingsAccountDto);
                    bankAccount.AccountType = AccountTypes.SavingsAccount;

                    int savingsAccountId = _bankAccountServiceRepository.Add(bankAccount);
                    return Ok(savingsAccountId);
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

        [HttpPost]
        [Route("CreateCurrentAccount")]
        public IActionResult CreateCurrentAccount([FromBody] CurrentAccountDto currentAccountDto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //var bankAccount = new BankAccount
                    //{
                    //    BankName = currentAccount.BankName,
                    //    BranchName = currentAccount.BranchName,
                    //    IfscCode = currentAccount.IfscCode,
                    //    AccountNumber = currentAccount.AccountNumber,
                    //    AccountType = Enums.AccountTypes.CurrentAccount,
                    //    AccountHolder1Name = currentAccount.AccountHolder1Name,
                    //    Holder1Email = currentAccount.Holder1Email,
                    //    Holder1Address = currentAccount.Holder1Address,
                    //    CompanyName = currentAccount.CompanyName,
                    //    GSTNo = currentAccount.GSTNo,
                    //};

                    //Approach 2 - AutoMapper 
                    var config = new MapperConfiguration(cfg =>
                    {
                        cfg.CreateMap<CurrentAccountDto, BankAccount>();
                    });

                    IMapper iMapper = config.CreateMapper();
                    BankAccount bankAccount = iMapper.Map<CurrentAccountDto, BankAccount>(currentAccountDto);
                    bankAccount.AccountType = AccountTypes.CurrentAccount;

                    int currentAccountId = _bankAccountServiceRepository.Add(bankAccount);
                    return Ok(currentAccountId);
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

        [HttpPost]
        [Route("CreateJointAccount")]
        public IActionResult CreateJointAccount([FromBody] JointAccountDto jointAccountDto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //var bankAccount = new BankAccount
                    //{
                    //    BankName = jointAccount.BankName,
                    //    BranchName = jointAccount.BranchName,
                    //    IfscCode = jointAccount.IfscCode,
                    //    AccountNumber = jointAccount.AccountNumber,
                    //    AccountType = Enums.AccountTypes.JointAccount,
                    //    AccountHolder1Name = jointAccount.AccountHolder1Name,
                    //    AccountHolder2Name = jointAccount.AccountHolder2Name,
                    //    Holder1Email = jointAccount.Holder1Email,
                    //    Holder2Email = jointAccount.Holder2Email,
                    //    Holder1Address = jointAccount.Holder1Address,
                    //    Holder2Address = jointAccount.Holder2Address,
                    //};

                    //Approach 2 - AutoMapper 
                    var config = new MapperConfiguration(cfg =>
                    {
                        cfg.CreateMap<JointAccountDto, BankAccount>();
                    });

                    IMapper iMapper = config.CreateMapper();
                    BankAccount bankAccount = iMapper.Map<JointAccountDto, BankAccount>(jointAccountDto);
                    bankAccount.AccountType = AccountTypes.JointAccount;

                    int jointAccountId = _bankAccountServiceRepository.Add(bankAccount);
                    return Ok(jointAccountId);
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