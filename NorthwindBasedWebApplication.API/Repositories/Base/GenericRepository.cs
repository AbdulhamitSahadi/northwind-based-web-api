using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using NorthwindBasedWebApplication.API.Data;

namespace NorthwindBasedWebApplication.API.Repositories.Base
{
    public class GenericRepository<T> : IGenericRepository<T> where T : IdentityRole<int>
    {
        private readonly ApplicationDbContext _context;
        public GenericRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Commit()
        {
            _context.Database.CommitTransaction();
        }

        public IQueryable<T> GetTableAsNoTracked() => _context.Set<T>().AsNoTracking().AsQueryable();

        public IQueryable<T> GetTableAsTracked() => _context.Set<T>().AsQueryable();

        public void RollBack()
        {
            _context.Database.RollbackTransaction();
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public IDbContextTransaction Transaction()
        {
            return _context.Database.BeginTransaction();
        }
    }
}
