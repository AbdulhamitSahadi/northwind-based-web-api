using NorthwindBasedWebApplication.API.Models.Base;
using System.Linq.Expressions;

namespace NorthwindBasedWebApplication.API.Repositories.Base
{
    public interface IEntityBaseRepository<T> where T : BaseEntity
    {
        Task<List<T>> GetAllAsync(Expression<Func<T, bool>> predicate = null,
            bool tracked = true, string? includeProperties = null);

        Task<T> GetAsync(Expression<Func<T, bool>> predicate = null,
            bool tracked = true, string? includeProperties = null);

        Task<bool> CreateAsync(T entity);
        Task<bool> UpdateAsync(T entity);
        Task<bool> IsExistAsync(Expression<Func<T, bool>> predicate = null,
            bool tracked = true, string? includeProperties = null);


        Task<bool> DeleteAsync(T entity);
    }
}
