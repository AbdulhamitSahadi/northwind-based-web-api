using Microsoft.EntityFrameworkCore;
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

        public async Task<ICollection<Order>> GetOrdersByShipper(int id)
        {
            var orders = await _context.Orders
                .Where(i => i.ShipVia == id)
                .ToListAsync();

            return orders;
        }
    }
}
