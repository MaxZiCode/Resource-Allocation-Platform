using NpgsqlTypes;

namespace ResourceAllocation.Domain.Models;

public class Allocation
{
    public Guid Id { get; set; }

    public Guid ResourceId { get; set; }
    public Resource Resource { get; set; } = null!;

    public required NpgsqlRange<DateTime> Period { get; set; }
    public required Status Status { get; set; }
    public required DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? ExpiresAt { get; set; }
}