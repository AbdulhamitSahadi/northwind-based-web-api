using NorthwindBasedWebApplication.API.Models;
using NorthwindBasedWebApplication.API.Repositories.Base;

namespace NorthwindBasedWebApplication.API.Repositories.IRepository
{
    public interface IRegionRepository : IEntityBaseRepository<Region>
    {
        Task<ICollection<Territory>> GetTerritoriesByRegionAsync(int id);
    }
}
