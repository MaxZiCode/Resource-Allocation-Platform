using Microsoft.EntityFrameworkCore;
using Npgsql;
using ResourceAllocation.Contracts;
using ResourceAllocation.Domain.Models;
using ResourceAllocation.Infrastructure.EntityFramework;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

AddInfrastructureServices();

var app = builder.Build();

app.MapDefaultEndpoints();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapGet("/resource-types", async (ApplicationDbContext context) =>
{
    var resourceTypes = await context.ResourceTypes.ToListAsync();
    return resourceTypes;
})
.WithName("Resource Types");

app.Run();

void AddInfrastructureServices()
{
    var connectionString = builder.Configuration.GetConnectionString(ResourceNames.Databases.ResourceDb);

    var dataSourceBuilder = new NpgsqlDataSourceBuilder(connectionString);
    dataSourceBuilder.MapEnum<Status>();
    var dataSource = dataSourceBuilder.Build();

    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseNpgsql(dataSource).UseSnakeCaseNamingConvention()
    );
}