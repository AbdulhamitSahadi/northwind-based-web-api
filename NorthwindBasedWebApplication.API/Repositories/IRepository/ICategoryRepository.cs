﻿using NorthwindBasedWebApplication.API.Models;
using NorthwindBasedWebApplication.API.Repositories.Base;

namespace NorthwindBasedWebApplication.API.Repositories.IRepository
{
    public interface ICategoryRepository : IEntityBaseRepository<Category>
    {
        Task<ICollection<Product>> GetProductsByCategory(int id);
    }
}
