using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApiDemo1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CalculatorController : ControllerBase
    {
        [HttpGet]
        [Route("Calculate/{Num1}/{Num2}")]
        public IActionResult Calculate(int num1, int num2) 
        {
            int addResult = Add(num1, num2);
            int subtractResult = Subtract(num1, num2);
            int multiplyResult = Multiply(num1, num2);
            int divisionResult = Division(num1, num2);

            string text = ($@"First Parameter Value Is {num1} And Second Parameter Value Is {num2} And 
                           Addition Result {addResult} Subtraction Result Is {subtractResult} Mutiplication Result Is 
                           {multiplyResult} And Division Result Is {divisionResult}, Calculation Completed At {DateTime.Now} ");

            return new JsonResult(text);
        }

        private int Add(int num1, int num2)
        {
            int total;
            total = num1 + num2;

            return total;
        }

        private int Subtract(int num1, int num2)
        {
            int total;
            total = num1 - num2;

            return total;
        }

        private int Multiply(int num1, int num2)
        {
            int total;
            total = num1 * num2;

            return total;
        }

        private int Division(int num1, int num2)
        {
            int total;
            total = num1 / num2;

            return total;
        }
    }
}
