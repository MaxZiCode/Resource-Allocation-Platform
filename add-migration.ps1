param (
    [Parameter(Mandatory=$true, HelpMessage="Migration Name")]
    [string]$Name
)

dotnet ef migrations add $Name `
    --project Infrastructure/ResourceAllocation.Infrastructure.csproj `
    --startup-project Api/ResourceAllocation.Api.csproj `
    --output-dir EntityFramework/Migrations