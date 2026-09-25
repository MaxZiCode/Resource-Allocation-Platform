using Aspire.Hosting.Docker.Resources.ServiceNodes;
using Projects;
using ResourceAllocation.Contracts;

var builder = DistributedApplication.CreateBuilder(args);

builder.AddDockerComposeEnvironment("docker-compose")
    .WithDashboard();

var postgres = AddPostgres(builder);
var resourceDb = postgres.AddDatabase(ResourceNames.Databases.ResourceDb);
var migrations = AddMigrations(builder, resourceDb);

builder.AddProject<ResourceAllocation_Api>("api")
    .WithReference(resourceDb)
    .WithExternalHttpEndpoints()
    .WaitForCompletion(migrations);

builder.Build().Run();

static IResourceBuilder<PostgresServerResource> AddPostgres(IDistributedApplicationBuilder builder)
{
    var volumeName = builder.Environment.EnvironmentName switch
    {
        "Development" => "resource-allocation-postgres-debug",
        "Production" => "resource-allocation-postgres",
        _ => throw new InvalidOperationException(
            $"Unsupported environment: {builder.Environment.EnvironmentName}")
    };

    var postgres = builder.AddPostgres("postgres")
        .WithPgAdmin(r => r.WithLifetime(ContainerLifetime.Persistent))
        .WithLifetime(ContainerLifetime.Persistent)
        .WithDataVolume(volumeName, isReadOnly: false)
        .PublishAsDockerComposeService((resource, service) =>
        {
            service.Healthcheck = new Healthcheck
            {
                Test =
                [
                    "CMD-SHELL",
                    $"pg_isready -U postgres -d {ResourceNames.Databases.ResourceDb}"
                ],
                Interval = "5s",
                Timeout = "5s",
                Retries = 10,
                StartPeriod = "10s"
            };
        }); ;

    postgres.Resource.Annotations.Add(
        new EndpointAnnotation(System.Net.Sockets.ProtocolType.Tcp, uriScheme: "postgres", targetPort: 5432, port: 5432));

    return postgres;
}

static IResourceBuilder<ProjectResource> AddMigrations(IDistributedApplicationBuilder builder, IResourceBuilder<PostgresDatabaseResource> resourceDb)
{
    var apiMigrations = builder.AddProject<ResourceAllocation_MigrationService>("api-migrations")
        .WithReference(resourceDb)
        .WaitFor(resourceDb)
        .PublishAsDockerComposeService((r, s) =>
        {
            s.Restart = "no";

            s.DependsOn[resourceDb.Resource.Parent.Name].Condition = "service_healthy";
        });

    return apiMigrations;
}