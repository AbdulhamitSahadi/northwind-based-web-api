using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Storage;

namespace NorthwindBasedWebApplication.API.Repositories.Base
{
    public interface IGenericRepository<T> where T : IdentityRole<int>
    {
        IDbContextTransaction Transaction();

        void Commit();
        void RollBack();

        IQueryable<T> GetTableAsTracked();
        IQueryable<T> GetTableAsNoTracked();

        Task<bool> SaveChangesAsync();
    }
}
