using ResourceAllocation.Contracts;
using ResourceAllocation.Infrastructure.EntityFramework;
using ResourceAllocation.MigrationService;

var builder = Host.CreateApplicationBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddHostedService<Worker>();
builder.Services.AddOpenTelemetry()
    .WithTracing(tracing => tracing.AddSource(Worker.ActivitySourceName));

var connectionString = builder.Configuration.GetConnectionString(ResourceNames.Databases.ResourceDb);

builder.Services.AddNpgsql<ApplicationDbContext>(connectionString,
    npgsqlOptionsAction: DbContextConfigurator.ConfigureNpgsqlDbContext,
    optionsAction: DbContextConfigurator.ConfigureDbContext
);

var host = builder.Build();
host.Run();