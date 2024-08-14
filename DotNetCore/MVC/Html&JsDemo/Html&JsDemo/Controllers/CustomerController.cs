using AutoMapper;
using Html_JsDemo.DatModels;
using Html_JsDemo.Repositories;
using Html_JsDemo.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Html_JsDemo.Controllers
{
    public class CustomerController : Controller
    {
        ICustomerRepository _customerRepository;
        IMapper _imapper;

        public CustomerController(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;

            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CustomerVm, Customer>();
            });

            _imapper = configuration.CreateMapper();
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Register(CustomerVm customerVm) 
        {
            Customer customer = _imapper.Map<CustomerVm, Customer>(customerVm);
            int affectedRowCount = _customerRepository.Register(customer);
            if (affectedRowCount > 0) 
            {
                ViewBag.SuccessMessage = "Customer Register Successfully...";
            }
            return Json(customer);
        }
    }
}
