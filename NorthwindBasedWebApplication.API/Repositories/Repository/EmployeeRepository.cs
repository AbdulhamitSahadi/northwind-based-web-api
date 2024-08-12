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
    }
}
