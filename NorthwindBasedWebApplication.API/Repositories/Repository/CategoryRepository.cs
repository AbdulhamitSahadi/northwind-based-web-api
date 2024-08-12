using Microsoft.EntityFrameworkCore;
using NorthwindBasedWebApplication.API.Data;
using NorthwindBasedWebApplication.API.Models;
using NorthwindBasedWebApplication.API.Repositories.Base;
using NorthwindBasedWebApplication.API.Repositories.IRepository;

namespace NorthwindBasedWebApplication.API.Repositories.Repository
{
    public class CategoryRepository : EntityBaseRepository<Category>, ICategoryRepository
    {
        private readonly ApplicationDbContext _context;

        public CategoryRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<ICollection<Product>> GetProductsByCategory(int id)
        {
            var products = await _context.Products
                .Where(i => i.CategoryId == id)
                .OrderBy(o => o.Id)
                .ToListAsync();

            return products;
        }
    }
}
