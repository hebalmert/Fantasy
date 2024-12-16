using Fantasy.Backend.Repositories.Interfaces;
using Fantasy.Backend.UnitsOfWork.Interfaces;
using Fantasy.Shared.Entities;
using Microsoft.AspNetCore.Identity;

namespace Fantasy.Backend.UnitsOfWork.Implementations;

public class UserUnitOfWork : IUserUnitOfWork
{
    private readonly IUserRepository _userRepository;

    public UserUnitOfWork(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<IdentityResult> AddUserAsync(User user, string password) =>
        await _userRepository.AddUserAsync(user, password);

    public async Task AddUserToRoleAsync(User user, string roleName) =>
        await _userRepository.AddUserToRoleAsync(user, roleName);

    public async Task CheckRoleAsync(string roleName) =>
        await _userRepository.CheckRoleAsync(roleName);

    public async Task<User> GetUserAsync(string email) =>
        await _userRepository.GetUserAsync(email);

    public async Task<bool> IsUserInRoleAsync(User user, string roleName) =>
        await _userRepository.IsUserInRoleAsync(user, roleName);
}