using NorthwindBasedWebApplication.API.Models;

namespace NorthwindBasedWebApplication.API.Repositories.IRepository
{
    public interface IUserRepository
    {
        Task<bool> CreateAsync(ApplicationUser user);
        Task<bool> DeleteAsync(int id);
        Task<IReadOnlyList<ApplicationUser>> GetAllAsync();
        Task<ApplicationUser> GetByIdAsync(int id);
        Task<bool> IsExistsAsync(int id);
        Task<bool> IsExistsAsync(string email);
        Task<bool> ChangePasswordAsync(ApplicationUser user, string currentPassword, string newPassword);
        Task<bool> UpdateAsync(ApplicationUser user);
    }
}
