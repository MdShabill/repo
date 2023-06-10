using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApiDemo1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehicleController : ControllerBase
    {
        [HttpGet]
        [Route("VehicleDetails/{Name}/{Color}/{TopSpeed}/{BasicPrice}/{GstAmount}")]
        public IActionResult VehicleDetails(string name, string color, int topSpeed, int basicPrice, int gstAmount)
        {
            LuxuryCar luxuryCar = new();
            string vehicleName = luxuryCar.GetName(name);
            string vehicleColor = luxuryCar.GetColor(color);
            int vehicleSpeed = luxuryCar.GetSpeed(topSpeed);
            int vehicleActualPrice = luxuryCar.GetPriceOnRoad(basicPrice, gstAmount);
            luxuryCar.SaveVehicle(luxuryCar);
            DateTime vehicleLaunchDate = luxuryCar.GetLaunchDate();
            
            string myVehicle = $@"My Vehicle Name Is {vehicleName} And Vehicle Color Is {vehicleColor} And
                                This Car Top Speed Is {vehicleSpeed} And Vehicle Actual Price Is {vehicleActualPrice}
                                And Vehicle Launch Date Is {vehicleLaunchDate}";

            return new JsonResult(myVehicle);
        }

        public class LuxuryCar
        {
            public string GetName(string name)
            {
                string carName;
                carName = name;

                return carName;
            }

            public string GetColor(string color)
            {
                string carColor;
                carColor = color;

                return carColor;
            }

            public int GetSpeed(int topSpeed)
            {
                int carSpeed;
                carSpeed = topSpeed;

                return carSpeed;
            }

            public int GetPriceOnRoad(int basicPrice, int gstAmount)
            {
                int actualPrice;
                actualPrice = basicPrice + gstAmount;

                return actualPrice;
            }

            public void SaveVehicle(LuxuryCar car)
            {
                //We Will Save The Luxury Car Into Data Base
            }

            public DateTime GetLaunchDate()
            { 
                return DateTime.Now;
            }
        }
    }
}
