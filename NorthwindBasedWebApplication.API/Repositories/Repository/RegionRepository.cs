using Microsoft.EntityFrameworkCore;
using NorthwindBasedWebApplication.API.Data;
using NorthwindBasedWebApplication.API.Models;
using NorthwindBasedWebApplication.API.Repositories.Base;
using NorthwindBasedWebApplication.API.Repositories.IRepository;

namespace NorthwindBasedWebApplication.API.Repositories.Repository
{
    public class RegionRepository : EntityBaseRepository<Region>, IRegionRepository
    {

        private readonly ApplicationDbContext _context;

        public RegionRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<ICollection<Territory>> GetTerritoriesByRegionAsync(int id)
        {
            var territories = await _context.Territories
                .Where(i => i.RegionId == id)
                .ToListAsync();

            return territories;
        }
    }
}
