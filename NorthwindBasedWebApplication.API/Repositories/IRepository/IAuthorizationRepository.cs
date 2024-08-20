using NorthwindBasedWebApplication.API.Models;
using NorthwindBasedWebApplication.API.Models.Common;
using NorthwindBasedWebApplication.API.Models.Common.Authorization;
using NorthwindBasedWebApplication.API.Models.Requests;


namespace NorthwindBasedWebApplication.API.Repositories.IRepository
{
    public interface IAuthorizationRepository
    {
        Task<bool> AddRoleAsync(string roleName);
        Task<bool> UpdateRoleAsync(Models.Common.Authorization.UpdateRoleRequest updateRoleRequest);
        Task<bool> DeleteRoleAsync(int id);
        Task<IReadOnlyList<RoleDto>> GetAllAsync();
        Task<ApplicationRole> GetByIdAsync(int id);
        Task<bool> IsExistsAsync(string role);
        Task<bool> IsExistsAsync(int id);

        Task<UserRolesResponse> GetRolesByUser(int id);
        Task<UserClaimsResponse> GetClaimsByUser(int id);
        
    }
}
