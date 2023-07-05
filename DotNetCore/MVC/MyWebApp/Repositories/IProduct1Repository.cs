using MyWebApp.Models;

namespace MyWebApp.Repositories
{
    public interface IProduct1Repository
    {
        public List<Product1> GetProduct(string productName);
    }
}
