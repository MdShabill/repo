using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ShopEase.DataModels.Restaurant;
using ShopEase.Repositories;
using ShopEase.ViewModels;
using ShopEase.ViewModels.Restaurant;

namespace ShopEase.Controllers
{
    public class RestaurantController : Controller
    {
        IRestaurantRepository _restaurantRepository;
        IMapper _imapper;

        public RestaurantController(IRestaurantRepository restaurantRepository)
        {
            _restaurantRepository = restaurantRepository;

            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Restaurant, RestaurantVm>();
                cfg.CreateMap<RestaurantVm, Restaurant>();
            });

            _imapper = configuration.CreateMapper();
        }

        public IActionResult ViewAll()
        {
            List<Restaurant> restaurants = _restaurantRepository.GetAll();

            List<RestaurantVm> restaurantVm = _imapper.Map<List<Restaurant>, List<RestaurantVm>>(restaurants);

            return View(restaurantVm);
        }

        public IActionResult View(int id)
        {
            Restaurant restaurant = _restaurantRepository.GetRestaurantById(id);
            RestaurantVm restaurantVm = _imapper.Map<Restaurant, RestaurantVm>(restaurant);

            return View(restaurantVm);
        }

        public IActionResult Add()
        {

            return View();
        }

        [HttpPost]
        public IActionResult Add(RestaurantVm restaurantVm) 
        {
            if (string.IsNullOrWhiteSpace(restaurantVm.RestaurantName))
            {
                ViewBag.ErrorMessage = "Restaurant Name Should Not Be Blank";
                return View();
            }

            if (string.IsNullOrWhiteSpace(restaurantVm.MobileNumber))
            {
                ViewBag.ErrorMessage = "Invalid Mobile Number";
                return View();
            }

            
            if (string.IsNullOrWhiteSpace(restaurantVm.Location))
            {
                ViewBag.ErrorMessage = "Please Give your correct Location";
                return View();
            }

            Restaurant restaurant = _imapper.Map<RestaurantVm, Restaurant>(restaurantVm);
            int affectedRowCount = _restaurantRepository.Register(restaurant);
            if (affectedRowCount > 0)
            {
                ViewBag.successMessage = "Add New Restaurant Successful";
            }
            return RedirectToAction("ViewAll", restaurantVm);
        }

        public IActionResult Edit(int id) 
        {
            Restaurant restaurant = _restaurantRepository.GetRestaurantById(id);
            RestaurantVm restaurantVm = _imapper.Map<Restaurant, RestaurantVm>(restaurant);

            return View(restaurantVm);
        }

        [HttpPost]
        public IActionResult Update(RestaurantVm restaurantVm) 
        {
            Restaurant restaurant = _imapper.Map<RestaurantVm, Restaurant>(restaurantVm);
            int affectedRowCount = _restaurantRepository.Update(restaurant);
            if (affectedRowCount > 0) 
            {
                ViewBag.SuccessMessage = "Update Restaurant Details Successful";
            }
            return RedirectToAction("ViewAll");
        }

        public IActionResult Delete(int id) 
        {
            int deletedRow = _restaurantRepository.Delete(id);
            if (deletedRow > 0)
            {
                ViewBag.SuccessMessage = "Restaurant Record Delete Successful";
            }
            return RedirectToAction("ViewAll");
        }
    }
}
