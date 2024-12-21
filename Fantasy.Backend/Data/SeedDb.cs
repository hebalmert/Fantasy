using Fantasy.Backend.Helpers;
using Fantasy.Backend.UnitsOfWork.Interfaces;
using Fantasy.Shared.Entities;
using Fantasy.Shared.Enums;
using Microsoft.EntityFrameworkCore;

namespace Fantasy.Backend.Data;

public class SeedDb
{
    private readonly DataContext _context;
    private readonly IFileStorage _fileStorage;
    private readonly IUserUnitOfWork _userUnitOfWork;

    public SeedDb(DataContext context, IFileStorage fileStorage, IUserUnitOfWork userUnitOfWork)
    {
        _context = context;
        _fileStorage = fileStorage;
        _userUnitOfWork = userUnitOfWork;
    }

    public async Task SeedAsync()
    {
        await _context.Database.EnsureCreatedAsync();
        await CheckCountriesAsync();
        await CheckTeamsAsync();
        await CheckRolesAsync();
        await CheckUserAsync("Hebert", "Merchan", "nexxtcargo@gmail.com", "768 555 5555", UserType.Admin);
    }

    private async Task CheckRolesAsync()
    {
        await _userUnitOfWork.CheckRoleAsync(UserType.Admin.ToString());
        await _userUnitOfWork.CheckRoleAsync(UserType.User.ToString());
    }

    private async Task<User> CheckUserAsync(string firstName, string lastName, string email, string phone, UserType userType)
    {
        var user = await _userUnitOfWork.GetUserAsync(email);
        if (user == null)
        {
            var country = await _context.Countries.FirstOrDefaultAsync(x => x.Name == "Colombia");
            user = new User
            {
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                UserName = email,
                PhoneNumber = phone,
                Country = country!,
                UserType = userType
            };

            await _userUnitOfWork.AddUserAsync(user, "123456");
            await _userUnitOfWork.AddUserToRoleAsync(user, userType.ToString());

            //Crear el token y confirmar la cuenta para activar
            var token = await _userUnitOfWork.GenerateEmailConfirmationTokenAsync(user);
            await _userUnitOfWork.ConfirmEmailAsync(user, token);
        }

        return user;
    }

    private async Task CheckCountriesAsync()
    {
        if (!_context.Countries.Any())
        {
            var countriesSQLScript = File.ReadAllText("Data\\Countries.sql");
            await _context.Database.ExecuteSqlRawAsync(countriesSQLScript);
        }
    }

    private async Task CheckTeamsAsync()
    {
        if (!_context.Teams.Any())
        {
            foreach (var country in _context.Countries)
            {
                var imagePath = string.Empty;
                var filePath = $"wwwroot\\Images\\Flags\\{country.Name}.png";
                if (File.Exists(filePath))
                {
                    var fileBytes = File.ReadAllBytes(filePath);
                    imagePath = await _fileStorage.SaveFileAsync(fileBytes, ".jpg", "teams");
                }
                _context.Teams.Add(new Team { Name = country.Name, Country = country!, Image = imagePath });
            }

            await _context.SaveChangesAsync();
        }
    }
}