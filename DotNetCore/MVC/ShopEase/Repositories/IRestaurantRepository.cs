using ShopEase.DataModels.Restaurant;

namespace ShopEase.Repositories
{
    public interface IRestaurantRepository
    {
        public List<Restaurant> GetAllSortedresult(string? sortColumnName, string? sortOrder);
        public int Register(Restaurant restaurant);
        public Restaurant GetRestaurantById(int id);
        public int Update(Restaurant restaurant);
        public int Delete(int id);
    }
}
