using Microsoft.EntityFrameworkCore;

namespace BreweryApi.Models
{
    public class BreweryDbContext : DbContext
    {
        public BreweryDbContext(DbContextOptions<BreweryDbContext> options) : base(options) { }

        public DbSet<BreweryDto> Breweries { get; set; }
    }
}
