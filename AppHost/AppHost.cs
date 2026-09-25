using Projects;
using ResourceAllocation.Contracts;

var builder = DistributedApplication.CreateBuilder(args);

var resourceDb = SetupDatabase(builder);

var api = builder.AddProject<ResourceAllocation_Api>("api");

var apiMigrations = api.AddEFMigrations("api-migrations")
    .WithMigrationsProject<ResourceAllocation_Infrastructure>()
    .RunDatabaseUpdateOnStart();

api.WaitForCompletion(apiMigrations);

builder.Build().Run();

static IResourceBuilder<PostgresDatabaseResource> SetupDatabase(IDistributedApplicationBuilder builder)
{
    var postgres = builder.AddPostgres("postgres")
        .WithPgAdmin(r => r.WithLifetime(ContainerLifetime.Persistent))
        .WithLifetime(ContainerLifetime.Persistent)
        .WithDataVolume();

    postgres.Resource.Annotations.Add(
        new EndpointAnnotation(System.Net.Sockets.ProtocolType.Tcp, uriScheme: "postgres", targetPort: 5432, port: 5432));

    var resourceDb = postgres.AddDatabase(ResourceNames.Databases.ResourceDb);
    return resourceDb;
}