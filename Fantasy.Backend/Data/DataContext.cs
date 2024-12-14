using Fantasy.Shared.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Fantasy.Backend.Data;

public class DataContext : IdentityDbContext<User>
{
    public DataContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Country> Countries => Set<Country>();

    public DbSet<Team> Teams => Set<Team>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        //Para tomar los calores de ConfigEntities

        modelBuilder.Entity<Country>().HasIndex(e => e.Name).IsUnique();

        modelBuilder.Entity<Team>().HasIndex(e => new { e.CountryId, e.Name }).IsUnique();

        //Para evitar el borrado en cascada de cualquier entidad creada
        DisableCascadingDelete(modelBuilder);
    }

    private void DisableCascadingDelete(ModelBuilder modelBuilder)
    {
        var relationShips = modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys());
        foreach (var item in relationShips)
        {
            item.DeleteBehavior = DeleteBehavior.Restrict;
        }
    }
}