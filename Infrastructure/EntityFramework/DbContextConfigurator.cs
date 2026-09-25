using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure;
using ResourceAllocation.Domain.Models;

namespace ResourceAllocation.Infrastructure.EntityFramework;

public static class DbContextConfigurator
{
    public static void ConfigureNpgsqlDbContext(NpgsqlDbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.MapEnum<Status>();
        optionsBuilder.MigrationsAssembly(typeof(DbContextConfigurator).Assembly);
    }

    public static void ConfigureDbContext(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSnakeCaseNamingConvention();
    }
}