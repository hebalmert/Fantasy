using Fantasy.Backend.Data;
using Fantasy.Backend.Repositories.Interfaces;
using Fantasy.Shared.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Fantasy.Backend.Repositories.Implementations
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _dataContext;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserRepository(DataContext dataContext, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            _dataContext = dataContext;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IdentityResult> AddUserAsync(User user, string password)
        {
            return await _userManager.CreateAsync(user, password);
        }

        public async Task AddUserToRoleAsync(User user, string roleName)
        {
            await _userManager.AddToRoleAsync(user, roleName);
        }

        public async Task CheckRoleAsync(string roleName)
        {
            var RoleExist = await _roleManager.RoleExistsAsync(roleName);
            if (!RoleExist)
            {
                await _roleManager.CreateAsync(new IdentityRole
                {
                    Name = roleName
                });
            }
        }

        public async Task<User> GetUserAsync(string email)
        {
            var user = await _dataContext.Users
                .Include(x => x.Country)
                .FirstOrDefaultAsync(x => x.Email == email);
            return user!;
        }

        public async Task<bool> IsUserInRoleAsync(User user, string roleName)
        {
            return await _userManager.IsInRoleAsync(user, roleName);
        }
    }
}