using NorthwindBasedWebApplication.API.Models;
using NorthwindBasedWebApplication.API.Repositories.Base;

namespace NorthwindBasedWebApplication.API.Repositories.IRepository
{
    public interface IProductRepository : IEntityBaseRepository<Product>
    {
        Task<Category> GetCategoryByProductAsync(int id);
        Task<Supplier> GetSupplierByProductAsync(int id);
        Task<ICollection<Order>> GetOrdersByProductAsync(int id);
    }
}
