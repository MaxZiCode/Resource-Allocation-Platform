using Microsoft.EntityFrameworkCore;
using ResourceAllocation.Domain.Models;

namespace ResourceAllocation.Infrastructure.EntityFramework;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<Allocation> Allocations => Set<Allocation>();
    public DbSet<Resource> Resources => Set<Resource>();
    public DbSet<ResourceType> ResourceTypes => Set<ResourceType>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.HasPostgresExtension("btree_gist");

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}