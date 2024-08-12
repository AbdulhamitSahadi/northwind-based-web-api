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
    }
}
