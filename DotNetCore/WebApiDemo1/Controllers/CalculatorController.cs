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

            CalculateService calculateService = new();
            int addResult = calculateService.Add(num1, num2);
            int subtractResult = calculateService.Subtract(num1, num2);
            int multiplyResult = calculateService.Multiply(num1, num2);
            int divisionResult = calculateService.Division(num1, num2);

            string text = ($@"First Parameter Value Is {num1} And Second Parameter Value Is {num2} And 
                           Addition Result {addResult} Subtraction Result Is {subtractResult} Mutiplication Result Is 
                           {multiplyResult} And Division Result Is {divisionResult}, Calculation Completed At {DateTime.Now} ");

            return new JsonResult(text);
        }

        public class CalculateService
        {
            public int Add(int num1, int num2)
            {
                int total;
                total = num1 + num2;

                return total;
            }

            public int Subtract(int num1, int num2)
            {
                int total;
                total = num1 - num2;

                return total;
            }

            public int Multiply(int num1, int num2)
            {
                int total;
                total = num1 * num2;

                return total;
            }

            public int Division(int num1, int num2)
            {
                int total;
                total = num1 / num2;

                return total;
            }
        }

        [HttpGet]
        [Route("Calculate/{CalculateType}/{Num1}/{Num2}")]
        public IActionResult Calculate(string calculateType, int num1, int num2)
        {
            CalculateService calculateService = new();

            string message;
            int result;
            switch (calculateType)
            {
                case "A":
                    result = calculateService.Add(num1, num2);
                    message = "Addition";
                    break;
                case "S":
                    result = calculateService.Subtract(num1, num2);
                    message = "Subtraction";
                    break;
                case "M":
                    result = calculateService.Multiply(num1, num2);
                    message = "Mutiplication";
                    break;
                case "D":
                    result = calculateService.Division(num1, num2);
                    message = "Division";
                    break;
                default:
                    return BadRequest("Invalid Calculate Type");
            }

            string text = $@"First Parameter Is {num1} And second Parameter {num2} And Calculate Type Is {message} And Result is {result} ";

            return new JsonResult(text);
        }

        [HttpGet]
        [Route("Calculate1/{CalculateType}/{Num1}/{Num2}")]
        public IActionResult Calculate1(string calculateType, int num1, int num2)
        {
            CalculateService calculateService = new();

            string message;
            int result;

            if (calculateType == "A")
            {
                message = "Addition";
                result = calculateService.Add(num1, num2);
            }
            else if (calculateType == "S")
            {
                message = "Subtraction";
                result = calculateService.Subtract(num1, num2);
            }
            else if (calculateType == "M")
            {
                message = "Mutiplication";
                result = calculateService.Multiply(num1, num2);
            }
            else if (calculateType == "D")
            {
                message = "Division";
                result = calculateService.Division(num1, num2);
            }
            else
            {
                return BadRequest("Invalid Calculate Type");
            }
                    
            string text = $@"First Parameter Is {num1} And second Parameter {num2} And Calculate Type Is {message} And Result is {result} ";

            return new JsonResult(text);
        }
    }
}
