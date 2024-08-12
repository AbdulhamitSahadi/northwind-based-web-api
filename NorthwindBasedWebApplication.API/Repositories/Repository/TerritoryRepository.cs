using NorthwindBasedWebApplication.API.Data;
using NorthwindBasedWebApplication.API.Models;
using NorthwindBasedWebApplication.API.Repositories.Base;
using NorthwindBasedWebApplication.API.Repositories.IRepository;

namespace NorthwindBasedWebApplication.API.Repositories.Repository
{
    public class TerritoryRepository : EntityBaseRepository<Territory>, ITerritoryRepository
    {

        private readonly ApplicationDbContext _context;

        public TerritoryRepository(ApplicationDbContext context) : base (context)
        {
            _context = context;
        }
    }
}
