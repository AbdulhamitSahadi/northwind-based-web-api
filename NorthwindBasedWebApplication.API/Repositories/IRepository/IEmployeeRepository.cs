using NorthwindBasedWebApplication.API.Models;
using NorthwindBasedWebApplication.API.Repositories.Base;

namespace NorthwindBasedWebApplication.API.Repositories.IRepository
{
    public interface IEmployeeRepository : IEntityBaseRepository<Employee>
    {
        Task<ICollection<Territory>> GetTerritoriesByEmployeeAsync(int id);
        Task<ICollection<Order>> GetOrdersByEmployeeAsync(int id);
    }
}
