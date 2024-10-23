using Fantasy.Shared.Entities;
using Microsoft.EntityFrameworkCore;

namespace Fantasy.Backend.Data;

public class SeedDb
{
    private readonly DataContext _context;

    public SeedDb(DataContext context)
    {
        _context = context;
    }

    public async Task SeedAsync()
    {
        await _context.Database.EnsureCreatedAsync();
        await CheckCountriesAsync();
        await CheckTeamsAsync();
    }

    private async Task CheckTeamsAsync()
    {
        if (!_context.Countries.Any())
        {
            var countriesSqlScript = File.ReadAllText("Data\\Countries.sql");
            await _context.Database.ExecuteSqlRawAsync(countriesSqlScript);
        }
    }

    private async Task CheckCountriesAsync()
    {
        if (!_context.Teams.Any())
        {
            foreach (var item in _context.Countries)
            {
                _context.Teams.Add(new Team { Name = item.Name, Country = item });
                if (item.Name == "Colombia")
                {
                    _context.Teams.Add(new Team { Name = "Medellin", Country = item });
                    _context.Teams.Add(new Team { Name = "Nacional", Country = item });
                    _context.Teams.Add(new Team { Name = "Millonarios", Country = item });
                    _context.Teams.Add(new Team { Name = "Junior", Country = item });
                }
            }
            await _context.SaveChangesAsync();
        }
    }
}