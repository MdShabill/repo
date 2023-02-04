using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Collections.Specialized;
using System.Data;
using WebApiDemo1.DTO.InputDTO;
using WebApiDemo1.Enums;
using WebApiDemo1.Repositories;
using WebApplication1.DTO.InputDTO;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        IOrderRepository _orderRepository;

        public OrdersController(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        [HttpGet]
        [Route("GetAllOrders")]
        public IActionResult GetAllOrders()
        {
            DataTable dataTable = _orderRepository.GetAllOrders();

            if (dataTable.Rows.Count > 0)
                return Ok(JsonConvert.SerializeObject(dataTable));
            else
                return NotFound();
        }

        [HttpGet]
        [Route("GetOrdersCount")]
        public IActionResult GetOrdersCount()
        {
            int orderCount = _orderRepository.GetOrdersCount();
            return Ok(orderCount);
        }

        [HttpGet]
        [Route("GetOrderDetailById/{orderId}")]
        public IActionResult GetOrderDetailById(int orderId)
        {
            if (orderId < 1)
                return BadRequest("OrderId should be greater than 0");

            DataTable dataTable = _orderRepository.GetOrderDetailById(orderId);

            if (dataTable.Rows.Count > 0)
                return Ok(JsonConvert.SerializeObject(dataTable));
            else
                return NotFound();
        }

        [HttpGet]
        [Route("GetOrdersDetailByOrderDate/{orderDate}")]
        public IActionResult GetOrdersDetailByOrderDate(string orderDate)
        {
            var orderDateTime = DateTime.Parse(orderDate);
            if (orderDateTime > DateTime.Now)
                return BadRequest("Order Date cannot be greater than current date");

            DataTable dataTable = _orderRepository.GetOrdersDetailByOrderDate(orderDate);

            if (dataTable.Rows.Count > 0)
                return Ok(JsonConvert.SerializeObject(dataTable));
            else
                return NotFound();
        }

        [HttpGet]
        [Route("GetOrdersByAmountRange/{minimumAmount}/{maximumAmount}")]
        public IActionResult GetOrdersByAmountRange(int minimumAmount, int maximumAmount)
        {
            if (minimumAmount > maximumAmount)
                return BadRequest("minimum amount cannot be more than maximum amount");

            DataTable dataTable = _orderRepository.GetOrdersByAmountRange(minimumAmount, maximumAmount);

            if (dataTable.Rows.Count > 0)
                return Ok(JsonConvert.SerializeObject(dataTable));
            else
                return NotFound();
        }

        [HttpPost]
        [Route("OrderAdd")]
        public IActionResult OrderAdd([FromBody] OrderDto order)
        {
            try
            {
                string errorMessage = validateOrderAddOrUpdate(order);
                if (!string.IsNullOrEmpty(errorMessage))
                    return BadRequest(errorMessage);
 
                    if (ModelState.IsValid)
                    {
                        int id = _orderRepository.OrderAdd(order);
                        return Ok(order.Id);
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
        [Route("OrderUpdate")]
        public IActionResult OrderUpdate([FromBody] OrderDto order)
        {
            try
            {
                string errorMessage = validateOrderAddOrUpdate(order, true);
                if (!string.IsNullOrEmpty(errorMessage))
                    return BadRequest(errorMessage);

                if (ModelState.IsValid)
                {
                    _orderRepository.OrderUpdate(order);
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

        private string validateOrderAddOrUpdate(OrderDto order, bool isUpdate = false)
        {
            string errorMessage = "";

            var orderDateTime = DateTime.Parse(order.OrderDate);

            if (isUpdate == true)
            {
                if (order.Id < 1)
                    errorMessage = "Id can not be less than 0";
            }
 
            if (order.CustomerId < 1)
                errorMessage = "Customer id Should be greater than 0";

            if (orderDateTime > DateTime.Now)
                errorMessage = "Order Date cannot be greater than current date";

            if (order.TotalAmount < 5000)
                errorMessage = "Invalid amount, order amount should be above 5000";

            else if (!Enum.IsDefined(typeof(ProductType), order.ProductName))
                errorMessage = "Invalid Product Name";

            return errorMessage;
        }
    }
}
