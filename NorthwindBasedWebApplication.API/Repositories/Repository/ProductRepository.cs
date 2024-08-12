using Microsoft.EntityFrameworkCore;
using NorthwindBasedWebApplication.API.Data;
using NorthwindBasedWebApplication.API.Models;
using NorthwindBasedWebApplication.API.Repositories.Base;
using NorthwindBasedWebApplication.API.Repositories.IRepository;

namespace NorthwindBasedWebApplication.API.Repositories.Repository
{
    public class ProductRepository : EntityBaseRepository<Product>, IProductRepository
    {

        private readonly ApplicationDbContext _context;

        public ProductRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Category> GetCategoryByProductAsync(int id)
        {
            var category = await _context.Products
                .Where(i => i.Id == id)
                .Select(c => c.Category)
                .FirstOrDefaultAsync();

            return category;
        }

        public async Task<ICollection<Order>> GetOrdersByProductAsync(int id)
        {
            var orders = await _context.OrderDetails
                .Where(i => i.ProductId == id)
                .Select(o => o.Order)
                .ToListAsync();

            return orders;
        }

        public async Task<Supplier> GetSupplierByProductAsync(int id)
        {
            var supplier = await _context.Products
                .Where(i => i.Id == id)
                .Select(s => s.Supplier)
                .FirstOrDefaultAsync();

            return supplier;
        }
    }
}
