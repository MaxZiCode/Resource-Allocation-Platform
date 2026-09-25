param (
    [Parameter(Mandatory=$true, HelpMessage="Migration Name")]
    [string]$Name
)

$env:ASPNETCORE_ENVIRONMENT="EFMigrations"

dotnet ef migrations add $Name `
    --project Infrastructure/ResourceAllocation.Infrastructure.csproj `
    --startup-project Api/ResourceAllocation.Api.csproj `
    --output-dir EntityFramework/Migrations

Remove-Item env:ASPNETCORE_ENVIRONMENT