using NorthwindBasedWebApplication.API.Models;
using NorthwindBasedWebApplication.API.Repositories.Base;

namespace NorthwindBasedWebApplication.API.Repositories.IRepository
{
    public interface ICustomerDemographicRepository : IEntityBaseRepository<CustomerDemographic>
    {
        Task<ICollection<Customer>> GetCustomersByCustomerDemographicAsync(int id);
    }
}
