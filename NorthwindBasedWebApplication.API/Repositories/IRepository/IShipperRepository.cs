using NorthwindBasedWebApplication.API.Models;
using NorthwindBasedWebApplication.API.Repositories.Base;

namespace NorthwindBasedWebApplication.API.Repositories.IRepository
{
    public interface IShipperRepository : IEntityBaseRepository<Shipper>
    {
        Task<ICollection<Order>> GetOrdersByShipper(int id);
    }
}
