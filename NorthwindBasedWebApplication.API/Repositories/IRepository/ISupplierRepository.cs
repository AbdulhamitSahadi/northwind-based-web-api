using NorthwindBasedWebApplication.API.Models;
using NorthwindBasedWebApplication.API.Repositories.Base;

namespace NorthwindBasedWebApplication.API.Repositories.IRepository
{
    public interface ISupplierRepository : IEntityBaseRepository<Supplier>
    {
        Task<ICollection<Product>> GetProductsBySupplier(int id);
    }
}
