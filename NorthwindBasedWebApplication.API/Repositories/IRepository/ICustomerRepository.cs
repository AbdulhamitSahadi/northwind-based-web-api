﻿using NorthwindBasedWebApplication.API.Models;
using NorthwindBasedWebApplication.API.Repositories.Base;

namespace NorthwindBasedWebApplication.API.Repositories.IRepository
{
    public interface ICustomerRepository : IEntityBaseRepository<Customer>
    {
    }
}