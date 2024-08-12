using Microsoft.EntityFrameworkCore;
using NorthwindBasedWebApplication.API.Data;
using NorthwindBasedWebApplication.API.Models;
using NorthwindBasedWebApplication.API.Repositories.Base;
using NorthwindBasedWebApplication.API.Repositories.IRepository;

namespace NorthwindBasedWebApplication.API.Repositories.Repository
{
    public class CustomerDemographicRepository : EntityBaseRepository<CustomerDemographic>, ICustomerDemographicRepository
    {
        private readonly ApplicationDbContext _context;

        public CustomerDemographicRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<ICollection<Customer>> GetCustomersByCustomerDemographicAsync(int id)
        {
            var customers = await _context.CustomersCustomerDemographics
                .Where(i => i.CustomerTypeId == id)
                .Select(c => c.Customer)
                .ToListAsync();

            return customers;
        }
    }
}
