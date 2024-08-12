using NorthwindBasedWebApplication.API.Models;
using NorthwindBasedWebApplication.API.Repositories.Base;

namespace NorthwindBasedWebApplication.API.Repositories.IRepository
{
    public interface ICustomerRepository : IEntityBaseRepository<Customer>
    {
        Task<ICollection<Order>> GetOrdersByCustomerAsync(int id);
        Task<ICollection<CustomerDemographic>> GetCustomerDemographicsByCustomerAsync(int id);
    }
}
