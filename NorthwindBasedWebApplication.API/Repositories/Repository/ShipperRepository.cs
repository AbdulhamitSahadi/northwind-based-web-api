using NorthwindBasedWebApplication.API.Data;
using NorthwindBasedWebApplication.API.Models;
using NorthwindBasedWebApplication.API.Repositories.Base;
using NorthwindBasedWebApplication.API.Repositories.IRepository;

namespace NorthwindBasedWebApplication.API.Repositories.Repository
{
    public class ShipperRepository : EntityBaseRepository<Shipper>, IShipperRepository
    {

        private readonly ApplicationDbContext _context;


        public ShipperRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
