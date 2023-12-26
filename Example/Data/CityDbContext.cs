using Example.Models.Cities;
using Microsoft.EntityFrameworkCore;

namespace Example.Data;

public class CityDbContext : DbContext
{
    public CityDbContext(DbContextOptions<CityDbContext> options)
        : base(options)
    {
    }

    public DbSet<City> Cities { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
    }
}