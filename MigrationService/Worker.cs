using Microsoft.EntityFrameworkCore;
using ResourceAllocation.Infrastructure.EntityFramework;
using System.Diagnostics;

namespace ResourceAllocation.MigrationService;

public class Worker(
    IServiceProvider serviceProvider,
    IHostApplicationLifetime hostApplicationLifetime) : BackgroundService
{
    public const string ActivitySourceName = "Migrations";
    private static readonly ActivitySource s_activitySource = new(ActivitySourceName);

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        using var activity = s_activitySource.StartActivity("Migrating database", ActivityKind.Client);

        try
        {
            using var scope = serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            var strategy = dbContext.Database.CreateExecutionStrategy();
            await strategy.ExecuteAsync(async () =>
            {
                // Run migration in a transaction to avoid partial migration if it fails.
                await dbContext.Database.MigrateAsync(cancellationToken);
            });
        }
        catch (Exception ex)
        {
            activity?.AddException(ex);
            throw;
        }

        hostApplicationLifetime.StopApplication();
    }
}