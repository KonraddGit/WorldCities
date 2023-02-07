using Microsoft.EntityFrameworkCore;
using WorldCities.Domain.Entities;

namespace WorldCities.Persistence;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext()
        : base()
    {
    }

    public ApplicationDbContext(DbContextOptions options)
        : base(options)
    {
    }

    public DbSet<City> Cities => Set<City>();
    public DbSet<Country> Countries => Set<Country>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(ApplicationDbContext).Assembly);
    }
}