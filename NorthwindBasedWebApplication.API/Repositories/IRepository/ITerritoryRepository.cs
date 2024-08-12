using NorthwindBasedWebApplication.API.Models;
using NorthwindBasedWebApplication.API.Repositories.Base;

namespace NorthwindBasedWebApplication.API.Repositories.IRepository
{
    public interface ITerritoryRepository : IEntityBaseRepository<Territory>
    {
        Task<ICollection<Employee>> GetEmployeesByTerritoryAsync(int id);
        Task<Region> GetRegionByTerritoryAsync(int id);
    }
}
