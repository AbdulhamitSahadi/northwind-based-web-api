using Microsoft.EntityFrameworkCore;
using NorthwindBasedWebApplication.API.Data;
using NorthwindBasedWebApplication.API.Models;
using NorthwindBasedWebApplication.API.Repositories.Base;
using NorthwindBasedWebApplication.API.Repositories.IRepository;

namespace NorthwindBasedWebApplication.API.Repositories.Repository
{
    public class EmployeeRepository : EntityBaseRepository<Employee>, IEmployeeRepository
    {

        private readonly ApplicationDbContext _context;

        public EmployeeRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<ICollection<Order>> GetOrdersByEmployeeAsync(int id)
        {
            var orders = await _context.Orders
                .Where(i => i.EmployeeId == id)
                .OrderBy(i => i.Id)
                .ToListAsync();

            return orders;
        }

        public async Task<ICollection<Territory>> GetTerritoriesByEmployeeAsync(int id)
        {
            var territories = await _context.EmployeesTerritories
                .Where(i => i.EmployeeId == id)
                .Select(t => t.Territory)
                .ToListAsync();

            return territories;
        }
    }
}
