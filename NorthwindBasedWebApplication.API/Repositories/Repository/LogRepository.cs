using NorthwindBasedWebApplication.API.Data;
using NorthwindBasedWebApplication.API.Models;
using NorthwindBasedWebApplication.API.Repositories.Base;
using NorthwindBasedWebApplication.API.Repositories.IRepository;

namespace NorthwindBasedWebApplication.API.Repositories.Repository
{
    public class LogRepository : EntityBaseRepository<LogDto>, ILogRepository
    {
        private readonly ApplicationDbContext _context; 


        public LogRepository(ApplicationDbContext context) : base(context)
        {

        }
    }
}
