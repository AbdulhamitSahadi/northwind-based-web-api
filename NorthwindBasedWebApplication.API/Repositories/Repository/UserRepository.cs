using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NorthwindBasedWebApplication.API.Data;
using NorthwindBasedWebApplication.API.Models;
using NorthwindBasedWebApplication.API.Repositories.IRepository;

namespace NorthwindBasedWebApplication.API.Repositories.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserRepository(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<bool> ChangePasswordAsync(ApplicationUser user, string currentPassword, string newPassword)
        {
            var passwordChangedResult = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);

            return passwordChangedResult.Succeeded;
        }

        public async Task<bool> CreateAsync(ApplicationUser user)
        {
            var createUserResult = await _userManager.CreateAsync(user);

            return createUserResult.Succeeded;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var user = await _context.Users.Where(i => i.Id == id).FirstOrDefaultAsync();
            var deleteUserResult = await _userManager.DeleteAsync(user);

            return deleteUserResult.Succeeded;
        }

        public async Task<IReadOnlyList<ApplicationUser>> GetAllAsync()
        {
            var users = _userManager.Users.ToList();

            return users;
        }

        public Task<ApplicationUser> GetByIdAsync(int id)
        {
            var user = _context.ApplicationUsers.Where(i => i.Id == id).FirstOrDefaultAsync();

            return user;
        }

       

        public async Task<bool> IsExistsAsync(int id)
        {
            return await _context.ApplicationUsers.AnyAsync(i => i.Id == id);
        }

        public async Task<bool> IsExistsAsync(string email)
        {
            return await _context.ApplicationUsers.AnyAsync(e => e.Email == email);
        }

        public async Task<bool> UpdateAsync(ApplicationUser user)
        {
            var updateUserResult = await _userManager.UpdateAsync(user);


            return updateUserResult.Succeeded;
        }
    }
}
