using Gateway.Domain;
using Microsoft.EntityFrameworkCore;

namespace Gateway.DomainData;

public class GatewayDbContext(DbContextOptions<GatewayDbContext> options) : DbContext(options)
{
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<Package> Packages => Set<Package>();
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new OrderConfiguration());
        modelBuilder.ApplyConfiguration(new PackageConfiguration());
        base.OnModelCreating(modelBuilder);
    }
}
