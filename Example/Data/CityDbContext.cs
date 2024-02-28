namespace Example.Data;

using Example.Models.Cities;
using Microsoft.EntityFrameworkCore;

public class CityDbContext
    : DbContext
{
    public CityDbContext(DbContextOptions<CityDbContext> options)
        : base(options)
    {
        Database.EnsureCreated();
    }

    public DbSet<City> Cities { get; set; }
}