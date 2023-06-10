using MyWebApp.Models;

namespace MyWebApp.Repositories
{
    public interface IProductRepository
    {
        public List<Product> GetAll();
    }
}
